using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageDebug : MonoBehaviour {

    public static ManageDebug instance = null;

    [Header("General Debug Info")]
    public bool debugPlayer = false;
    public bool debugPlayerStats = false;
    public bool debugInventory = false;
    public bool debugResourceNodes = false;

    [Header ("Player Debug Info")]
    public bool debug_InventoryAmounts = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }
}
