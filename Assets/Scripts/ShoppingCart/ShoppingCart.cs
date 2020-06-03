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
    [Header("Display Items")]
    [SerializeField]
    Transform m_CartDisplayItemParent = null;
    [SerializeField]
    GroceryItemObject m_GroceryItemPrefab = null;
    [Header("Total Price")]
    [SerializeField]
    TextMeshProUGUI m_TotalPriceText = null;

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
    }
    public void RemoveItem(baseGroceryItemSO.GROCERY_ID itemID)
    {
        if (!m_DictionaryCart.ContainsKey(itemID))
            return;
        // Destroy Object
        GroceryItemObject itemToRemove = m_DictionaryCart[itemID];
        itemToRemove.transform.parent = null;
        Destroy(itemToRemove.gameObject);
        m_DictionaryCart.Remove(itemID);
        // Scroll Height Recalculate
        RecalculateScrollHeight();
        // Price Recalculate
        RecalculatePrice();
    }
    public void SelectStorePressed()
    {
        DisableCartView();
        StoreSelectCanvas.Instance.SetCanvasActive(true);
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
            m_CartUIParent.DOAnchorPosY(-58, 0.5f, true);
            m_CartButtonImage.DOColor(Color.green, 0.4f);
            m_CartButtonText.text = "Continue Shopping";
        }
        else
        {
            m_CartUIParent.DOAnchorPosY(-1184, 0.5f, true);
            m_CartButtonImage.color = Color.white;
            m_CartButtonText.text = "View Cart";
        }
    }
    void DisableCartView()
    {
        m_CartViewActive = false;
        m_CartUIParent.DOAnchorPosY(-1184, 0.1f, true);
        m_CartButtonImage.color = Color.white;
        m_CartButtonText.text = "View Cart";
    }

    #endregion


    public Dictionary<baseGroceryItemSO.GROCERY_ID, GroceryItemObject> GetCart() { return m_DictionaryCart; }
    public int GetCartLength() { return m_DictionaryCart.Count; }
}
