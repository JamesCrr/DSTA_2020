using System.Collections.Generic;
using UnityEngine;

public class ItemFinder : MonoBehaviour
{
    private static ItemFinder _instance;

    public static ItemFinder Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public List<baseGroceryItemSO> SearchGroceries(string keywords)
    {
        Dictionary<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> groceryList = GroceryItemDatabase.Instance.GetGroceryScriptableObjectDict();

        List<baseGroceryItemSO> groceryWithKeyWords = new List<baseGroceryItemSO>();
        foreach (KeyValuePair<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> entry in groceryList)
        {
            baseGroceryItemSO groceryItem = entry.Value;
            if (groceryItem == null)
                continue;

            string groceryName = groceryItem.GetItemName().ToLower();
            if (groceryName.Contains(keywords.ToLower()))
            {
                groceryWithKeyWords.Add(groceryItem);
            }
        }

        return groceryWithKeyWords;
    }

    public List<baseGroceryItemSO> SearchGroceriesSTT(string sentence)
    {
        Dictionary<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> groceryList = GroceryItemDatabase.Instance.GetGroceryScriptableObjectDict();

        List<baseGroceryItemSO> groceryWithKeyWords = new List<baseGroceryItemSO>();
        foreach (KeyValuePair<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> entry in groceryList)
        {
            baseGroceryItemSO groceryItem = entry.Value;
            if (groceryItem == null)
                continue;

            string groceryName = groceryItem.GetItemName().ToLower();
            if (sentence.Contains(groceryName.ToLower()))
            {
                groceryWithKeyWords.Add(groceryItem);
            }
        }

        return groceryWithKeyWords;
    }

    public List<baseGroceryItemSO> SearchCategoriesSTT(string sentence)
    {
        Dictionary<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> groceryList = GroceryItemDatabase.Instance.GetGroceryScriptableObjectDict();

        List<baseGroceryItemSO> groceryWithKeyWords = new List<baseGroceryItemSO>();
        foreach (KeyValuePair<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> entry in groceryList)
        {
            baseGroceryItemSO groceryItem = entry.Value;
            if (groceryItem == null)
                continue;

            List<string> groceryKeyword = groceryItem.GetKeywordsString();
            foreach(string keyword in groceryKeyword)
            {
                if (sentence.Contains(keyword))
                {
                    groceryWithKeyWords.Add(groceryItem);
                    break;
                }
            }

        }

        return groceryWithKeyWords;
    }
}
