using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InjectorGames.FarAlone.Items
{
    [Serializable]
    public class ItemInfo
    {
        [HideInInspector]
        public int id;

        public string name;
        public int weight;
        public Sprite sprite;
    }
}
