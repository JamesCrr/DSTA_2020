using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreObject : MonoBehaviour
{
    public enum STORE_CROWDED
    {
        VERY_CROWDED,
        CROWDED,
        NOT_CROWDED,

        TOTAL_CROWDED
    }
    [Header("Store Info")]
    [SerializeField]
    string m_StoreName = null;
    [SerializeField]
    string m_StoreAddress = null;
    [SerializeField]
    Sprite m_StoreImage = null;
    [Header("Other Info")]
    [SerializeField]
    float m_StoreDistance = 200;
    [SerializeField]
    STORE_CROWDED m_StoreCrowded = STORE_CROWDED.CROWDED;
    [Header("Items that they don't have")]
    [SerializeField]
    List<baseGroceryItemSO.GROCERY_ID> m_ListOfOutItems = new List<baseGroceryItemSO.GROCERY_ID>();
    [Header("Derived from StoreSelectCanvas")]
    List<baseGroceryItemSO.GROCERY_ID> m_ListOfMissingItemsFromCart = new List<baseGroceryItemSO.GROCERY_ID>();

    [Header("Own UI Elements")]
    [SerializeField]
    TextMeshProUGUI m_StoreNameText = null;
    [SerializeField]
    Image m_StoreImageObject = null;
    [SerializeField]
    TextMeshProUGUI m_MissingItemsText = null;

    private void Awake()
    {
        // Update my own UI
        m_StoreNameText.text = m_StoreName;
        m_StoreImageObject.sprite = m_StoreImage;
        m_MissingItemsText.text = "0/0";
    }

    public string GetStoreName() { return m_StoreName; }
    public string GetStoreAddress() { return m_StoreAddress; }
    public Sprite GetStoreImage() { return m_StoreImage; }
    public float GetStoreDistance() { return m_StoreDistance; }
    public STORE_CROWDED GetStoreCrowded() { return m_StoreCrowded; }
    public List<baseGroceryItemSO.GROCERY_ID> GetOutOfStockItems() { return m_ListOfOutItems; }

    #region UI Related
    public void SetMissingListOfItemFromCart(List<baseGroceryItemSO.GROCERY_ID> newList)
    {
        m_ListOfMissingItemsFromCart = newList;
    }
    public void UpdateMissingItemsFromCart_UI()
    {
        // Update UI
        int itemsLeftCount = ShoppingCart.Instance.GetCartLength() - m_ListOfMissingItemsFromCart.Count;
        m_MissingItemsText.text = itemsLeftCount + "/" + ShoppingCart.Instance.GetCartLength();
        if (itemsLeftCount == ShoppingCart.Instance.GetCartLength())
            m_MissingItemsText.color = Color.green;
        else
            m_MissingItemsText.color = Color.red;
    }
    public List<baseGroceryItemSO.GROCERY_ID> GetMissingItemsFromCart() { return m_ListOfMissingItemsFromCart; }

    public TextMeshProUGUI GetMissingTextObject() { return m_MissingItemsText; }
    #endregion

    public void SelectButtonPressed()
    {
        StoreDetailCanvas.Instance.SetNewStoreDetails(this);
        StoreDetailCanvas.Instance.SetCanvasActive(true);
    }
}
