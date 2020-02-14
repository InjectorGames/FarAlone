using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InjectorGames.FarAlone.UI
{
    public class InfoWindow : MonoBehaviour
    {
        public static InfoWindow Instance;

        private float fadeTime = 3.0f;
        private Text notification;

        private void Awake()
        {
            Instance = this;
            notification = GetComponent<Text>();
            //notification.color.a = 0f;
        }
        public void ShowMessage(string text)
        {
            Debug.Log("Started");
            notification.text = text;
            StartCoroutine(animate(notification, fadeTime));
            Debug.Log("Finished");
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