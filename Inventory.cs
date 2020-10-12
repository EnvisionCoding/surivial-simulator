using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    [Header("Debug?")]
    public ManageDebug DebugManager;

    [SerializeField]
    public Item_Data itemData;
    private Item[] inventory = new Item[10];
    private List<Item> box = new List<Item>();
    private bool full;

    //Inventory Items;
    private Item LOG;
    [SerializeField]
    private Item STICK;

    private Item STONE;
    private Item FLINT;

    private Item HERB;
    private Item WATER;
    private Item BERRY;
    private Item RAW_MEAT;
    private Item COOKED_MEAT;

    private Item CAMP_FIRE;

    Item[] singleItem = new Item[1];
    int[] singleItemCost = new int[1];

    Item[] doubleItem = new Item[2];
    int[] doubleItemCost = new int[2];

    public static Inventory instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {

        LOG = 
            new Item(itemData.getIdData(ITEMS.LOG), 0, 
                     itemData.getMAxStack(ITEMS.LOG), "Log", 0, 0);
        STICK =
            new Item(itemData.getIdData(ITEMS.STICK), 0,
                     itemData.getMAxStack(ITEMS.STICK), "Stick", 0, 0);


        singleItem[0] = LOG;
        singleItemCost[0] = 2;
        STICK.setCraftReq(singleItem, singleItemCost);

        STONE =
            new Item(itemData.getIdData(ITEMS.STONE), 0,
                     itemData.getMAxStack(ITEMS.STONE), "Stone", 0, 0);

        FLINT =
            new Item(itemData.getIdData(ITEMS.FLINT), 0,
                     itemData.getMAxStack(ITEMS.FLINT), "Flint", 0, 0);

        singleItem[0] = STONE;
        singleItemCost[0] = 2;
        FLINT.setCraftReq(singleItem, singleItemCost);
        Debug.Log(FLINT.getCraftReq());

        HERB =
            new Item(itemData.getIdData(ITEMS.HERB), 0,
                     itemData.getMAxStack(ITEMS.HERB), "Herb", 0, 0);

        WATER =
            new Item(itemData.getIdData(ITEMS.WATER), 0,
                     itemData.getMAxStack(ITEMS.WATER), "Water", 0, 0);

        BERRY =
            new Item(itemData.getIdData(ITEMS.BERRY), 0,
                     itemData.getMAxStack(ITEMS.BERRY), "Berry", 0, 0);

        box.Add(LOG);
        box.Add(STICK);
        box.Add(STONE);
        box.Add(HERB);
        box.Add(WATER);
        box.Add(BERRY);
        box.Add(RAW_MEAT);
        box.Add(COOKED_MEAT);
	}
	
    public Item getItem(ITEMS type)
    {
        switch (type)
        {
            case ITEMS.LOG:
                return LOG;
            case ITEMS.STICK:
                return STICK;
            case ITEMS.STONE:
                return STONE;
            case ITEMS.FLINT:
                return FLINT;
            case ITEMS.HERB:
                return HERB;
            case ITEMS.WATER:
                return WATER;
            case ITEMS.BERRY:
                return BERRY;
        }

        return null;
    }

    public int getMaxStack(ITEMS type)
    {
        switch (type)
        {
            case ITEMS.LOG:
                return LOG.getMaxStack();
            case ITEMS.STICK:
                return STICK.getMaxStack();
            case ITEMS.STONE:
                return STONE.getMaxStack();
            case ITEMS.FLINT:
                return FLINT.getMaxStack();
            case ITEMS.HERB:
                return HERB.getMaxStack();
            case ITEMS.WATER:
                return WATER.getMaxStack();
            case ITEMS.BERRY:
                return BERRY.getMaxStack();
        }

        return -1;
    }

    public int getItemCount(ITEMS type)
    {
        switch(type)
        {
            case ITEMS.LOG:
                return LOG.getCurrentAmount();
            case ITEMS.STICK:
                return STICK.getCurrentAmount();
            case ITEMS.STONE:
                return STONE.getCurrentAmount();
            case ITEMS.FLINT:
                return FLINT.getCurrentAmount();
            case ITEMS.HERB:
                return HERB.getCurrentAmount();
            case ITEMS.WATER:
                return WATER.getCurrentAmount();
            case ITEMS.BERRY:
                return BERRY.getCurrentAmount();
        }

        return -1;
    }

    public void addItem(ITEMS insert, int amt)
    {
        int currentCount = 0;

        switch(insert)
        {
            case ITEMS.LOG:
                currentCount = LOG.getCurrentAmount();
                if (currentCount + amt <= LOG.getMaxStack())
                    LOG.setCurrentAmmount(true, amt);
                else
                {
                    //if(DebugManager.debugInventory)
                        Debug.Log("Full on Logs");
                }
                    
                break;
            case ITEMS.STICK:
                break;
            case ITEMS.STONE:
                currentCount = STONE.getCurrentAmount();
                if (currentCount + amt <= STONE.getMaxStack())
                    STONE.setCurrentAmmount(true, amt);
                else
                {
                    //if (DebugManager.debugInventory)
                        Debug.Log("Full on Stone");
                }
                   
                break;
            case ITEMS.FLINT:
                currentCount = FLINT.getCurrentAmount();
                if (currentCount + amt <= FLINT.getMaxStack())
                    FLINT.setCurrentAmmount(true, amt);
                else
                {
                   // if (DebugManager.debugInventory)
                        Debug.Log("Full on Flint");
                }
                    
                break;
            case ITEMS.HERB:
                currentCount = HERB.getCurrentAmount();
                if (currentCount + amt <= HERB.getMaxStack())
                    HERB.setCurrentAmmount(true, amt);
                else
                {
                    //if (DebugManager.debugInventory)
                        Debug.Log("Full on Herb");
                }
                   
                break;
            case ITEMS.WATER:
                currentCount = WATER.getCurrentAmount();
                if (currentCount + amt <= WATER.getMaxStack())
                    WATER.setCurrentAmmount(true, amt);
                else
                {
                   // if (DebugManager.debugInventory)
                        Debug.Log("Full on Water");
                }
                    
                break;
            case ITEMS.BERRY:
                currentCount = BERRY.getCurrentAmount();
                if (currentCount + amt <= BERRY.getMaxStack())
                    BERRY.setCurrentAmmount(true, amt);
                else
                {
                    //if (DebugManager.debugInventory)
                        Debug.Log("Full on Berries");
                }
                    
                break;
            default:
                //if (DebugManager.debugInventory)
                    Debug.Log("Shouldn't Happen");
                break;
        }

    }


    public void removeItem(ITEMS remove, int amt)
    {
        int currentCount = 0;

        switch (remove)
        {
            case ITEMS.LOG:
                currentCount = LOG.getCurrentAmount();
                
                break;
            case ITEMS.STICK:
                break;

            case ITEMS.STONE:
                currentCount = STONE.getCurrentAmount();
                
                break;
            case ITEMS.FLINT:
                currentCount = FLINT.getCurrentAmount();
                
                break;
            case ITEMS.HERB:
                currentCount = HERB.getCurrentAmount();

                if (currentCount - amt >= HERB.getMinStack())
                    HERB.setCurrentAmmount(false, amt);
                else
                {
                    if (DebugManager.debugInventory)
                        Debug.Log("No Herb");
                }

                break;
            case ITEMS.WATER:
                currentCount = WATER.getCurrentAmount();

                if (currentCount - amt >= WATER.getMinStack())
                    WATER.setCurrentAmmount(false, amt);
                else
                {
                    if (DebugManager.debugInventory)
                        Debug.Log("No Herb");
                }

                break;
            case ITEMS.BERRY:
                currentCount = BERRY.getCurrentAmount();

                //if (DebugManager.debugInventory)
                //    Debug.Log("Current BERRY Count: " + BERRY.getCurrentAmount());

                if (currentCount - amt >= BERRY.getMinStack())
                    BERRY.setCurrentAmmount(false, amt);
                else
                {
                    if (DebugManager.debugInventory)
                        Debug.Log("No Berries");
                }
                    
                
                
                break;
            default:
                if (DebugManager.debugInventory)
                    Debug.Log("Shouldn't Happen");
                break;
        }

    }

    public Item getLog()
    {
        return LOG;
    }

    public Item getStick()
    {
        return STICK;
    }

    public Item getStone()
    {
        return STONE;
    }

    public Item getFlint()
    {
        return FLINT;
    }

}
