using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Items;

namespace Reserve
{
public enum InventoryType
{
    Small,
    Medium,
    Big,
}

public class Inventory
{
    public InventoryType Type {get; set;}
    public float current_weight;
    public float max_weight;
    public int cur_elements_amount;
    public int max_elements_amount;
    public List<Item> elements;

    public Inventory()
    {
        Type = InventoryType.Small;
        cur_elements_amount = 0;
        max_elements_amount = 0;
        elements = new List<Item>(cur_elements_amount);
        current_weight = 0.0f;
        max_weight = 0.0f;
    } 

    public Inventory(InventoryType size)
    {
        cur_elements_amount = 0;
        current_weight = 0.0f;

        if(size == InventoryType.Small)
        {
            max_elements_amount = 5;
            max_weight = 10.0f;
            elements = new List<Item>(max_elements_amount);
        }
        else if(size == InventoryType.Medium)
        {
            max_elements_amount = 7;
            max_weight = 13.0f;
            elements = new List<Item>(max_elements_amount);
        }
        else if(size == InventoryType.Big)
        {
            max_elements_amount = 10;
            max_weight = 17.0f;
            elements = new List<Item>(max_elements_amount);
        }
    }

    public void Change_Inventory(InventoryType new_size)
    {
        Type = new_size;
        
        if(new_size == InventoryType.Small)
        {
            max_elements_amount = 5;
            max_weight = 10.0f;
        }
        else if(new_size == InventoryType.Medium)
        {
            max_elements_amount = 7;
            max_weight = 13.0f;
        }
        else if(new_size == InventoryType.Big)
        {
            max_elements_amount = 10;
            max_weight = 17.0f;
        }
    }

    public bool Add_Element(Item arg_item)
    {
        var new_item = arg_item;
        if(arg_item is Weapon)
        {
            new_item = (Weapon)arg_item;
        }
        else if(arg_item is Detail)
        {
            new_item = (Detail)arg_item;
        }
        if((this.cur_elements_amount +1) > this.max_elements_amount || (this.current_weight + new_item.weight) > this.max_weight)
        {
            return false;
        }
        else{
            elements.Add(new_item);
            return true;
        }
    }

    public void Delete_Element(int position)
    {
        elements.RemoveAt(position);
    }

    public bool CanStore(float arg_weight)
    {
        if(this.cur_elements_amount == this.max_elements_amount || this.current_weight + arg_weight > this.max_weight)
        {
            return false;
        }
        return true;
    }
}
}
