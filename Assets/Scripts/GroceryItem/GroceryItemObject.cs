using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroceryItemObject : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField]
    TextMeshProUGUI m_ItemNameText = null;
    [SerializeField]
    Image m_ItemImage = null;
    [SerializeField]
    TextMeshProUGUI m_ItemCounterText = null;

    baseGroceryItemSO m_SOReference = null;
    int m_ItemCounter = 1;

    public void AssignSO(baseGroceryItemSO scriptableObject)
    {
        if (m_SOReference != null)
            return;
        m_SOReference = scriptableObject;

        m_ItemCounter = 1;
        UpdateTextCounter();
        m_ItemNameText.text = m_SOReference.GetItemName();
        m_ItemImage.sprite = m_SOReference.GetItemImage();
    }
    public baseGroceryItemSO GetAssignedSO() { return m_SOReference; }


    public void IncreaseItemCountButtonPressed()
    {
        m_ItemCounter++;
        UpdateTextCounter();
    }
    public void DecreaseItemCountButtonPressed()
    {
        m_ItemCounter--;
        if(m_ItemCounter <= 0)
        {
            ShoppingCart.Instance.RemoveItem(m_SOReference.GetEnumID());
            return;
        }
        UpdateTextCounter();
    }
    public void RemoveItemButtonPressed()
    {
        ShoppingCart.Instance.RemoveItem(m_SOReference.GetEnumID());
    }
    void UpdateTextCounter()
    {
        m_ItemCounterText.text = "x" + m_ItemCounter.ToString();
    }

    public int GetItemCount() { return m_ItemCounter; }
}
