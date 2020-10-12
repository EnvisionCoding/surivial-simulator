using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedInventory : MonoBehaviour {

    [Header("Items")]
    public Item log;
    public Item stick;
    public Item stone;
    public Item flint;
    public Item berry;
    public Item water;
    public Item herb;
    public Item campFire;

    [Header("Debugg")]
    public ManageDebug DebugManager;

    [Header("Instance")]
    public static ImprovedInventory instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    public int getItemCount(ITEMS type)
    {
        switch (type)
        {
            case ITEMS.LOG:
                return log.getCurrentAmount();
            case ITEMS.STICK:
                return stick.getCurrentAmount();
            case ITEMS.STONE:
                return stone.getCurrentAmount();
            case ITEMS.FLINT:
                return flint.getCurrentAmount();
            case ITEMS.HERB:
                return herb.getCurrentAmount();
            case ITEMS.WATER:
                return water.getCurrentAmount();
            case ITEMS.BERRY:
                return berry.getCurrentAmount();
            case ITEMS.CAMP_FIRE:
                return campFire.getCurrentAmount();
        }

        return -1;
    }

    public int getMaxStack(ITEMS type)
    {
        switch (type)
        {
            case ITEMS.LOG:
                return log.getMaxStack();
            case ITEMS.STICK:
                return stick.getMaxStack();
            case ITEMS.STONE:
                return stone.getMaxStack();
            case ITEMS.FLINT:
                return flint.getMaxStack();
            case ITEMS.HERB:
                return herb.getMaxStack();
            case ITEMS.WATER:
                return water.getMaxStack();
            case ITEMS.BERRY:
                return berry.getMaxStack();
            case ITEMS.CAMP_FIRE:
                return campFire.getMaxStack();
        }

        return -1;
    }

    public Item getItem(ITEMS type)
    {
        switch (type)
        {
            case ITEMS.LOG:
                return log;
            case ITEMS.STICK:
                return stick;
            case ITEMS.STONE:
                return stone;
            case ITEMS.FLINT:
                return flint;
            case ITEMS.HERB:
                return herb;
            case ITEMS.WATER:
                return water;
            case ITEMS.BERRY:
                return berry;
            case ITEMS.CAMP_FIRE:
                return campFire;
        }

        return null;
    }

    public void addItem(ITEMS insert, int amount)
    {
        int currentCount = 0;

        switch(insert)
        {
            case ITEMS.LOG:
                currentCount = log.getCurrentAmount();
                if (currentCount + amount <= log.getMaxStack())
                    log.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Logs.");
                }
                break;
            case ITEMS.STICK:
                currentCount = stick.getCurrentAmount();
                if (currentCount + amount <= stick.getMaxStack())
                    stick.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Sticks.");
                }
                break;
            case ITEMS.STONE:
                currentCount = stone.getCurrentAmount();
                if (currentCount + amount <= stone.getMaxStack())
                    stone.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Logs.");
                }

                break;
            case ITEMS.FLINT:
                currentCount = flint.getCurrentAmount();
                if (currentCount + amount <= flint.getMaxStack())
                    flint.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Logs.");
                }

                break;
            case ITEMS.HERB:
                currentCount = herb.getCurrentAmount();
                if (currentCount + amount <= herb.getMaxStack())
                    herb.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Logs.");
                }

                break;
            case ITEMS.WATER:
                currentCount = water.getCurrentAmount();
                if (currentCount + amount <= water.getMaxStack())
                    water.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Logs.");
                }

                break;
            case ITEMS.BERRY:
                currentCount = berry.getCurrentAmount();
                if (currentCount + amount <= berry.getMaxStack())
                    berry.setCurrentAmmount(true, amount);
                else
                {
                    if (DebugManager.debug_InventoryAmounts)
                        Debug.Log("Full On Logs.");
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
                
                //We check to see if have the required amount to craft, ei remove in the craftManager script
                log.setCurrentAmmount(false, amt);

                break;
            case ITEMS.STICK:

                
                break;

            case ITEMS.STONE:
                //We check to see if have the required amount to craft, ei remove in the craftManager script
                stone.setCurrentAmmount(false, amt);
                break;
            case ITEMS.FLINT:
                currentCount = flint.getCurrentAmount();

                break;
            case ITEMS.HERB:
                currentCount = herb.getCurrentAmount();

                if (currentCount - amt >= herb.getMinStack())
                    herb.setCurrentAmmount(false, amt);
                else
                {
                    if (DebugManager.debugInventory)
                        Debug.Log("No Herb");
                }

                break;
            case ITEMS.WATER:
                currentCount = water.getCurrentAmount();

                if (currentCount - amt >= water.getMinStack())
                    water.setCurrentAmmount(false, amt);
                else
                {
                    if (DebugManager.debugInventory)
                        Debug.Log("No Herb");
                }

                break;
            case ITEMS.BERRY:
                currentCount = berry.getCurrentAmount();

                //if (DebugManager.debugInventory)
                //    Debug.Log("Current BERRY Count: " + BERRY.getCurrentAmount());

                if (currentCount - amt >= berry.getMinStack())
                    berry.setCurrentAmmount(false, amt);
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




}
