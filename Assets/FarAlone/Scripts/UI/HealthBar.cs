using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InjectorGames.FarAlone.Players;

namespace InjectorGames.FarAlone.UI
{
    public class HealthBar : MonoBehaviour
    {
        public Transform player;
        public GameObject curHB;
        
        private RectTransform HBrt;

        private float healthProportion;//A proportion between healthbar scale and player's HP

        void Start()
        {
            HBrt = curHB.GetComponent<RectTransform>();
            healthProportion = player.GetComponent<PlayerController>().HP / HBrt.sizeDelta.x;
        }


        void Update()
        {
            float newHealth = player.GetComponent<PlayerController>().HP / healthProportion;

            HBrt.sizeDelta = new Vector2(newHealth, HBrt.sizeDelta.y);
        }
    }
}
