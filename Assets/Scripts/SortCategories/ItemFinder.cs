using System.Collections.Generic;
using UnityEngine;

public class ItemFinder : MonoBehaviour
{
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
}
