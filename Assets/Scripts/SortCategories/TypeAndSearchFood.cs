using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TypeAndSearchFood : ItemFinder
{
    public TMP_InputField m_TextInput;

    //ItemFinder m_ItemFinder = new ItemFinder();


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            List<baseGroceryItemSO> list = SearchGroceries(m_TextInput.text);


            string test = "";
            foreach(baseGroceryItemSO grocery in list)
            {
                test += grocery.GetItemName() + " ,";
            }

            Debug.Log(test);
        }

    }
}
