using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum App_Scenes
{
    HomeUI,
    StoreDetail,
    StoreSelect,
    CartPanel,
    Search,
    TotalScenes
}

public class HomeUI : MonoBehaviour
{
    [Header("Home UI Settings")]
    [SerializeField] GameObject UIItemPrefab;
    [SerializeField] GameObject PopularItemReference;
    [SerializeField] GameObject RecentItemReference;
    public GameObject HomeObject;
    public GameObject SearchObject;

    List<baseGroceryItemSO> ListOfGroceryItems = new List<baseGroceryItemSO>();
    int maxFoodItems = 0;

    public static HomeUI Instance = null;

    public App_Scenes currentPage;

    public App_Scenes startingPage;

    private void Awake()
    {
        if (Instance != null && Instance != this.gameObject)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        // Always starts on home UI
        currentPage = App_Scenes.HomeUI;
        startingPage = App_Scenes.HomeUI;

        maxFoodItems = (int)baseGroceryItemSO.GROCERY_ID.TOTAL_GROCERY;

        // Loop through all the items
        // Add it into an internal list
        // Just for temporarily populating stuff in the home screen
        // Will be changed to use a database that stores items that are in stock
        for (int index = 0; index < maxFoodItems; ++index)
        {
            ListOfGroceryItems.Add(GroceryItemDatabase.Instance.GetGroceryItem((baseGroceryItemSO.GROCERY_ID)index));
        }

        InitRecentItems();

        InitPopularItems();
    }

    /// <summary>
    /// Populate the recent item category
    /// </summary>
    public void InitRecentItems()
    {
        /// Currently not being used yuet
        /// // no way to store information on that
    }

    /// <summary>
    /// Populate the popular items category
    /// </summary>
    public void InitPopularItems()
    {
        // For testing purposes, randomly select a few items from the grocery store as popular items
        int startingNum = Random.Range(0, maxFoodItems - 5);
        for (int i = startingNum; i < 5; ++i)
        {
            GameObject ItemPrefab = GameObject.Instantiate(UIItemPrefab, PopularItemReference.transform);

            ItemPrefab.GetComponent<ItemPrefab>().Init(ListOfGroceryItems[i]);
        }

    }

    public void DisableSearchObject()
    {
        // Clear the panel
        SearchObject.GetComponent<TypeAndSearchFood>().RemoveAllContentToPanel();
        SearchObject.GetComponent<TypeAndSearchFood>().m_TextInput.text = "";
        SearchObject.GetComponent<TypeAndSearchFood>().SetPanelActive(false);
    }

    public void EnableSearchObject()
    {
        SearchObject.GetComponent<TypeAndSearchFood>().SetPanelActive(true);
    }

    public void ToggleHome()
    {
        if (currentPage == App_Scenes.Search)
        {
            DisableSearchObject();

            startingPage = App_Scenes.HomeUI;

            //Set home UI to active
            HomeObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
