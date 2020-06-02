using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    static public ShoppingCart Instance = null;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public void AddItem(GroceryItemObject newGroceryItem)
    {

    }
    public void RemoveItem(GroceryItemObject itemToRemove)
    {

    }


}
