using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissingItemFromStore : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    Image m_ItemImageObject = null;
    [SerializeField]
    TextMeshProUGUI m_ItemNameText = null;

    baseGroceryItemSO.GROCERY_ID missingID = baseGroceryItemSO.GROCERY_ID.TOTAL_GROCERY;

    public void SetMissingItemType(baseGroceryItemSO.GROCERY_ID newID)
    {
        m_ItemImageObject.sprite = GroceryItemDatabase.Instance.GetGroceryItem(newID).GetItemImage();
        m_ItemNameText.text = GroceryItemDatabase.Instance.GetGroceryItem(newID).GetItemName();
        missingID = newID;
    }
    
    public void RemoveMissingItemPressed()
    {
        ShoppingCart.Instance.RemoveItem(missingID);
        StoreDetailCanvas.Instance.MissingItemRemovedFromList();

        transform.parent = null;
        Destroy(gameObject);
    }
}
