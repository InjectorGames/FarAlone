using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class Weapon : Item
    {
        public float damage;

        public override ItemType Type => ItemType.Weapon;

        public Weapon()
        {
            name = "NAME";
            weight = 0.0f;
            damage = 0.0f;
        }

        public Weapon(string arg_name, float arg_weight, float arg_damage)
        {
            name = arg_name;
            weight = arg_weight;
            damage = arg_damage;
        }
    }
}
