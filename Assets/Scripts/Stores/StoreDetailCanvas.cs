using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreDetailCanvas : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    Image m_StoreImageObject = null;
    [SerializeField]
    TextMeshProUGUI m_StoreNameText = null;
    [SerializeField]
    TextMeshProUGUI m_StoreAddressText = null;

    [SerializeField]
    TextMeshProUGUI m_StoreDistanceText = null;
    [SerializeField]
    TextMeshProUGUI m_MissingItemsText = null;
    [SerializeField]
    Transform m_MissingItemsParent = null;
    [SerializeField]
    GameObject m_MissingItemPrefab = null;

    Canvas m_Canvas = null;
    StoreObject m_MyStoreObject = null;

    public static StoreDetailCanvas Instance = null;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        m_Canvas = GetComponent<Canvas>();
        SetCanvasActive(false);
    }


    public void SetCanvasActive(bool newValue)
    {
        if (newValue)
        {
            m_Canvas.enabled = true;
        }
        else
        {
            m_Canvas.enabled = false;
        }
    }
    public void SetNewStoreDetails(StoreObject newStoreObject)
    {
        m_MyStoreObject = newStoreObject;
        m_StoreImageObject.sprite = newStoreObject.GetStoreImage();
        m_StoreNameText.text = newStoreObject.GetStoreName();
        m_StoreAddressText.text = newStoreObject.GetStoreAddress();
        m_StoreDistanceText.text = newStoreObject.GetStoreDistance().ToString() + "m";

        m_MissingItemsText.text = newStoreObject.GetMissingTextObject().text;
        m_MissingItemsText.color = newStoreObject.GetMissingTextObject().color;

        // Unparent existing Objects first
        foreach (Transform child in m_MissingItemsParent.transform)
        {
            child.parent = null;
            Destroy(child.gameObject);
        }
        foreach (baseGroceryItemSO.GROCERY_ID missingItemID in newStoreObject.GetMissingItemsFromCart())
        {
            GameObject newMissingItem = Instantiate(m_MissingItemPrefab);
            newMissingItem.transform.parent = m_MissingItemsParent.transform;
            newMissingItem.GetComponent<MissingItemFromStore>().SetMissingItemType(missingItemID);
        }

    }

    public void MissingItemRemovedFromList()
    {
        StoreSelectCanvas.Instance.UpdateStoreMissingItems();
        SetNewStoreDetails(m_MyStoreObject);
    }
}
