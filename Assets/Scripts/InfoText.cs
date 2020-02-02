using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class InfoText : MonoBehaviour
{
    public static InfoText instance;
    private int fadeTime = 5;
Text notification;
    
    void Start()
    {
        instance = this;
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

    private IEnumerator animate(Text text, int time) {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        yield return new WaitForSeconds(time);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
    }
}
