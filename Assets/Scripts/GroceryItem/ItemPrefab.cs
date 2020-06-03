using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPrefab : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] GameObject m_ItemName;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(baseGroceryItemSO groceryItem)
    {
        if (groceryItem == null)
            return;

        // Set up the prefab
        m_ItemName.GetComponent<TextMeshProUGUI>().text = groceryItem.GetItemName();

    }
}
