using System;
using UnityEngine;

namespace InjectorGames.FarAlone.Items
{
    [Serializable]
    public class Item
    {
        protected int id;
        public int ID => id;

        [SerializeField]
        protected string name;
        public string Name => name;

        [SerializeField]
        protected int weight;
        public int Weight => weight;

        [SerializeField]
        protected Sprite sprite;
        public Sprite Sprite => sprite;

        public void SetID(int id)
        {
            this.id = id;
        }
    }
}
