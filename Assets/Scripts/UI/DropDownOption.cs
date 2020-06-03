using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DropDownOption : MonoBehaviour
{
    public GameObject m_DropDownContentPanel;
    public TextMeshProUGUI m_TitleText;

    public baseGroceryItemSO.CATEGORY m_Category;

    public void Init(baseGroceryItemSO.CATEGORY category)
    {
        m_TitleText.text = category.ToString();
        m_Category = category;

        m_DropDownContentPanel.SetActive(false);
    }

    public void AddContentToPanel(GameObject newContent, baseGroceryItemSO.GROCERY_ID groceryID)
    {
        newContent.transform.SetParent(m_DropDownContentPanel.transform);

        //init content stuff here
        DropDownGroceryContent dropDownGroceryContent = newContent.GetComponent<DropDownGroceryContent>();
        if (dropDownGroceryContent == null)
            return;

        dropDownGroceryContent.Init(groceryID);
    }

    public void RemoveAllContentToPanel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void DropDownSwitch()
    {
        m_DropDownContentPanel.SetActive(!m_DropDownContentPanel.activeSelf);

        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
