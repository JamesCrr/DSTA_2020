using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypeAndSearchFood : ItemFinder
{
    public TMP_InputField m_TextInput;

    TouchScreenKeyboard m_Keyboard;


    [Header("UI related")]
    public Transform m_DropDownParent;
    public GameObject m_DropDownPrefab;
    public GameObject m_DropDownContentPrefab;

    public void Start()
    {
        m_Keyboard = TouchScreenKeyboard.Open(m_TextInput.text, TouchScreenKeyboardType.Default);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || (m_Keyboard != null && m_Keyboard.status == TouchScreenKeyboard.Status.Done))
        {
            //get list of groceries with similar names
            List<baseGroceryItemSO> list = SearchGroceries(m_TextInput.text);

            if (list.Count == 0)
                return;

            Dictionary<baseGroceryItemSO.CATEGORY, List<baseGroceryItemSO>> categorySorterDictionary = new Dictionary<baseGroceryItemSO.CATEGORY, List<baseGroceryItemSO>>();
            foreach (baseGroceryItemSO item in list)
            {
                if (categorySorterDictionary.ContainsKey(item.GetCategoryType()))
                {
                    categorySorterDictionary[item.GetCategoryType()].Add(item);
                    continue;
                }

                categorySorterDictionary.Add(item.GetCategoryType(), new List<baseGroceryItemSO>());
                categorySorterDictionary[item.GetCategoryType()].Add(item);
            }

            RemoveAllContentToPanel();
            InitDropDownUI(categorySorterDictionary);
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }

    public void InitDropDownUI(Dictionary<baseGroceryItemSO.CATEGORY, List<baseGroceryItemSO>> grocerieList)
    {
        //check how many categories i have first
        //check how many items in each category
        //start initialising
        //each category must intialise also
        foreach (KeyValuePair<baseGroceryItemSO.CATEGORY, List<baseGroceryItemSO>> entry in grocerieList)
        {
            GameObject dropDownPrefab = Instantiate(m_DropDownPrefab, m_DropDownParent);

            DropDownOption dropDownOption = dropDownPrefab.GetComponent<DropDownOption>();
            if (dropDownOption == null)
                continue;

            dropDownOption.Init(entry.Key);

            foreach (baseGroceryItemSO grocery in entry.Value)
            {
                dropDownOption.AddContentToPanel(Instantiate(m_DropDownContentPrefab), grocery.GetEnumID());
            }
        }
    }

    public void RemoveAllContentToPanel()
    {
        foreach (Transform child in m_DropDownParent)
        {
            Destroy(child.gameObject);
        }
    }
}
