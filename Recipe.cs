using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour {

    public ITEMS type;
    public Item owningItem;
    public Item[] reqItems;
    public int[] reqAmounts;
    public float timeToCraft;
    public int AmountToCraft;

    public Recipe(Item toMake)
    {
        owningItem = toMake;
        reqItems = owningItem.getCraftReq();
        reqAmounts = owningItem.getCraftReqNum();
    }

	// Use this for initialization
	void Start () {
        switch(type)
        {
            case ITEMS.STICK:
                reqItems = owningItem.getCraftReq();
                reqAmounts = owningItem.getCraftReqNum();
                AmountToCraft = owningItem.getAmountPerCraft();
                break;
            case ITEMS.FLINT:
                reqItems = owningItem.getCraftReq();
                reqAmounts = owningItem.getCraftReqNum();
                AmountToCraft = owningItem.getAmountPerCraft();
                break;
            case ITEMS.CAMP_FIRE:
                reqItems = owningItem.getCraftReq();
                reqAmounts = owningItem.getCraftReqNum();
                AmountToCraft = owningItem.getAmountPerCraft();
                break;
            default:
                break;
        }
	}
    public Item[] getReqItems(ITEMS type)
    {
        return reqItems;
    }

    public int[] getReqAmounts()
    {
        return reqAmounts;
    }

    public int getSingleItemAmount()
    {
        return reqAmounts[0];
    }

    public void printRecipe()
    {
        Debug.Log("This recipe makes: " + owningItem.getName() + "\n"
            + "Requires: ");

        foreach (Item i in reqItems)
            Debug.Log(i.getName());
    }
}
