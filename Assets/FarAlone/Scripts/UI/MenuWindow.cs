using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace InjectorGames.FarAlone.UI
{
    public sealed class MenuWindow : Window
    {
        #region Singletone
        public static MenuWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        public GameObject game;
        public GameObject menu;

        private void Awake()
        {
            SetInstance();
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
