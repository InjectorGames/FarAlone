using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InjectorGames.FarAlone.Players;

namespace InjectorGames.FarAlone.UI
{
public class StaminaBar : MonoBehaviour
{
    public Transform player;
    public GameObject curSB;

    private RectTransform SBrt;

    private float staminaProportion;

    void Start()
    {
        SBrt = curSB.GetComponent<RectTransform>();

        staminaProportion = 100 / SBrt.sizeDelta.x;
    }

    
    void Update()
    {

        float newStamina = player.GetComponent<PlayerController>().stamina / staminaProportion;
        SBrt.sizeDelta = new Vector2(newStamina, SBrt.sizeDelta.y);
    }
}
}
