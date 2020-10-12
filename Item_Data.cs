using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Data : MonoBehaviour {

    public static Item_Data instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public string getNameData(ITEMS type)
    {
        switch(type)
        {
            case ITEMS.STONE:
                return "Stone";
            case ITEMS.FLINT:
                return "Flint";
            case ITEMS.LOG:
                return "Log";
            case ITEMS.STICK:
                return "Stick";
            case ITEMS.HERB:
                return "Herb";
            case ITEMS.WATER:
                return "Water";
            case ITEMS.BERRY:
                return "Berries";
            case ITEMS.RAW_MEAT:
                return "Raw Meat";
            case ITEMS.COOKED_MEAT:
                return "Cooked Meat";
            case ITEMS.CAMP_FIRE:
                return "Camp Fire";
            default:
                return "Default";
        }
    }

    public int getIdData(ITEMS type)
    {
       switch(type)
        {
            case ITEMS.STONE:
                return 0;
            case ITEMS.FLINT:
                return 1;
            case ITEMS.LOG:
                return 2;
            case ITEMS.STICK:
                return 3;
            case ITEMS.HERB:
                return 4;
            case ITEMS.WATER:
                return 5;
            case ITEMS.BERRY:
                return 6;
            case ITEMS.RAW_MEAT:
                return 7;
            case ITEMS.COOKED_MEAT:
                return 8;
            case ITEMS.CAMP_FIRE:
                return 9;
            default:
                return 10;
        }
    }

    public int getMAxStack(ITEMS type)
    {
        switch (type)
        {
            case ITEMS.STONE:
                return 10;
            case ITEMS.FLINT:
                return 20;
            case ITEMS.LOG:
                return 10;
            case ITEMS.STICK:
                return 20;
            case ITEMS.HERB:
                return 20;
            case ITEMS.WATER:
                return 3;
            case ITEMS.BERRY:
                return 10;
            case ITEMS.RAW_MEAT:
                return 5;
            case ITEMS.COOKED_MEAT:
                return 5;
            case ITEMS.CAMP_FIRE:
                return 1;
            default:
                return 999;
        }
    }
}
