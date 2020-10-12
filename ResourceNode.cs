using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResourceNode : MonoBehaviour {

    [Header("Debug?")]
    public ManageDebug DebugManager;

    [Header("Item Data")]
    public Item_Data dataController;
    public ITEMS TYPES;

    [Header("Node Info")]
    [SerializeField]
    private int ID;
    [SerializeField]
    private new string name;
    public bool RichNode;

    [Header("Respawn Info")]
    [SerializeField]
    private int resource_left;
    public bool NodeHarvestable = false;
    public bool StartRespawnTimer = false;
    public float respawnTimer;
    [SerializeField]
    private bool respawn;
    public float resetTime = 5f;

    [Header("Resource Info")]
    private int min_ATM;    // min gather amount per node
    private int max_ATM;    // max gather amount per node
    [SerializeField]
    private Item resource;
       
    public ResourceNode(int id, string n, int time, bool r, int min, int max, Item item)
    {
        ID = id;
        name = n;
        respawnTimer = time;
        respawn = r;
        min_ATM = min;
        max_ATM = max;
        resource = item;
    }


	// Use this for initialization
	void Start ()
    {
        ID = dataController.getIdData(TYPES);
        name = dataController.getNameData(TYPES);
        //resource = dataController.
        hideChildren();

        RespawnNode();
    }
	
	// Update is called once per frame
	void Update () {
		if(respawn)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                RichNode = false;
                RespawnNode();
            }
                
        }
	}

        void RespawnNode()
    {
        //Determines if the node is a Rich Node, which is high in resources
        int richChance = Random.Range(1, 11);
        if (richChance > 8)             // 20% chance of being a rich node
            RichNode = true;

        ResourceFloor();
        ResourceCeiling();
        resource_left = max_ATM;
        NodeHarvestable = true;
        respawn = false;
        respawnTimer = resetTime;

        if(DebugManager.debugResourceNodes)
            Debug.Log("Respawned");
    }

    public void Harvest()
    {
        if(NodeHarvestable)
        {
            resource_left--;

            if (DebugManager.debugResourceNodes)
                Debug.Log("Harvested <1>   <" + name + ">");
        }

        if(resource_left == 0)
        {
            NodeHarvestable = false;

            if (DebugManager.debugResourceNodes)
                Debug.Log("Out of Resources");

            respawn = true;

        }
    }

    void ResourceFloor()
    {
        if (RichNode)
            min_ATM = Random.Range(3, 6);
        else
            min_ATM = Random.Range(1, 4);
    }

    void ResourceCeiling()
    {
        if (RichNode)
            max_ATM = Random.Range(5, 9);
        else
            max_ATM = Random.Range(3, 6);
    }

    void hideChildren()
    {
        for(int i = 0; i < this.transform.childCount; i ++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void spawnItems()
    {
        int spawnAMT = Random.Range(1, 11);

        if (DebugManager.debugResourceNodes)
            Debug.Log(spawnAMT);

        for(int i = 0; i < spawnAMT; i ++)
        {
            this.transform.GetChild(i).gameObject.SetActive(true);
        }

    }

    public int getID()
    {
        return ID;
    }

    public string getName()
    {
        return name;
    }

    public ITEMS getType()
    {
        return TYPES;
    }

    public Item getItemType()
    {
        return resource;
    }

    public ITEMS determineItemData()
    {
        if (ID == 0)
            return ITEMS.STONE;
        else if (ID == 1)
            return ITEMS.FLINT;
        else if (ID == 2)
            return ITEMS.LOG;
        else if (ID == 3)
            return ITEMS.STICK;
        else if (ID == 4)
            return ITEMS.HERB;
        else if (ID == 5)
            return ITEMS.WATER;
        else if (ID == 6)
            return ITEMS.BERRY;
        else if (ID == 7)
            return ITEMS.RAW_MEAT;
        else if (ID == 8)
            return ITEMS.COOKED_MEAT;

        return ITEMS.Default;
    }
}
