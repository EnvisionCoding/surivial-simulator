using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EventSystem : MonoBehaviour {

    [Header ("Singleton Self & References")]
    public static EventSystem instance = null;
    private Player pInstance;

    [Header ("Toggle Instant to Slow Attribute Changes")]
    [SerializeField]
    private bool foodOverTime = false;
    [SerializeField]
    private bool waterOverTime = false;
    [SerializeField]
    private bool healthOverTime = false;

    [Header ("Hunger Saturation")]
    bool toggleHungerTick = true;
    public static float maxWait = 3f;
    public float currentWait;

    [Header("UI EventSystem")]
    public GameObject Parent_UI;

    public GameObject UI_Crafting;
    public GameObject BTN_SwapToCrafting;
    public GameObject UI_Inventory;
    public GameObject BTN_SwapToInventory;
    public Text ErrorText;

    //Set up a Singleton
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        pInstance = Player.instance;
	}
	
	// Update is called once per frame
	void Update () {

        #region Manage HungerTick wait timer.
        if (toggleHungerTick)
        {
            pInstance.hungarTick = true;
        }
        else
        {
            currentWait -= Time.deltaTime;
            if(currentWait <= 0)
            {
                toggleHungerTick = true;
                currentWait = maxWait;
            }
        }
        #endregion


        if(pInstance.alive)
        {

            #region Hunger Event
            if (pInstance.hungarTick)
            {
                if (pInstance.hunger > 0)
                    pInstance.hunger -= pInstance.deterationRate * Time.deltaTime;
                else
                {
                    pInstance.hunger = 0;
                    pInstance.health -= (pInstance.deterationRate * 4) * Time.deltaTime;

                    //check to see if player died
                    if ((pInstance.health <= 0))
                    {
                        pInstance.alive = false;
                        pInstance.health = 0;
                    }
                }
            }
            #endregion

            #region Health Degerations via Out of Water
            if (pInstance.water <= 0)
            {
                pInstance.health -= (pInstance.deterationRate * 4) * Time.deltaTime;

                //check to see if player died
                if(pInstance.health <= 0)
                {
                    pInstance.alive = false;
                    pInstance.health = 0;
                }
            }
            #endregion
        }
    }

    public void healEvent(float healAmount)
    {

        if(healthOverTime)
        {

        }
        else
        {
            if (ImprovedInventory.instance.getItemCount(ITEMS.HERB) > 0)
            {
                if (ManageDebug.instance.debug_InventoryAmounts)
                    Debug.Log("I have: " + ImprovedInventory.instance.getItemCount(ITEMS.HERB) + " herbs left");

                if (pInstance.health + healAmount <= PlayerStats.instance.GetMaxHealth())
                {
                    if (pInstance.DebugManager.debugPlayer)
                        Debug.Log("Added " + healAmount + " to health");

                    pInstance.health += healAmount;

                    ImprovedInventory.instance.removeItem(ITEMS.HERB, 1);
                    pInstance.updateInventoryText(ITEMS.HERB);

                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("healed");
                }
                else if (pInstance.health == PlayerStats.instance.GetMaxHealth())
                {
                    //do nothing
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Already Full Health");
                }
                else
                {
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Set health to max");

                    pInstance.health = PlayerStats.instance.GetMaxHealth();

                    ImprovedInventory.instance.removeItem(ITEMS.HERB, 1);
                    pInstance.updateInventoryText(ITEMS.HERB);

                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("healed");
                }
            }
            else
            {
                if (ManageDebug.instance.debugPlayer)
                    Debug.Log("No herbs to heal with");
            }
        }
        

        pInstance.attemptToHeal = false;
    }

    public void drinkEvent(float waterAmount)
    {
        if(waterOverTime)
        {

        }
        else
        {
            if (ImprovedInventory.instance.getItemCount(ITEMS.WATER) > 0)
            {
                if (ManageDebug.instance.debug_InventoryAmounts)
                    Debug.Log("I have: " + ImprovedInventory.instance.getItemCount(ITEMS.WATER) + " water left");

                if (pInstance.water + waterAmount <= PlayerStats.instance.getMaxWater())
                {
                    if (pInstance.DebugManager.debugPlayer)
                        Debug.Log("Added " + waterAmount + " to water");

                    pInstance.water += waterAmount;
                    

                    ImprovedInventory.instance.removeItem(ITEMS.WATER, 1);
                    pInstance.updateInventoryText(ITEMS.WATER);

                    if (pInstance.DebugManager.debugPlayer)
                        Debug.Log("Drank");
                }
                else if (pInstance.water == PlayerStats.instance.getMaxWater())
                {
                    //do nothing
                    if (pInstance.DebugManager.debugPlayer)
                        Debug.Log("Already Full Water");
                }
            }

            pInstance.attemptToDrink = false;
        }
    }

    public void eatEvent(float foodAmount)
    {
        pInstance.hungarTick = false;

        if(foodOverTime)
        {

        }
        else
        {
            if (ImprovedInventory.instance.getItemCount(ITEMS.BERRY) > 0)
            {
                //Checks if Toggle of Player's Debug is on, if so lets debug
                if (pInstance.DebugManager.debugPlayer)
                    Debug.Log("I have: " + ImprovedInventory.instance.getItemCount(ITEMS.BERRY) + " berries left");

                if(pInstance.hunger + foodAmount <= PlayerStats.instance.GetMaxHungar())
                {
                    //Checks if Toggle of Player's Debug is on, if so lets debug
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Added " + foodAmount + " to hunger.");

                    //Increments / Decrements food and water accordingly
                    pInstance.hunger += foodAmount;
                    pInstance.water -= foodAmount;

                    //Removes the just consumed item and updates inventory UI
                    ImprovedInventory.instance.removeItem(ITEMS.BERRY, 1);
                    pInstance.updateInventoryText(ITEMS.BERRY);

                    //Checks if Toggle of Player's Debug is on, if so lets debug
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Yummy");
                }
                else if(pInstance.hunger + foodAmount >= PlayerStats.instance.GetMaxHungar())
                {
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Settings hunger to max hunger.");

                    // water to decrease = maxhuner - 
                    pInstance.hunger = PlayerStats.instance.GetMaxHungar();
                    pInstance.water -= foodAmount;

                   ImprovedInventory.instance.removeItem(ITEMS.BERRY, 1);
                    pInstance.updateInventoryText(ITEMS.BERRY);

                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Yummy");
                }
                else
                {
                    //Player is full and doesnt need to eat
                    //Checks if Toggle of Player's Debug is on, if so lets debug
                    if (pInstance.DebugManager.debugPlayer)
                        Debug.Log("Already Full Hungar");
                }
            }
            else
            {
                //Checks if Toggle of Player's Debug is on, if so lets debug
                if (pInstance.DebugManager.debugPlayer)
                    Debug.Log("No Food To Eat..");
            }


            pInstance.attemptToEat = false;

            updateHungerTickTimer();
        }
    }

    void updateHungerTickTimer()
    {
        currentWait = maxWait;
        toggleHungerTick = false;
    }

    public void SwitchToCrafting()
    {
        UI_Crafting.SetActive(true);
        BTN_SwapToCrafting.SetActive(false);

        UI_Inventory.SetActive(false);
        BTN_SwapToInventory.SetActive(true);
    }

    public void SwitchToInventory()
    {
        UI_Crafting.SetActive(false);
        BTN_SwapToCrafting.SetActive(true);

        UI_Inventory.SetActive(true);
        BTN_SwapToInventory.SetActive(false);
    }

    public void toggleCrafting()
    {
        //check to see if inventory UI is open, if it is switch to crafting UI
        if(UI_Inventory.activeSelf)
        {
            SwitchToCrafting();
        }
    }

    public void toggleInventory()
    {
        //check to see if craft UI is open, if it is switch to inventory UI
        if (UI_Crafting.activeSelf)
        {
            SwitchToInventory();
        }
    }
}
