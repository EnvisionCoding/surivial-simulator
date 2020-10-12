using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {
        
    private float health;
    private float hungar;
    private float water;

    private float maxHealth;
    private float maxHungar;
    private float maxWater;

    public static PlayerStats instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        maxHungar = 100;
        maxHealth = 100;
        maxWater = 100;
	}

    public void setMaxHealth(float amount)
    {
        maxHealth = amount;
    }

    public void setMaxHunger(float amount)
    {
        maxHungar = amount;
    }

    public void setMaxWater(float amount)
    {
        maxWater = amount;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetMaxHungar()
    {
        return maxHungar;
    }

    public float getMaxWater()
    {
        return maxWater;
    }
}
