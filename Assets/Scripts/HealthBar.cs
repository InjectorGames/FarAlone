using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InjectorGames.FarAlone.Players;

namespace InjectorGames.FarAlone.UI
{
    public class HealthBar : MonoBehaviour
    {
        Vector3 localScale;
        public Transform player;
        private float healthProportion;//A proportion between healthbar scale and player's HP

        void Start()
        {
            localScale = transform.localScale;
            healthProportion = player.GetComponent<PlayerController>().HP / localScale.x;;
        }


        void Update()
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            localScale.x = player.GetComponent<PlayerController>().HP / healthProportion;
            transform.localScale = localScale;
        }
    }
}
