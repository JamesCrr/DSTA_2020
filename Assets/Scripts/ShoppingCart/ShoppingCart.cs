using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCart : MonoBehaviour
{
    static public ShoppingCart Instance = null;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    Dictionary<baseGroceryItemSO.GROCERY_ID, GroceryItemObject> m_DictionaryCart = new Dictionary<baseGroceryItemSO.GROCERY_ID,GroceryItemObject>();
    
    [Header("UI Related")]
    [SerializeField]
    RectTransform m_CartUIParent = null;
    bool m_CartViewActive = false;
    [SerializeField]
    Image m_CartButtonImage = null;
    [SerializeField]
    TextMeshProUGUI m_CartButtonText = null;
    [SerializeField] GameObject m_CartDetailsText = null;

    [Header("Display Items")]
    [SerializeField]
    Transform m_CartDisplayItemParent = null;
    [SerializeField]
    GroceryItemObject m_GroceryItemPrefab = null;
    [Header("Total Price")]
    [SerializeField]
    TextMeshProUGUI m_TotalPriceText = null;

    [System.NonSerialized] public int NumOfItems = 0;

    private void Start()
    {
        RecalculateScrollHeight();
        RecalculatePrice();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            AddItem(baseGroceryItemSO.GROCERY_ID.GI_MILK);
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            RemoveItem(baseGroceryItemSO.GROCERY_ID.GI_MILK);
        }
    }

    public void AddItem(baseGroceryItemSO.GROCERY_ID itemID)
    {
        if (m_DictionaryCart.ContainsKey(itemID))
        {
            m_DictionaryCart[itemID].IncreaseItemCountButtonPressed();
            // Price Recalculate

            NumOfItems++;

            RecalculatePrice();
            return;
        }
        // New Entry
        GroceryItemObject newItem = Instantiate(m_GroceryItemPrefab);
        newItem.AssignSO(GroceryItemDatabase.Instance.GetGroceryItem(itemID));
        m_DictionaryCart.Add(itemID, newItem);
        // Parent new Entry under for Vertical Layout
        newItem.transform.parent = m_CartDisplayItemParent.transform;
        // Scroll Height Recalculate
        RecalculateScrollHeight();
        // Price Recalculate
        RecalculatePrice();

        NumOfItems++;
    }
    public void RemoveItem(baseGroceryItemSO.GROCERY_ID itemID)
    {
        if (!m_DictionaryCart.ContainsKey(itemID))
            return;
        else
        {
            // If it returns true means it successfully reduced by 1
            if(m_DictionaryCart[itemID].DecreaseItemCountButtonPressed())
            {
                NumOfItems--;

                RecalculatePrice();
            }
            // If it didnt that means theres no more items and it should be deleted
            // hopefully
            else
            {
                // Destroy Object
                GroceryItemObject itemToRemove = m_DictionaryCart[itemID];
                itemToRemove.transform.parent = null;
                Destroy(itemToRemove.gameObject);
                m_DictionaryCart.Remove(itemID);
                // Scroll Height Recalculate
                RecalculateScrollHeight();
                // Price Recalculate
                RecalculatePrice();

                NumOfItems--;

            }
        }
    }
    public void SelectStorePressed()
    {
        DisableCartView();
        StoreSelectCanvas.Instance.SetCanvasActive(true);

        HomeUI.Instance.currentPage = App_Scenes.StoreSelect;

        // If the user selected with their last scene as home UI
        if (HomeUI.Instance.startingPage == App_Scenes.HomeUI)
        {
            // Disable home UI
            HomeUI.Instance.HomeObject.SetActive(false);
        }
        // else if the user was last from the search scene
        else if (HomeUI.Instance.startingPage == App_Scenes.Search)
        {
            // Disable the search UI
            HomeUI.Instance.DisableSearchObject();
        }
    }

    public void ClearCart()
    {
        foreach (Transform child in m_CartDisplayItemParent.transform)
        {
            child.parent = null;
            Destroy(child.gameObject);
        }
        m_DictionaryCart.Clear();
        NumOfItems = 0;

        // Scroll Height Recalculate
        RecalculateScrollHeight();
        // Price Recalculate
        RecalculatePrice();
    }

    void RecalculatePrice()
    {
        float totalPrice = 0f;
        foreach(KeyValuePair<baseGroceryItemSO.GROCERY_ID, GroceryItemObject> entry in m_DictionaryCart)
        {
            totalPrice += entry.Value.GetAssignedSO().GetItemPrice() * entry.Value.GetItemCount();
        }
        m_TotalPriceText.text = "$" + totalPrice.ToString();
    }
    void RecalculateScrollHeight()
    {
        Vector2 size = m_CartDisplayItemParent.GetComponent<RectTransform>().sizeDelta;
        size.y = 138 * m_CartDisplayItemParent.childCount;
        m_CartDisplayItemParent.GetComponent<RectTransform>().sizeDelta = size;
    }

    #region UI Related Functions
    public void ToggleCartView()
    {
        m_CartViewActive = !m_CartViewActive;
        if(m_CartViewActive)
        {
            m_CartUIParent.DOAnchorPosY(-550, 0.5f, true);
            m_CartButtonImage.DOColor(Color.green, 1.0f);
            m_CartButtonText.text = "Continue Shopping";
            m_CartDetailsText.SetActive(false);

            HomeUI.Instance.currentPage = App_Scenes.CartPanel;

            ItemDetailCanvas.Instance.ToggleCanvas(false);
        }
        else
        {
            m_CartUIParent.DOAnchorPosY(-2946, 0.5f, true);
            m_CartButtonImage.color = Color.white;
            m_CartButtonText.text = "View Cart";
            m_CartDetailsText.SetActive(true);
        }
    }
    void DisableCartView()
    {
        m_CartViewActive = false;
        m_CartUIParent.DOAnchorPosY(-3200, 0.1f, true);
        m_CartButtonImage.color = Color.white;
        m_CartButtonText.text = "View Cart";
        m_CartDetailsText.SetActive(true);

    }

    #endregion


    public Dictionary<baseGroceryItemSO.GROCERY_ID, GroceryItemObject> GetCart() { return m_DictionaryCart; }
    public int GetCartLength() { return m_DictionaryCart.Count; }
}
