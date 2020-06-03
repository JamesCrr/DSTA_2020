using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GroceryItem")]
public class baseGroceryItemSO : ScriptableObject
{
    // Do NOT slot new items in between enums
    // Just keep extending the list
    public enum GROCERY_ID
    {
        GI_APPLE,
        GI_BANANA,

        GI_EGG,
        GI_MILK,
        
        GI_FRESH_CHICKEN,
        GI_FRESH_PORK,
        GI_FRESH_SALMON,
        
        GI_COFFEE_NESCAFE,
        GI_TEA_LIPONGREEN,

        TOTAL_GROCERY
    }

    [Header("Basic Info")]
    [SerializeField]
    Sprite m_ItemImage = null;
    [SerializeField]
    string m_ItemName = "Item Name";
    [SerializeField]
    float m_ItemPrice = 1.5f;
    [SerializeField]
    string m_ItemDescription = "";
    [SerializeField]
    GROCERY_ID m_EnumID = GROCERY_ID.GI_APPLE;

    [Header("Dietary")]
    [SerializeField]
    bool m_Edible = true;
    [SerializeField]
    bool m_Halal = false;
    [SerializeField]
    bool m_HealthierChoice = false;

    [Header("Nutrition Info")]
    [SerializeField]
    int m_Energy = 108;
    [SerializeField]
    int m_Cholesterol = 0;
    [SerializeField]
    int m_Sodium = 120;
    [SerializeField]
    float m_TransFat = 0.2f;
    [SerializeField]
    float m_SaturatedFat = 0.1f;
    [SerializeField]
    float m_Protein = 1.2f;
    [SerializeField]
    float m_Carbohydrate = 20.5f;
    [SerializeField]
    float m_DietaryFibre = 4.5f;


    public Sprite GetItemImage() { return m_ItemImage; }
    public string GetItemName() { return m_ItemName; }
    public string GetItemDescription() { return m_ItemDescription; }
    public float GetItemPrice() { return m_ItemPrice; }
    public GROCERY_ID GetEnumID() { return m_EnumID; }

    public bool IsItemHalal() { return m_Halal; }
    public bool IsItemHealthierChoice() { return m_HealthierChoice; }
    public bool IsItemEdible() { return m_Edible; }

    public int GetNutrition_Energy() { return m_Energy; }
    public int GetNutrition_Cholesterol() { return m_Cholesterol; }
    public int GetNutrition_Sodium() { return m_Sodium; }
    public float GetNutrition_TransFat() { return m_TransFat; }
    public float GetNutrition_SaturatedFat() { return m_SaturatedFat; }
    public float GetNutrition_Protein() { return m_Protein; }
    public float GetNutrition_Carbohydrate() { return m_Carbohydrate; }
    public float GetNutrition_DietaryFibre() { return m_DietaryFibre; }

}
