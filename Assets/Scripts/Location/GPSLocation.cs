#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GPSLocation : MonoBehaviour
{
    [Header("Location Data")]
    float m_longitude = 0.0f;
    float m_latiude = 0.0f;
    IEnumerator m_TrackingCoroutine = null;

    static GPSLocation m_Instance = null;
    private void Awake()
    {
        if(m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        m_Instance = this;
        DontDestroyOnLoad(gameObject);

        StartLocationTracking();
    }

    public void StartLocationTracking()
    {
        if (m_TrackingCoroutine != null)
            return;
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
#endif
        m_TrackingCoroutine = GetLocation();
        StartCoroutine(m_TrackingCoroutine);
    }
    public void StopLocationTracking()
    {
        if (m_TrackingCoroutine == null)
            return;
        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
        m_TrackingCoroutine = null;
    }

    IEnumerator GetLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location Disabled by user");
            yield break;
        }

        // Start service before querying location
        Input.location.Start(1,1);
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.LogError("Timed out");
            yield break;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");
            yield break;
        }

        // Access granted and location value could be retrieved
        Debug.Log("Location: " + Input.location.lastData.latitude + " | " + Input.location.lastData.longitude + ", time:" + Input.location.lastData.timestamp);
        m_longitude = Input.location.lastData.longitude;
        m_latiude = Input.location.lastData.latitude;


    }


}
