using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{

    public Button exitButton;    

    // Start is called before the first frame update
    void Start()
    {
        Button btn = exitButton.GetComponent<Button>();
        btn.onClick.AddListener(QuitGame);
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}
