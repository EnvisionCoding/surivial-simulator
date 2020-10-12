using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour {

    public static CraftManager instance = null;

    public Recipe StickRecipe;
    public Recipe FlintRecipe;
    public Recipe CampFireRecipe;

    public Text craftingText;
    
    [SerializeField]
    private bool enoughLogs;
    [SerializeField]
    private bool enoughSticks;
    [SerializeField]
    private bool enoughStones;
    [SerializeField]
    private bool enoughFlint;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void craftStick()
    {
        if(canCraftSticks())
        {
            craftingText.color = Color.green;
            craftingText.text = "Crafted Sticks.";
            ImprovedInventory.instance.removeItem(ITEMS.LOG, 1);
            ImprovedInventory.instance.addItem(ITEMS.STICK, 2);
            Player.instance.updateInventoryText(ITEMS.LOG);
            Player.instance.updateInventoryText(ITEMS.STICK);
        }
        else
        {          
            Debug.Log("Cannot Craft");
        }
    }

    public bool canCraftSticks()
    {
        if(ImprovedInventory.instance.getItemCount(ITEMS.STICK) + ImprovedInventory.instance.stick.getAmountPerCraft() > ImprovedInventory.instance.getMaxStack(ITEMS.STICK))
        {
            return false;
        }
        else
        {
            if (ImprovedInventory.instance.getItemCount(ITEMS.LOG) >= StickRecipe.getSingleItemAmount())
            {
                return true;
            }
            else
            {
                craftingText.color = Color.red;
                craftingText.text = "Cannot Craft Sticks, missing logs.";
                return false;
            }
                
        }
    }

    public void craftFlint()
    {
        if (canCraftFlint())
        {
            Debug.Log("Crafting");
            craftingText.color = Color.green;
            craftingText.text = "Crafted Flint.";
            ImprovedInventory.instance.removeItem(ITEMS.STONE, 1);
            ImprovedInventory.instance.addItem(ITEMS.FLINT, 2);
            Player.instance.updateInventoryText(ITEMS.STONE);
            Player.instance.updateInventoryText(ITEMS.FLINT);
        }
        else
        {

            Debug.Log("Cannot Craft");
        }
    }

    public bool canCraftFlint()
    {
        if (ImprovedInventory.instance.getItemCount(ITEMS.FLINT) + ImprovedInventory.instance.flint.getAmountPerCraft() > ImprovedInventory.instance.getMaxStack(ITEMS.FLINT))
        {
            return false;
        }
        else
        {
            if (ImprovedInventory.instance.getItemCount(ITEMS.STONE) >= FlintRecipe.getSingleItemAmount())
            {
                return true;
            }
            else
            {
                craftingText.color = Color.red;
                craftingText.text = "Cannot Craft Flint, missing stones.";
                return false;
            }

        }

    }
}
