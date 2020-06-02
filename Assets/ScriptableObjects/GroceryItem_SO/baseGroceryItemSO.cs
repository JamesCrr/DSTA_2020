using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GroceryItem")]
public class baseGroceryItemSO : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField]
    Sprite m_ItemImage = null;
    [SerializeField]
    string m_ItemName = "Item Name";
    [SerializeField]
    float m_ItemPrice = 1.5f;
    [SerializeField]
    string m_ItemDescription = "";

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


    Sprite GetItemImage() { return m_ItemImage; }
    string GetItemName() { return m_ItemName; }
    string GetItemDescription() { return m_ItemDescription; }
    float GetItemPrice() { return m_ItemPrice; }

    bool IsItemHalal() { return m_Halal; }
    bool IsItemHealthierChoice() { return m_HealthierChoice; }
    bool IsItemEdible() { return m_Edible; }

    int GetNutrition_Energy() { return m_Energy; }
    int GetNutrition_Cholesterol() { return m_Cholesterol; }
    int GetNutrition_Sodium() { return m_Sodium; }
    float GetNutrition_TransFat() { return m_TransFat; }
    float GetNutrition_SaturatedFat() { return m_SaturatedFat; }
    float GetNutrition_Protein() { return m_Protein; }
    float GetNutrition_Carbohydrate() { return m_Carbohydrate; }
    float GetNutrition_DietaryFibre() { return m_DietaryFibre; }
}
