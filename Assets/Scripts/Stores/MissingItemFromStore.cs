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

    public void SetMissingItemType(baseGroceryItemSO.GROCERY_ID newID)
    {
        m_ItemImageObject.sprite = GroceryItemDatabase.Instance.GetGroceryItem(newID).GetItemImage();
        m_ItemNameText.text = GroceryItemDatabase.Instance.GetGroceryItem(newID).GetItemName();
    }
}
