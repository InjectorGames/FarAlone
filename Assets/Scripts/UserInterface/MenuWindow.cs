using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InjectorGames.FarAlone.UI
{
    public class MenuWindow : MonoBehaviour
    {
        public GameObject game;
        public GameObject menu;

        private void Start()
        {
            if (game == null)
                throw new NullReferenceException();
            if (menu == null)
                throw new NullReferenceException();
        }

        public void OnStart()
        {
            menu.SetActive(false);
            game.SetActive(true);
        }
        public void OnOptions()
        {

        }
        public void OnExit()
        {
            Application.Quit(0);
        }
    }
}
