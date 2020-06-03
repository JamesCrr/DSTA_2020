using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSelectCanvas : MonoBehaviour
{
    [Header("Stores")]
    [SerializeField]
    List<StoreObject> m_ListOfStores = new List<StoreObject>();

    Canvas m_Canvas = null;

    public static StoreSelectCanvas Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_Canvas = GetComponent<Canvas>();
        //SetCanvasActive(false);
    }
    private void Start()
    {
        UpdateStoreMissingItems();
    }

    public void SetCanvasActive(bool newValue)
    {
        if (newValue)
        {
            m_Canvas.enabled = newValue;
            UpdateStoreMissingItems();
        }
        else
        {
            m_Canvas.enabled = newValue;

            HomeUI.Instance.HomeObject.SetActive(true);
        }
    }


    public void UpdateStoreMissingItems()
    {
        Dictionary<baseGroceryItemSO.GROCERY_ID, GroceryItemObject> myCart = ShoppingCart.Instance.GetCart();
        foreach (StoreObject store in m_ListOfStores)
        {
            List<baseGroceryItemSO.GROCERY_ID> listOfOutOfStockItems = store.GetOutOfStockItems();
            List<baseGroceryItemSO.GROCERY_ID> itemsMissingFromCart = new List<baseGroceryItemSO.GROCERY_ID>();
            // Check if out of stock items are in cart
            foreach (baseGroceryItemSO.GROCERY_ID itemID in listOfOutOfStockItems)
            {
                // If Missing, add to list
                if (myCart.ContainsKey(itemID))
                    itemsMissingFromCart.Add(itemID);
            }
            // Update Store List
            store.SetMissingListOfItemFromCart(itemsMissingFromCart);
            store.UpdateMissingItemsFromCart_UI();
        }
    }
}
