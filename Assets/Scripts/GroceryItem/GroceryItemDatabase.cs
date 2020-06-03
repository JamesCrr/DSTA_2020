using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryItemDatabase : MonoBehaviour
{
    public static GroceryItemDatabase Instance = null;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ConvertToDictionary();
    }
    [SerializeField]
    List<baseGroceryItemSO> m_ListOfScriptableObjects = new List<baseGroceryItemSO>();
    Dictionary<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> m_DictOfScriptableObjects = new Dictionary<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO>();

    void ConvertToDictionary()
    {
        foreach (baseGroceryItemSO item in m_ListOfScriptableObjects)
        {
            if(m_DictOfScriptableObjects.ContainsKey(item.GetEnumID()))
            {
                Debug.LogError("Duplicate Item Key in Database!!!");
                break;
            }
            m_DictOfScriptableObjects.Add(item.GetEnumID(), item);
        }
        m_ListOfScriptableObjects.Clear();
    }

    public baseGroceryItemSO GetGroceryItem(baseGroceryItemSO.GROCERY_ID dictID)
    {
        if (m_DictOfScriptableObjects.ContainsKey(dictID))
            return m_DictOfScriptableObjects[dictID];
        return null;
    }

    public Dictionary<baseGroceryItemSO.GROCERY_ID, baseGroceryItemSO> GetGroceryScriptableObjectDict()
    {
        return m_DictOfScriptableObjects;
    }


}
