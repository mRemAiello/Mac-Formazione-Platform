using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Chiave, Seme per sbloccare la pianta, maschera antigas
// Bonus a vita, armatura e attacco
// Incantesimi dei 5 elementi
// Kunai, stelle ninja, bombe fumogene

public class PlayerInventory : Inventory
{
    public static PlayerInventory Instance { get; protected set; }
    public static bool InstanceExists => Instance != null;

    protected void Awake()
    {
        if (InstanceExists)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // TODO: Caricare l'inventario dal save
    }

    protected void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    // TODO: Implementare
    public void Save()
    {

    }

    void OnApplicationQuit()
    {
        Save();
    }
}