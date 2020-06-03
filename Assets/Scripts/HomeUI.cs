using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    [Header("Home UI Settings")]
    [SerializeField] GameObject UIItemPrefab;
    [SerializeField] GameObject PopularItemReference;
    [SerializeField] GameObject RecentItemReference;

    List<baseGroceryItemSO> ListOfGroceryItems = new List<baseGroceryItemSO>();
    int maxFoodItems = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxFoodItems = (int)baseGroceryItemSO.GROCERY_ID.TOTAL_GROCERY;

        // Loop through all the items
        // Add it into an internal list
        // Just for temporarily populating stuff in the home screen
        // Will be changed to use a database that stores items that are in stock
        for (int index = 0; index < maxFoodItems; ++index)
        {
            ListOfGroceryItems.Add(GroceryItemDatabase.Instance.GetGroceryItem((baseGroceryItemSO.GROCERY_ID)index));
        }

        // For testing purposes, randomly select a few items from the grocery store as popular items
        int startingNum = Random.Range(0, maxFoodItems - 5);
        for (int i = startingNum; i < 5; ++i)
        {
            GameObject ItemPrefab = GameObject.Instantiate(UIItemPrefab, PopularItemReference.transform);

            ItemPrefab.GetComponent<ItemPrefab>().Init(ListOfGroceryItems[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
