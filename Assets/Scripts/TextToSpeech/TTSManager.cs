﻿using IBM.Watson.TextToSpeech.V1;
using IBM.Watson.TextToSpeech.V1.Model;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Cloud.SDK;


public class TTSManager : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Space(10)]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string iamApikey;
    [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/text-to-speech/api\"")]
    [SerializeField]
    private string serviceUrl;
    private TextToSpeechService service;
    private string allisionVoice = "en-US_AllisonV3Voice";
    private string synthesizeText = "Hello, welcome to the Watson Unity SDK!";
    private string synthesizeMimeType = "audio/wav";
    private bool _textEntered = false;
    private AudioClip _recording = null;
    private byte[] audioStream = null;
    [SerializeField]
    private FlowManager flowManager;
    #endregion

    private static TTSManager _instance;

    public static TTSManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    void Update()
    {
        while (service != null && !service.IsListening)
        {
            if (audioStream != null && audioStream.Length > 0)
            {
                Log.Debug("ExampleTextToSpeech", "Audio stream of {0} bytes received!", audioStream.Length.ToString()); // Use audioStream and play audio
                _recording = WaveFile.ParseWAV("myClip", audioStream);
                PlayClip(_recording);
            }
            audioStream = null;
            StartListening(); // need to connect because service disconnect websocket after transcribing https://cloud.ibm.com/docs/text-to-speech?topic=text-to-speech-usingWebSocket#WSsend
        }
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(iamApikey))
        {
            throw new IBMException("Please add IAM ApiKey to the Iam Apikey field in the inspector.");
        }

        IamAuthenticator authenticator = new IamAuthenticator(apikey: iamApikey);

        while (!authenticator.CanAuthenticate())
        {
            yield return null;
        }

        service = new TextToSpeechService(authenticator);
        if (!string.IsNullOrEmpty(serviceUrl))
        {
            service.SetServiceUrl(serviceUrl);
        }

        Active = true;
        flowManager.enabled = true;
    }

    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleTextToSpeech.OnError()", "Error! {0}", error);
    }

    private void StartListening()
    {
        Log.Debug("ExampleTextToSpeech", "start-listening");
        service.Voice = allisionVoice;
        service.OnError = OnError;
        service.StartListening(OnSynthesize);
    }

    public bool Active
    {
        get { return service.IsListening; }
        set
        {
            if (value && !service.IsListening)
            {
                StartListening();
            }
            else if (!value && service.IsListening)
            {
                Log.Debug("ExampleTextToSpeech", "stop-listening");
                service.StopListening();
            }
        }
    }

    private void OnSynthesize(byte[] result)
    {
        Log.Debug("ExampleTextToSpeechV1", "Binary data received!");
        audioStream = ConcatenateByteArrays(audioStream, result);
    }

    #region Synthesize Without Websocket Connection
    private IEnumerator ExampleSynthesize()
    {
        byte[] synthesizeResponse = null;
        AudioClip clip = null;
        service.Synthesize(
            callback: (DetailedResponse<byte[]> response, IBMError error) =>
            {
                synthesizeResponse = response.Result;
                Log.Debug("ExampleTextToSpeechV1", "Synthesize done!");
                clip = WaveFile.ParseWAV("myClip", synthesizeResponse);
                PlayClip(clip);
            },
            text: synthesizeText,
            voice: allisionVoice,
            accept: synthesizeMimeType
        );

        while (synthesizeResponse == null)
            yield return null;

        yield return new WaitForSeconds(clip.length);
    }
    #endregion

    #region PlayClip
    private void PlayClip(AudioClip clip)
    {
        if (Application.isPlaying && clip != null)
        {
            GameObject audioObject = new GameObject("AudioObject");
            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            GameObject.Destroy(audioObject, clip.length);
        }
    }
    #endregion

    #region Concatenate Byte Arrays
    private byte[] ConcatenateByteArrays(byte[] a, byte[] b)
    {
        if (a == null || a.Length == 0)
        {
            return b;
        }
        else if (b == null || b.Length == 0)
        {
            return a;
        }
        else
        {
            List<byte> list1 = new List<byte>(a);
            List<byte> list2 = new List<byte>(b);
            list1.AddRange(list2);
            byte[] result = list1.ToArray();
            return result;
        }
    }
    #endregion

    #region Activate Text to Speech
    public void TextToSpeech(string text)
    {
        service.SynthesizeUsingWebsockets(text);
    }
    #endregion
}
