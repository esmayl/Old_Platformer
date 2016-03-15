using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct ItemInfo {

    public string itemName;
    public int gainAmount;
    public ItemType itemType;
    public bool pickedUp;
    public bool startAnimation;
    public GameObject mesh;
    public Material meshMaterial;
    public Vector3 startPos;
}
