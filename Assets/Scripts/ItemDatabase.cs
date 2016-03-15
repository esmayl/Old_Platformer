using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ItemType
{
    hp,mp,score
}


public class ItemDatabase : MonoBehaviour
{
    public GameObject[] items;

    public static Dictionary<string, GameObject> lootTable = new Dictionary<string, GameObject>();

	void Start ()
    {

        ItemDatabase.AddToList("MeleeEnemy", items[0]);
        ItemDatabase.AddToList("Turret", items[1]);
        ItemDatabase.AddToList("RangedEnemy", items[2]);
	}

    public static void AddToList(string enemyName, GameObject item)
    {
        if (lootTable.ContainsKey(enemyName))
        {
            lootTable[enemyName] = item;
            return;
        }
        lootTable.Add(enemyName, item);
    }
    public static void DropItem(Vector3 deathPosition, string name)
    {
        GameObject loot = Instantiate(lootTable[name]);
        loot.transform.position = deathPosition;
        loot.name = lootTable[name].name;

        loot.AddComponent<BoxCollider>().isTrigger = true;
        
    }
}
