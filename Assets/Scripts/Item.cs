using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
public enum ItemType
{
    Item,
    Weapon,
    Detail,
}

public abstract class Item : MonoBehaviour
{
    public string name;
    public float weight; // the weight of the item in the inventory
    public abstract ItemType Type { get; }

}

}

