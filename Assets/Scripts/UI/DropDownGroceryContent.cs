using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropDownGroceryContent : MonoBehaviour
{
    public TextMeshProUGUI m_GorceryName;
    public Image m_GroceryImage;

    baseGroceryItemSO.GROCERY_ID m_GroceryID;

    public void Init(baseGroceryItemSO.GROCERY_ID groceryID)
    {
        m_GroceryID = groceryID;

        baseGroceryItemSO groceryInfo = GroceryItemDatabase.Instance.GetGroceryItem(m_GroceryID);
        m_GroceryImage.sprite = groceryInfo.GetItemImage();
        m_GorceryName.text = groceryInfo.GetItemName();
    }

    public void Click()
    {
        //go to the correct page
    }
}
