using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public sealed class InfoWindow : Window
    {
        #region Singletone
        public static InfoWindow Instance;

        private void SetInstance()
        {
            if (Instance != null)
                throw new Exception("Multiple instances of this class is not supported");
            Instance = this;
        }
        #endregion

        public Text text;
        public float fadeTime = 3.0f;

        private void Awake()
        {
            SetInstance();
        }
        public void ShowMessage(string message)
        {
            text.text = message;
            StartCoroutine(animate(text, fadeTime));
        }

        private IEnumerator animate(Text text, float time)
        {
            yield return fadeIn(text, time / 2);
            yield return new WaitForSeconds(time);
            yield return fadeOut(text, time / 2);
        }

        private IEnumerator fadeIn(Text text, float time) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            while (text.color.a < 1.0f) {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + Time.deltaTime / time);
                yield return null;
            }
        }

        private IEnumerator fadeOut(Text text, float time) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
            while (text.color.a > 0.0f) {
                text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime / time);
                yield return null;
            }
        }
    }
}