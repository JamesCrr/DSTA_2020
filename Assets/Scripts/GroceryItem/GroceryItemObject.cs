using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryItemObject : MonoBehaviour
{
    baseGroceryItemSO m_SOReference = null;

    public void AssignSO(baseGroceryItemSO scriptableObject)
    {
        if (m_SOReference != null)
            return;
        m_SOReference = scriptableObject;
    }
    public baseGroceryItemSO GetAssignedSO() { return m_SOReference; }


}
