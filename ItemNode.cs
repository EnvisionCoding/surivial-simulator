using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemNode : MonoBehaviour {

    public ResourceNode host;

    [SerializeField]
    private ITEMS TYPES;

    [SerializeField]
    private int ID;

    [SerializeField]
    private string name;

    // Use this for initialization
    void Start () {
        setValues();
	}
	
    void setValues()
    {
        TYPES = host.getType();
        ID = host.dataController.getIdData(TYPES);
        name = host.dataController.getNameData(TYPES);
    }
}
