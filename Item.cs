using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    [Header("Item Info")]
    public ITEMS type;
    public int ID;
    public new string name;

    [Header("Stack Info")]
    public int min_stack;
    public int max_stack;
    public int currentAmount;

    [Header("Unimplimented")]
    public int sell_cost;
    public int buy_cost;

    [Header("Crafting")]
    public Item[] requiredItems;
    public int[] requiredAmountPerItem;
    public int numToGetPerCraft;

    void Start()
    {
        switch(type)
        {
            case ITEMS.LOG:
                ID = Item_Data.instance.getIdData(ITEMS.LOG);
                name = Item_Data.instance.getNameData(ITEMS.LOG);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.LOG);
                min_stack = 0;
                break;
            case ITEMS.STICK:
                ID = Item_Data.instance.getIdData(ITEMS.STICK);
                name = Item_Data.instance.getNameData(ITEMS.STICK);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.STICK);
                min_stack = 0;
                break;
            case ITEMS.STONE:
                ID = Item_Data.instance.getIdData(ITEMS.STONE);
                name = Item_Data.instance.getNameData(ITEMS.STONE);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.STONE);
                min_stack = 0;
                break;
            case ITEMS.FLINT:
                ID = Item_Data.instance.getIdData(ITEMS.FLINT);
                name = Item_Data.instance.getNameData(ITEMS.FLINT);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.FLINT);
                min_stack = 0;
                break;
            case ITEMS.BERRY:
                ID = Item_Data.instance.getIdData(ITEMS.BERRY);
                name = Item_Data.instance.getNameData(ITEMS.BERRY);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.BERRY);
                min_stack = 0;
                break;
            case ITEMS.HERB:
                ID = Item_Data.instance.getIdData(ITEMS.HERB);
                name = Item_Data.instance.getNameData(ITEMS.HERB);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.HERB);
                min_stack = 0;
                break;
            case ITEMS.WATER:
                ID = Item_Data.instance.getIdData(ITEMS.WATER);
                name = Item_Data.instance.getNameData(ITEMS.WATER);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.WATER);
                min_stack = 0;
                break;
            case ITEMS.CAMP_FIRE:
                ID = Item_Data.instance.getIdData(ITEMS.CAMP_FIRE);
                name = Item_Data.instance.getNameData(ITEMS.CAMP_FIRE);
                max_stack = Item_Data.instance.getMAxStack(ITEMS.CAMP_FIRE);
                min_stack = 0;
                break;
            default:
                break;
        }
            
    }

    public Item(int id, int min, int max, string name, int sell, int buy)
    {
        ID = id;
        min_stack = min;
        max_stack = max;
        this.name = name;
        sell_cost = sell;
        buy_cost = buy;
        currentAmount = 0;
    }

    public void setCraftReq(Item[] items, int[] amounts)
    {
        requiredItems = items;
        requiredAmountPerItem = amounts;
    }

    public int getAmountPerCraft()
    {
        return numToGetPerCraft;
    }

    public Item[] getCraftReq()
    {
        return requiredItems;
    }

    public int[] getCraftReqNum()
    {
        return requiredAmountPerItem;
    }

    public int getNumCraftReq()
    {
        return requiredItems.Length;
    }

    public int getNumCraftAmt()
    {
        return requiredAmountPerItem[0];
    }

    public int getId()
    {
        return ID;
    }

    public int getMinStack()
    {
        return min_stack;
    }

    public int getMaxStack()
    {
        return max_stack;
    }

    public string getName()
    {
        return name;
    }

    public int getSellPrice()
    {
        return sell_cost;
    }

    public int getBuyPrice()
    {
        return buy_cost;
    }

    public int getCurrentAmount()
    {
        return currentAmount;
    }

    public void setCurrentAmmount(bool increment, int amount)
    {
        if (increment)
        {
            if (currentAmount + amount < max_stack)
            {
                currentAmount += amount;
            }
            else
                currentAmount = max_stack;
        }
        else
        {
            if (currentAmount - amount >= min_stack)
            {
                currentAmount -= amount;
            }
            else
                currentAmount = 0;
        }
    }
}


