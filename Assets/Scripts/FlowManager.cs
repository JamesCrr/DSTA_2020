using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlowManager : MonoBehaviour
{
    private static FlowManager _instance;

    private FlowStates currState;
    [SerializeField]
    private Text speechToText;
    private int kmWillingToTravel;
    private bool amHalal;
    private baseGroceryItemSO itemSearched;
    private int amountBought;
    private bool waitingOnResponse = true;
    private List<baseGroceryItemSO> itemsBought;

    public static FlowManager Instance { get { return _instance; } }

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

    public enum FlowStates
    {
        state_travelRestrictions,
        state_whatTravelRestrictions,
        state_dietryRestrictions,
        state_confirmSettings,
        state_addItems,
        state_verifyItems,
        state_continueOrCheckout,
        state_verifyCart,
        state_addOrRemove,
        state_removeItems,
        state_verifyRemoveItems,
        state_storeSuggestion,
        state_planRoute
    }

    private void Start()
    {
        currState = FlowStates.state_travelRestrictions;
        TTSManager.Instance.TextToSpeech("Hello, do you have any travel restrictions?");
        kmWillingToTravel = -1;
        amHalal = false;
        amountBought = 0;
    }

    private void Update()
    {
        switch (currState) //OnStateUpdate
        {
            case FlowStates.state_travelRestrictions:
                if (speechToText.text.Contains("yes") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_whatTravelRestrictions);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    kmWillingToTravel = -1;
                    ChangeState(FlowStates.state_dietryRestrictions);
                }
                else if((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_whatTravelRestrictions:
                if(char.IsDigit(speechToText.text[0]) && speechToText.text.Contains("Final"))
                {
                    string tempDigit = "";
                    foreach(char c in speechToText.text)
                    {
                        if (char.IsDigit(c))
                            tempDigit += c;
                        else
                            break;
                    }
                    kmWillingToTravel = int.Parse(tempDigit);
                    ChangeState(FlowStates.state_dietryRestrictions);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_dietryRestrictions:
                if (speechToText.text.Contains("hello") && speechToText.text.Contains("Final")) //halal is detected as hello in the ibm stt
                {
                    amHalal = true;
                    ChangeState(FlowStates.state_confirmSettings);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    amHalal = false;
                    ChangeState(FlowStates.state_confirmSettings);

                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_confirmSettings:
                if (speechToText.text.Contains("yes") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_addItems);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_travelRestrictions);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_addItems:
                if (!waitingOnResponse)
                {
                    List<baseGroceryItemSO> itemsByName = ItemFinder.Instance.SearchGroceriesSTT(speechToText.text);
                    List<baseGroceryItemSO> itemsByCategory = ItemFinder.Instance.SearchCategoriesSTT(speechToText.text);
                    List<baseGroceryItemSO> itemsSearched = new List<baseGroceryItemSO>();


                    foreach (baseGroceryItemSO item in itemsByName)
                    {
                        foreach (baseGroceryItemSO categoryItem in itemsByCategory)
                        {
                            if (categoryItem == item)
                            {
                                itemsSearched.Add(item);
                            }
                        }
                    }
                    if (itemsSearched.Count == 0)
                    {
                        itemsSearched = itemsByName;
                        foreach (baseGroceryItemSO item in itemsByCategory)
                        {
                            itemsSearched.Add(item);
                        }
                    }
                    if (itemsSearched.Count == 0)
                    {
                        speechToText.text = "";
                        TTSManager.Instance.TextToSpeech("Sorry, I could not find any matches from your search, please try again.");
                    }
                    else if (itemsSearched.Count == 1)
                    {
                        itemSearched = itemsSearched[0];
                        ChangeState(FlowStates.state_verifyItems);
                    }
                    else
                    {
                        string temp = "I have found multiple items";
                        foreach (baseGroceryItemSO item in itemsSearched)
                        {
                            temp += ". ";
                            temp += item.GetItemName();
                        }
                        temp += ". Which item would you like? You can also search for something else if you'd like.";
                        TTSManager.Instance.TextToSpeech(temp);
                    }
                    waitingOnResponse = true;
                    speechToText.text = "default";
                }
                else if(speechToText.text.Contains("Final"))
                {
                    waitingOnResponse = false;
                }
                break;
            case FlowStates.state_verifyItems:
                if (char.IsDigit(speechToText.text[0]) && speechToText.text.Contains("Final"))
                {
                    string tempDigit = "";
                    foreach (char c in speechToText.text)
                    {
                        if (char.IsDigit(c))
                            tempDigit += c;
                        else
                            break;
                    }
                    amountBought = int.Parse(tempDigit);
                    //add amountBought amount of itemSearched into cart (function) 
                    itemSearched.SetAmountInCart(amountBought);
                    itemsBought.Add(itemSearched);
                    ChangeState(FlowStates.state_continueOrCheckout);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_addItems);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_continueOrCheckout:
                if (speechToText.text.Contains("yes") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_addItems);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_verifyCart);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_verifyCart:
                if (speechToText.text.Contains("yes") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_addOrRemove);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_storeSuggestion);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_addOrRemove:
                if (speechToText.text.Contains("add") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_addItems);
                }
                else if (speechToText.text.Contains("remove") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_removeItems);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_removeItems:
                if (!waitingOnResponse)
                {
                    List<baseGroceryItemSO> itemsByName = SearchGroceriesFromCartSTT(speechToText.text);
                    List<baseGroceryItemSO> itemsByCategory = SearchCategoriesFromCartSTT(speechToText.text);
                    List<baseGroceryItemSO> itemsSearched = new List<baseGroceryItemSO>();

                    foreach (baseGroceryItemSO item in itemsByName)
                    {
                        foreach (baseGroceryItemSO categoryItem in itemsByCategory)
                        {
                            if (categoryItem == item)
                            {
                                itemsSearched.Add(item);
                            }
                        }
                    }
                    if (itemsSearched.Count == 0)
                    {
                        itemsSearched = itemsByName;
                        foreach (baseGroceryItemSO item in itemsByCategory)
                        {
                            itemsSearched.Add(item);
                        }
                    }
                    if (itemsSearched.Count == 0)
                    {
                        speechToText.text = "";
                        TTSManager.Instance.TextToSpeech("Sorry, I could not find any matches from your search, please try again.");
                    }
                    else if (itemsSearched.Count == 1)
                    {
                        itemSearched = itemsSearched[0];
                        ChangeState(FlowStates.state_verifyRemoveItems);
                    }
                    else
                    {
                        string temp = "I have found multiple items";
                        foreach (baseGroceryItemSO item in itemsSearched)
                        {
                            temp += ". ";
                            temp += item.GetItemName();
                        }
                        temp += ". Which item would you like to remove? You can also search for something else if you'd like.";
                        TTSManager.Instance.TextToSpeech(temp);
                    }
                    waitingOnResponse = true;
                    speechToText.text = "default";
                }
                else if (speechToText.text.Contains("Final"))
                {
                    waitingOnResponse = false;
                }
                break;
            case FlowStates.state_verifyRemoveItems:
                if (char.IsDigit(speechToText.text[0]) && speechToText.text.Contains("Final"))
                {
                    string tempDigit = "";
                    foreach (char c in speechToText.text)
                    {
                        if (char.IsDigit(c))
                            tempDigit += c;
                        else
                            break;
                    }
                    amountBought = int.Parse(tempDigit);
                    //remove amountBought amount of itemSearched into cart (function) 
                    itemSearched.SetAmountInCart(itemSearched.GetAmountInCart() - amountBought);
                    if (itemSearched.GetAmountInCart() <= 0)
                        itemsBought.Remove(itemSearched);
                    ChangeState(FlowStates.state_verifyCart);
                }
                else if (speechToText.text.Contains("no") && speechToText.text.Contains("Final"))
                {
                    ChangeState(FlowStates.state_removeItems);
                }
                else if ((speechToText.text.Contains("repeat") || speechToText.text.Contains("again")) && speechToText.text.Contains("Final"))
                {
                    ChangeState(currState);
                }
                break;
            case FlowStates.state_storeSuggestion:
                break;
            case FlowStates.state_planRoute:
                break;
            default:
                break;
        }
    }

    public void ChangeState(FlowStates state)
    {
        string temp;
        switch (currState) //OnStateExit
        {
            case FlowStates.state_travelRestrictions:
            case FlowStates.state_whatTravelRestrictions:
            case FlowStates.state_dietryRestrictions:
            case FlowStates.state_confirmSettings:
            case FlowStates.state_addItems:
            case FlowStates.state_verifyItems:
            case FlowStates.state_continueOrCheckout:
            case FlowStates.state_verifyCart:
            case FlowStates.state_addOrRemove:
            case FlowStates.state_removeItems:
            case FlowStates.state_verifyRemoveItems:
            case FlowStates.state_storeSuggestion:
            case FlowStates.state_planRoute:
                speechToText.text = "default";
                waitingOnResponse = true;
                break;
            default:
                break;
        }
        currState = state;
        switch (currState) //OnStateEnter
        {
            case FlowStates.state_travelRestrictions:
                TTSManager.Instance.TextToSpeech("Hello, do you have any travel restrictions?");
                break;
            case FlowStates.state_whatTravelRestrictions:
                TTSManager.Instance.TextToSpeech("How far in kilometers are you willing to travel?");
                break;
            case FlowStates.state_dietryRestrictions:
                TTSManager.Instance.TextToSpeech("What dietry restrictions do you have? If you don't have any, say no");
                break;
            case FlowStates.state_confirmSettings:
                temp = "So you are willing to travel ";
                if (kmWillingToTravel == -1)
                    temp += "any amount of";
                else
                    temp += kmWillingToTravel.ToString();
                temp += " kilometers and you ";
                if (amHalal)
                    temp += "are halal. ";
                else
                    temp += "have no dietry restrictions. ";
                temp += "Did I get that right?";
                TTSManager.Instance.TextToSpeech(temp);
                break;
            case FlowStates.state_addItems:
                TTSManager.Instance.TextToSpeech("What would you like to buy?");
                break;
            case FlowStates.state_verifyItems:
                temp = "The item I found is ";
                temp += itemSearched.GetItemName();
                temp += " and it costs ";
                temp += itemSearched.GetDollars().ToString();
                temp += " dollars and ";
                temp += itemSearched.GetCents().ToString();
                temp += " cents. How many would you like to buy? If you do not want it, say no.";
                TTSManager.Instance.TextToSpeech(temp);
                break;
            case FlowStates.state_continueOrCheckout:
                TTSManager.Instance.TextToSpeech("Would you like to continue shopping?");
                break;
            case FlowStates.state_verifyCart:
                temp = "Currently, your cart has";
                foreach(baseGroceryItemSO item in itemsBought)
                {
                    temp += ". ";
                    temp += item.GetAmountInCart().ToString();
                    temp += " ";
                    temp += item.GetItemName();
                }
                temp += "Would you like to change anything in your cart?";
                TTSManager.Instance.TextToSpeech(temp);
                break;
            case FlowStates.state_addOrRemove:
                TTSManager.Instance.TextToSpeech("Would you like to add or remove items?");
                break;
            case FlowStates.state_removeItems:
                TTSManager.Instance.TextToSpeech("What would you like to remove?");
                break;
            case FlowStates.state_verifyRemoveItems:
                temp = "The item I found is ";
                temp += itemSearched.GetItemName();
                temp += " and it costs ";
                temp += itemSearched.GetDollars().ToString();
                temp += " dollars and ";
                temp += itemSearched.GetCents().ToString();
                temp += " cents and you have ";
                temp += itemSearched.GetAmountInCart().ToString();
                temp += " amount in your cart. How many would you like to remove? If you do not want to remove it, say no.";
                TTSManager.Instance.TextToSpeech(temp);
                break;
            case FlowStates.state_storeSuggestion:
                break;
            case FlowStates.state_planRoute:
                break;
            default:
                break;
        }
    }

    List<baseGroceryItemSO> SearchGroceriesFromCartSTT(string sentence)
    {
        List<baseGroceryItemSO> groceryWithKeyWords = new List<baseGroceryItemSO>();
        foreach (baseGroceryItemSO item in itemsBought)
        {
            string groceryName = item.GetItemName().ToLower();
            if (sentence.Contains(groceryName.ToLower()))
            {
                groceryWithKeyWords.Add(item);
            }
        }

        return groceryWithKeyWords;
    }

    List<baseGroceryItemSO> SearchCategoriesFromCartSTT(string sentence)
    {
        List<baseGroceryItemSO> groceryWithKeyWords = new List<baseGroceryItemSO>();
        foreach (baseGroceryItemSO item in itemsBought)
        {
            List<string> groceryKeyword = item.GetKeywordsString();
            foreach (string keyword in groceryKeyword)
            {
                if (sentence.Contains(keyword))
                {
                    groceryWithKeyWords.Add(item);
                    break;
                }
            }
        }

        return groceryWithKeyWords;
    }
}
