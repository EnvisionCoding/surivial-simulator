using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : MonoBehaviour {

    public static Player instance = null;

    [Header("Debug?")]
    public ManageDebug DebugManager;

    [Header ("Character Setup")]
    //Player attributes
    Animator anim;
    static public GameObject playerGO;
    public PlayerStats playerStat;
    private Rigidbody2D rbody;

    //Character stats
    [Header("Player Attributes")]
    public float hunger;
    public float water;
    public float health;
    public Slider hungerSlider;
    public Slider waterSlider;
    public Slider healthSlider;

    public float totalFoodToIncrease;
    public float currentFoodToIncrease;
    public float start, target, timer;
    public float digestrionRate = 0.8f;


    static public float walk_speed = 1.5f;
    static public float run_speed = 2.5f;
    static public float turnspeed = 100f;
    private int ammo = 0;

    // this is per second if mult by delta time.   1 is 1 per sec, etc.
    public float deterationRate = .25f;
    public float totalFoodEaten = 0f;
    public bool doWaterDecrement = false;

    [Header("Eating, Drinking, Health Status")]
    public bool attemptToEat = false;
    public bool attemptToDrink = false;
    public bool attemptToHeal = false;
    public bool alive = false;
    public bool hungarTick = true;
    public bool waterTick = false;
    public bool healthTick = false;
    public float timeToIncreaseOver = 8f;

    //UI
    [Header("General UI")]
    public Text updHealth;
    public Text updHungar;
    public Text updWater;
    public GameObject lightSource;
    public GameObject InteractionShell;

    [Header("Inventory UI")]
    public Text updLogInventory;
    public Text updStickInventory;
    public Text updStoneInventory;
    public Text updFlintInventory;
    public Text updHerbInventory;
    public Text updWaterInventory;
    public Text updBerriesInventory;
    public Text updCampFireInventory;
    public Slider logSlider;
    public Slider stickSlider;
    public Slider stoneSlider;
    public Slider flintSlider;
    public Slider herbSlider;
    public Slider invWaterSlider;
    public Slider berrySlider;
    public Slider campFireSlider;

    [Header("HotKey UI")]
    public Text updBerryNumber;
    public Text updHerbNumber;
    public Text updWaterNumber;

    [Header("Node Handlers")]
    public GameObject currentNode;
    public ResourceNode node;
    private bool nodeIsHarvestable;

    public GameObject PBHolder;
    public Slider slider;

    //Managers
    [Header("Harvest Info")]
    public bool canHarvest = false;
    public float MaxTime;
    public float harvestTime;
      

    [Header("Inventory and Item Data")]
    public Inventory myBox;
    public Item_Data itemData;

    IEnumerator hungarSaturationTimer(float waitTimer)
    {
        yield return new WaitForSeconds(waitTimer);
        hungarTick = true;
    }


    IEnumerator _SubOverTime(float duration, float amount, System.Action<float> callback)
    {
        //updateStatus(false, true, true);
        float totalAdded = 0f;
        float step = amount / duration;

        while (Mathf.Abs(totalAdded) < Mathf.Abs(amount))
        {
            float add = step * Time.deltaTime;

            if (Mathf.Abs(add) >= Mathf.Abs(amount - totalAdded))
            {
                Debug.Log("water break");
                callback(amount - totalAdded);
                yield break;
            }

            callback(add);
            totalAdded += add;
            yield return null;
        }
    }

    IEnumerator _AddOverTime(float duration, float amount, System.Action<float> callback)
    {
        float totalAdded = 0f;
        float step = amount / duration;

        while(Mathf.Abs(totalAdded) < Mathf.Abs(amount))
        {
            float add = step * Time.deltaTime;

            if(Mathf.Abs(add) >= Mathf.Abs(amount - totalAdded))
            {
                Debug.Log("hungar break");
                callback(amount - totalAdded);
                yield break;
            }

            callback(add);
            totalAdded += add;
            yield return null;
        }
    }

    public void AddOverTime(float duraction, float amount, System.Action<float> callback)
    {
        StartCoroutine(_AddOverTime(duraction, amount, callback));
        StartCoroutine(hungarSaturationTimer(duraction + 5f));
    }

    public void SubOverTime(float duration, float amount, System.Action<float> callback)
    {
        StartCoroutine(_SubOverTime(duration, amount, callback));
    }

    public void WaterAddOverTime(float duration, float amount, System.Action<float> callback)
    {
        StartCoroutine(_AddOverTime(duration, amount, callback));
    }

    //Set up for Singleton
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        myBox = GetComponent<Inventory>();
        //lightSource.SetActive(false);

        playerGO = this.gameObject;
        slider.maxValue = MaxTime;

        initAttributes();
        initInventory();
        initStatus();

        timer = 0;
        
	}

    void Update()
    {
        //8 is what time to increase over  - promote to var later
        timer += Time.deltaTime / Mathf.Max((Mathf.Abs((target - start)) * digestrionRate), 8f);
        
        

        //if(!hungarTick)
            //hunger = Mathf.Lerp(start, target, timer);
        // larp smooth addition between values start and target over the timers length.  clamped by build

        if (hunger == target)
        {
            hungarTick = true;
            target = 0;
        }

            #region Movement and Animatino
            //Movement
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            this.transform.position += Vector3.left * walk_speed * Time.deltaTime;
            anim.Play("left");
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            this.transform.position += Vector3.right * walk_speed * Time.deltaTime;
            anim.Play("right");
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            this.transform.position += Vector3.down * walk_speed * Time.deltaTime;
            anim.Play("down");
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            this.transform.position += Vector3.up * walk_speed * Time.deltaTime;
            anim.Play("up");
        }

        // Stops animation with the character facing the respected direction
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            anim.Play("stopL");
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            anim.Play("stopR");
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            anim.Play("stopD");
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            anim.Play("stopU");
        }

        #endregion

        
         //Hunger and Water Events are handled via EventSystem.cs
                   

        //Update the UI
        updateStatus(true, true, true);


        if(Input.GetKeyDown(KeyCode.U))
        {
            //CraftManager.instance.checkRecipe(CraftManager.instance.getRecipe("Stick"));
            Debug.Log(Inventory.instance.getStick().getNumCraftReq());
            Debug.Log(Inventory.instance.getStick().getNumCraftAmt());
        }

        #region Event Area
        if (Input.GetKeyDown(KeyCode.C))
            EventSystem.instance.toggleCrafting();

        if (Input.GetKeyDown(KeyCode.I))
            EventSystem.instance.toggleInventory();

        if (attemptToEat == false)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                attemptToEat = true;
                //doEat();
                EventSystem.instance.eatEvent(10);
            }
        }

        if(attemptToHeal == false)
        {
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                attemptToHeal = true;
                //doHeal();
                EventSystem.instance.healEvent(20);
            }
        }

        if(attemptToDrink == false)
        {
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                attemptToDrink = true;
                //doDrink();
                EventSystem.instance.drinkEvent(5);
            }
        }
        #endregion


        #region Resource Harvesting
        // Harvesting
        if (canHarvest)
        {
            if(Input.GetKey(KeyCode.E))
            {
                harvestTime += Time.deltaTime;
                    slider.value = harvestTime;

            }
            if(Input.GetKeyUp(KeyCode.E))
            {
                harvestTime = 0;
                slider.value = 0;
            }

            if(slider.value >= slider.maxValue)
            {
                //node.spawnItems();
                //canHarvest = false;
                slider.value = 0;
                harvestTime = 0;

                //Check to see if inventory is full of that nodes resource
                ITEMS currentItem = node.determineItemData();

                if(ImprovedInventory.instance.getItemCount(currentItem) == ImprovedInventory.instance.getMaxStack(currentItem))
                {
                    //Inventory if full on currentItem
                    //Checks if toggle of player's debug is on, if so lets debug
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Inventory is full of " + ImprovedInventory.instance.getItem(currentItem).name + ".");
                }
                else
                {
                    node.Harvest();
                }

                #region Legacy - harvesting
                /*
                if (myBox.getItemCount(currentItem) == myBox.getMaxStack(currentItem))
                {
                    //Inventory is full
                    //Checks if Toggle of Player's Debug is on, if so lets debug
                    if (ManageDebug.instance.debugPlayer)
                        Debug.Log("Inventory is full of " + myBox.getItem(currentItem).name + ".");
                }
                else
                {
                    node.Harvest();
                }
                */
                #endregion


                manageIntventory();
                if(node.NodeHarvestable == false)
                {
                    InteractionShell.SetActive(false);
                }
            }
        }
        #endregion

    }

    public void doDrink()
    {
        float waterToIncrease = 15f;

        if(ImprovedInventory.instance.getItemCount(ITEMS.WATER) > 0)
        {
            if (DebugManager.debugPlayer)
                Debug.Log("I have: " + ImprovedInventory.instance.getItemCount(ITEMS.WATER) + " water left");

            if (water + waterToIncrease <= playerStat.getMaxWater())
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Added " + waterToIncrease + " to water");

                WaterAddOverTime(timeToIncreaseOver, waterToIncrease, (a) => water += a);
                //water += waterToIncrease;
                updateStatus(false, false, true);
                ImprovedInventory.instance.removeItem(ITEMS.WATER, 1);
                updateInventoryText(ITEMS.WATER);

                if (DebugManager.debugPlayer)
                    Debug.Log("Drank");
            }
            else if (water == playerStat.getMaxWater())
            {
                //do nothing
                if (DebugManager.debugPlayer)
                    Debug.Log("Already Full Water");
            }
        }

        #region Legacy Code - water handling
        /*
        if (myBox.getItemCount(ITEMS.WATER) > 0 )
        {
            if (DebugManager.debugPlayer)
                Debug.Log("I have: " + myBox.getItemCount(ITEMS.WATER) + " water left");

            if(water + waterToIncrease <= playerStat.getMaxWater())
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Added " + waterToIncrease + " to water");

                WaterAddOverTime(timeToIncreaseOver, waterToIncrease, (a) => water += a);
                //water += waterToIncrease;
                updateStatus(false, false, true);
                myBox.removeItem(ITEMS.WATER, 1);
                updateInventoryText(ITEMS.WATER);

                if (DebugManager.debugPlayer)
                    Debug.Log("Drank");
            }
            else if (water == playerStat.getMaxWater())
            {
                //do nothing
                if (DebugManager.debugPlayer)
                    Debug.Log("Already Full Water");
            }
        }
        */
        #endregion

        attemptToDrink = false;
    }

    public void doHeal()
    {
        float healthToIncrease = 25f;

        if(ImprovedInventory.instance.getItemCount(ITEMS.HERB) > 0)
        {
            if (DebugManager.debugPlayer)
                Debug.Log("I have: " +ImprovedInventory.instance.getItemCount(ITEMS.HERB) + " herbs left");

            if (health + healthToIncrease <= playerStat.GetMaxHealth())
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Added " + healthToIncrease + " to health");

                health += healthToIncrease;
                updateStatus(true, false, false);
                ImprovedInventory.instance.removeItem(ITEMS.HERB, 1);
                updateInventoryText(ITEMS.HERB);

                if (DebugManager.debugPlayer)
                    Debug.Log("healed");
            }
            else if (health == playerStat.GetMaxHealth())
            {
                //do nothing
                if (DebugManager.debugPlayer)
                    Debug.Log("Already Full Health");
            }
            else
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Set health to max");

                health = playerStat.GetMaxHealth();
                updateStatus(true, false, false);
                ImprovedInventory.instance.removeItem(ITEMS.HERB, 1);
                updateInventoryText(ITEMS.HERB);

                if (DebugManager.debugPlayer)
                    Debug.Log("healed");
            }

        }
        else
        {
            if (DebugManager.debugPlayer)
                Debug.Log("No herbs to heal with");

        }

        #region Legacy Code - Healing
        /*
        if (myBox.getItemCount(ITEMS.HERB) > 0)
        {
            if (DebugManager.debugPlayer)
                Debug.Log("I have: " + myBox.getItemCount(ITEMS.HERB) + " herbs left");

            if(health + healthToIncrease <= playerStat.GetMaxHealth())
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Added " + healthToIncrease + " to health");

                health += healthToIncrease;
                updateStatus(true, false, false);
                myBox.removeItem(ITEMS.HERB, 1);
                updateInventoryText(ITEMS.HERB);

                if (DebugManager.debugPlayer)
                    Debug.Log("healed");
            }
            else if(health == playerStat.GetMaxHealth())
            {
                //do nothing
                if (DebugManager.debugPlayer)
                    Debug.Log("Already Full Health");
            }
            else
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Set health to max");

                health = playerStat.GetMaxHealth();
                updateStatus(true, false, false);
                myBox.removeItem(ITEMS.HERB, 1);
                updateInventoryText(ITEMS.HERB);

                if (DebugManager.debugPlayer)
                    Debug.Log("healed");
            }
        }
        else
        {
            if (DebugManager.debugPlayer)
                Debug.Log("No herbs to heal with");

        }
        */
        #endregion

        attemptToHeal = false;
    }

    public void doEat()
    {
        float increasePerFood = 10f;
        hungarTick = false;

        if (ImprovedInventory.instance.getItemCount(ITEMS.BERRY) > 0)
        {
            timer = 0;                      //set timer to 0 so it added the new total over again
            target += increasePerFood;      //increase the current target to reach
            start = hunger;                 //set the starting amount of larp to the current hungar
                                            //increase the var duraction for slowing incrementation
                                            //timer += Time.deltaTime * this var
                                            //formula 

            if (DebugManager.debugPlayer)
                Debug.Log("I have: " + ImprovedInventory.instance.getItemCount(ITEMS.BERRY) + " berries left");

            if (hunger + target <= playerStat.GetMaxHungar())
            {
                totalFoodEaten += increasePerFood;

                if (DebugManager.debugPlayer)
                    Debug.Log("Added 10 to food");

                //hunger += foodToIncrease;
                //AddOverTime(timeToIncreaseOver, foodToIncrease, (a) => hunger += a);
                //Debug.Log("Should drink");
                //SubOverTime(timeToIncreaseOver, -10, (a) => water += a);

                Debug.Log("water value: " + water);

                //doWaterDecrement = true;
                updateStatus(false, true, true);
                ImprovedInventory.instance.removeItem(ITEMS.BERRY, 1);
                updateInventoryText(ITEMS.BERRY);
                if (DebugManager.debugPlayer)
                    Debug.Log("Yummy");
            }
            else if (hunger == playerStat.GetMaxHungar())
            {
                //do nothing
                if (DebugManager.debugPlayer)
                    Debug.Log("Already Full Hungar");
            }
            else
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Set food to max food");

                totalFoodEaten += playerStat.GetMaxHealth() - hunger;
                hunger = playerStat.GetMaxHungar();
                doWaterDecrement = true;
                updateStatus(false, true, false);
                ImprovedInventory.instance.removeItem(ITEMS.BERRY, 1);
                updateInventoryText(ITEMS.BERRY);
                if (DebugManager.debugPlayer)
                    Debug.Log("Yummy");
            }

        }
        else
        {
            if (DebugManager.debugPlayer)
                Debug.Log("No Food To Eat..");
        }

        #region Legacy Code - Eating
        /*
        if (myBox.getItemCount(ITEMS.BERRY) > 0)
        {
            timer = 0;                      //set timer to 0 so it added the new total over again
            target += increasePerFood;      //increase the current target to reach
            start = hunger;                 //set the starting amount of larp to the current hungar
                                            //increase the var duraction for slowing incrementation
                                            //timer += Time.deltaTime * this var
                                            //formula 


            if (DebugManager.debugPlayer)
                Debug.Log("I have: " + myBox.getItemCount(ITEMS.BERRY) + " berries left");

            if (hunger + target <= playerStat.GetMaxHungar())
            {
                totalFoodEaten += increasePerFood;

                if (DebugManager.debugPlayer)
                    Debug.Log("Added 10 to food");

                //hunger += foodToIncrease;
                //AddOverTime(timeToIncreaseOver, foodToIncrease, (a) => hunger += a);
                //Debug.Log("Should drink");
                //SubOverTime(timeToIncreaseOver, -10, (a) => water += a);

                Debug.Log("water value: " + water );

                //doWaterDecrement = true;
                updateStatus(false, true, true);
                myBox.removeItem(ITEMS.BERRY, 1);
                updateInventoryText(ITEMS.BERRY);
                if (DebugManager.debugPlayer)
                    Debug.Log("Yummy");
            }
            else if (hunger == playerStat.GetMaxHungar())
            {
                //do nothing
                if (DebugManager.debugPlayer)
                    Debug.Log("Already Full Hungar");
            }
            else
            {
                if (DebugManager.debugPlayer)
                    Debug.Log("Set food to max food");

                totalFoodEaten += playerStat.GetMaxHealth() - hunger;
                hunger = playerStat.GetMaxHungar();
                doWaterDecrement = true;
                updateStatus(false, true, false);
                myBox.removeItem(ITEMS.BERRY, 1);
                updateInventoryText(ITEMS.BERRY);
                if (DebugManager.debugPlayer)
                    Debug.Log("Yummy");
            }

        }
        else
        {
            if (DebugManager.debugPlayer)
                Debug.Log("No Food To Eat..");
        }
        */
        #endregion


        attemptToEat = false;
       
    }

    // Initiates player health, hungar, and water attributes to full
    public void initAttributes()
    {
        PlayerStats.instance.setMaxHealth(100);
        PlayerStats.instance.setMaxHunger(100);
        PlayerStats.instance.setMaxWater(100);

        alive = true;
        health = 100;
        hunger = 90;
        water = 100;

        start = target = hunger;
    }

    // Initiates player UI with starting attribute ammounts
    public void initStatus()
    {
        healthSlider.minValue = 0;
        healthSlider.maxValue = PlayerStats.instance.GetMaxHealth();
        updHealth.text = health.ToString();
        hungerSlider.minValue = 0;
        hungerSlider.maxValue = PlayerStats.instance.GetMaxHungar();
        updHungar.text = hunger.ToString();
        waterSlider.minValue = 0;
        waterSlider.maxValue = PlayerStats.instance.getMaxWater();
        updWater.text = water.ToString();
    }

    public void updateStatus(bool h, bool hungar, bool w)
    {
        if(h)
        {
            updHealth.text = health.ToString();
            healthSlider.value = health;
        }
            

        if(hungar)
        {
            updHungar.text = hunger.ToString();
            hungerSlider.value = hunger;
        }
            
        if(w)
        {
            updWater.text = water.ToString();
            waterSlider.value = water;
        }
            
    }

    public void initInventory()
    {
        updLogInventory.text = "0";
        updStickInventory.text = "0";

        updStoneInventory.text = "0";
        updFlintInventory.text = "0";

        updHerbInventory.text = "0";

        updWaterInventory.text = "0";
        updBerriesInventory.text = "0";

        updCampFireInventory.text = "0";

        //updBerryNumber.text = "0";
        //updHerbNumber.text = "0";
        //updWaterNumber.text = "0";

        logSlider.minValue = 0;
        logSlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.LOG);
        logSlider.value = 0;

        stickSlider.minValue = 0;
        stickSlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.STICK);
        stickSlider.value = 0;

        stoneSlider.minValue = 0;
        stoneSlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.STONE);
        stoneSlider.value = 0;

        flintSlider.minValue = 0;
        flintSlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.FLINT);
        flintSlider.value = 0;

        herbSlider.minValue = 0;
        herbSlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.HERB);
        herbSlider.value = 0;

        berrySlider.minValue = 0;
        berrySlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.BERRY);
        berrySlider.value = 0;

        invWaterSlider.minValue = 0;
        invWaterSlider.maxValue = Item_Data.instance.getMAxStack(ITEMS.WATER);
        invWaterSlider.value = 0;

        campFireSlider.minValue = 0;
        campFireSlider.maxValue = 1;
        campFireSlider.value = 0;
    }

    public void manageIntventory()
    {
        ITEMS thisItemDataType = node.determineItemData();
        ImprovedInventory.instance.addItem(thisItemDataType, 1);

        #region Legacy Code - Add to inventory
        myBox.addItem(thisItemDataType, 1);
        #endregion

        updateInventoryText(thisItemDataType);
    }

    public void updateInventoryText(ITEMS typeToUpdate)
    {
        switch(typeToUpdate)
        {
            case ITEMS.LOG:
                logSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updLogInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.STICK:
                stickSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updStickInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.STONE:
                stoneSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updStoneInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.FLINT:
                flintSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updFlintInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.HERB:
                herbSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updHerbInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                //updHerbNumber.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.WATER:
                invWaterSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updWaterInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                //updWaterNumber.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.BERRY:
                berrySlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                updBerriesInventory.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                //updBerryNumber.text = ImprovedInventory.instance.getItemCount(typeToUpdate).ToString();
                break;
            case ITEMS.CAMP_FIRE:
                campFireSlider.value = ImprovedInventory.instance.getItemCount(typeToUpdate);
                break;
            default:
                updLogInventory.text = "x";
                break;

                #region Legacy Code - Update Inventory UI
                /*
                case ITEMS.LOG:
                    logSlider.value = myBox.getItemCount(typeToUpdate);
                    updLogInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.STICK:
                    stickSlider.value = myBox.getItemCount(typeToUpdate);
                    updStickInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.STONE:
                    stoneSlider.value = myBox.getItemCount(typeToUpdate);
                    updStoneInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.FLINT:
                    flintSlider.value = myBox.getItemCount(typeToUpdate);
                    updFlintInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.HERB:
                    herbSlider.value = myBox.getItemCount(typeToUpdate);
                    updHerbInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    //updHerbNumber.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.WATER:
                    invWaterSlider.value = myBox.getItemCount(typeToUpdate);
                    updWaterInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    //updWaterNumber.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.BERRY:
                    berrySlider.value = myBox.getItemCount(typeToUpdate);
                    updBerriesInventory.text = myBox.getItemCount(typeToUpdate).ToString();
                    //updBerryNumber.text = myBox.getItemCount(typeToUpdate).ToString();
                    break;
                case ITEMS.CAMP_FIRE:
                    campFireSlider.value = myBox.getItemCount(typeToUpdate);
                    break;
                default:
                    updLogInventory.text = "x";
                    break;
                    */
                #endregion
        }
    }



    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "ResourceNode")
        {
            node = col.gameObject.GetComponent<ResourceNode>();

            if (node.NodeHarvestable)
                nodeIsHarvestable = true;
            else
                nodeIsHarvestable = false;

            if(nodeIsHarvestable)
            {
                InteractionShell.SetActive(true);
                canHarvest = true;
            }
            else
            {
                InteractionShell.SetActive(false);
                canHarvest = false;
            }
            
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "ResourceNode")
        {
            if(node.NodeHarvestable)
                InteractionShell.GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(col.gameObject.transform.position);
            else
                InteractionShell.SetActive(false);    
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "ResourceNode")
        {
            InteractionShell.SetActive(false);
            canHarvest = false;
        }
    }

  
}
