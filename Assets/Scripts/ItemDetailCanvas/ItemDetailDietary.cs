using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailDietary : MonoBehaviour
{
    [SerializeField]
    GameObject m_HealthierChoice = null;
    [SerializeField]
    GameObject m_Halal = null;

    public void SetHalal(bool newValue)
    {
        m_Halal.SetActive(newValue);
    }
    public void SetHealhier(bool newValue)
    {
        m_HealthierChoice.SetActive(newValue);
    }
}
