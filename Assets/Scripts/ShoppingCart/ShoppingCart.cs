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
    [SerializeField]
    GroceryItemObject m_GroceryItemPrefab = null;

    Dictionary<baseGroceryItemSO.GROCERY_ID, List<GroceryItemObject>> m_DictionaryCart = new Dictionary<baseGroceryItemSO.GROCERY_ID, List<GroceryItemObject>>();


    public void AddItem(baseGroceryItemSO newGrocerySO)
    {
        GroceryItemObject newItem = Instantiate(m_GroceryItemPrefab);
        newItem.AssignSO(newGrocerySO);
        if (!m_DictionaryCart.ContainsKey(newGrocerySO.GetEnumID()))
            m_DictionaryCart.Add(newGrocerySO.GetEnumID(), new List<GroceryItemObject>());
        m_DictionaryCart[newGrocerySO.GetEnumID()].Add(newItem);
    }
    public void RemoveItem(baseGroceryItemSO GrocerySO)
    {
        if (!m_DictionaryCart.ContainsKey(GrocerySO.GetEnumID()))
            return;
        if (m_DictionaryCart[GrocerySO.GetEnumID()].Count < 1)
            return;
        m_DictionaryCart[GrocerySO.GetEnumID()].RemoveAt(0);
    }

    public Dictionary<baseGroceryItemSO.GROCERY_ID, List<GroceryItemObject>> GetCart() { return m_DictionaryCart; }
}
