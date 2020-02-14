using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public class Window : MonoBehaviour
    {
        public GameObject view;

        public bool IsShowing()
        {
            return view.activeSelf;
        }
        public void Show()
        {
            view.SetActive(true);
        }
        public void Hide()
        {
            view.SetActive(false);
        }
    }
}