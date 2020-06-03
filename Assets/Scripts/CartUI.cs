using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartUI : MonoBehaviour
{
    [Header("Cart UI Settings")]
    [SerializeField] GameObject CartTextObject;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Gets the current count of objects in the Cart
        int CartCount = ShoppingCart.Instance.NumOfItems;

        string NewText = CartCount.ToString() + " items in Cart";

        CartTextObject.GetComponent<TextMeshProUGUI>().text = NewText;
    }

    public void GoToCart()
    {

    }
}
