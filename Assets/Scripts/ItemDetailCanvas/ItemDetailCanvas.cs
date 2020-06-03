using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailCanvas : MonoBehaviour
{
    static public ItemDetailCanvas Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        // ToggleCanvas(false);
        // SetNewItemDetail(baseGroceryItemSO.GROCERY_ID.GI_BANANA);
    }

    [Header("Canvas Parent")]
    [SerializeField]
    Canvas m_CanvasCom = null;

    [Header("UI Elements")]
    [SerializeField]
    TextMeshProUGUI m_ItemName = null;
    [SerializeField]
    Image m_ItemImage = null;
    [SerializeField]
    TextMeshProUGUI m_ItemPrice = null;

    [Header("Dietary")]
    [SerializeField]
    ItemDetailDietary m_ItemDietary = null;

    [Header("Nutri Info")]
    [SerializeField]
    GameObject m_ItemNutritionParent = null;
    [SerializeField]
    TextMeshProUGUI m_ItemEnergy = null;
    [SerializeField]
    TextMeshProUGUI m_ItemTransfat = null;
    [SerializeField]
    TextMeshProUGUI m_ItemSaturatedFat = null;
    [SerializeField]
    TextMeshProUGUI m_ItemCholesterol = null;
    [SerializeField]
    TextMeshProUGUI m_ItemSodium = null;
    [SerializeField]
    TextMeshProUGUI m_ItemProtein = null;
    [SerializeField]
    TextMeshProUGUI m_ItemCarbohydrate = null;
    [SerializeField]
    TextMeshProUGUI m_ItemDietaryFibre = null;

    baseGroceryItemSO currentItem;


    public void ToggleCanvas(bool newValue)
    {
        m_CanvasCom.enabled = newValue;

        //// If it came from the home UI
        //if (HomeUI.Instance.currentScene == App_Scenes.HomeUI)
        //{

        //}
        //// if it came from search UI
        //else if (HomeUI.Instance.currentScene == App_Scenes.Search)
        //{

        //}

        // closing the page
        if (newValue == false)
            currentItem = null;
    }

    public void OpenItemDetails(baseGroceryItemSO itemData)
    {
        ToggleCanvas(true);
        SetNewItemDetail(itemData.GetEnumID());
        //HomeUI.Instance.currentScene = App_Scenes.ItemCanvas;
    }

    public void OpenItemDetails(baseGroceryItemSO.GROCERY_ID enumID)
    {
        ToggleCanvas(true);
        SetNewItemDetail(enumID);

    }

    public void AddItemToCart()
    {
        ShoppingCart.Instance.AddItem(currentItem.GetEnumID());
    }

    public void SetNewItemDetail(baseGroceryItemSO.GROCERY_ID newItemID)
    {
        baseGroceryItemSO newItem = GroceryItemDatabase.Instance.GetGroceryItem(newItemID);

        currentItem = newItem;

        m_ItemName.text = newItem.GetItemName();
        m_ItemPrice.text = "$" + newItem.GetItemPrice().ToString();
        m_ItemImage.sprite = newItem.GetItemImage();

        if (!newItem.IsItemEdible())
        {
            m_ItemNutritionParent.SetActive(false);
            m_ItemDietary.gameObject.SetActive(false);
            return;
        }
        if(!newItem.IsItemHalal() && !newItem.IsItemHealthierChoice())
            m_ItemDietary.gameObject.SetActive(false);
        else
        {
            m_ItemDietary.SetHalal(newItem.IsItemHalal());
            m_ItemDietary.SetHealhier(newItem.IsItemHealthierChoice());
        }

        m_ItemEnergy.text = newItem.GetNutrition_Energy().ToString() + "kcal";
        m_ItemTransfat.text = newItem.GetNutrition_TransFat().ToString() + "g";
        m_ItemSaturatedFat.text = newItem.GetNutrition_SaturatedFat().ToString() + "g";
        m_ItemCholesterol.text = newItem.GetNutrition_Cholesterol().ToString() + "mg";
        m_ItemSodium.text = newItem.GetNutrition_Sodium().ToString() + "mg";
        m_ItemProtein.text = newItem.GetNutrition_Protein().ToString() + "g";
        m_ItemCarbohydrate.text = newItem.GetNutrition_Carbohydrate().ToString() + "g";
        m_ItemDietaryFibre.text = newItem.GetNutrition_DietaryFibre().ToString() + "g";
    }
}
