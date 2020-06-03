using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStorage : MonoBehaviour
{
    public static UserStorage Instance = null;

    // Set up instance
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public List<List<baseGroceryItemSO>> GroceryHistory = new List<List<baseGroceryItemSO>>();
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    ///  For adding a new list to the Grocery History
    /// </summary>
    /// <param name="GroceryList">Grocery List being added</param>
    public void AddNewGroceryList(List<baseGroceryItemSO> GroceryList)
    {
        // Add into the history
        GroceryHistory.Add(GroceryList);
    }


}
