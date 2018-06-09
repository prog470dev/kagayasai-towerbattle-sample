using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour {

    //スタート画面に遷移
    public void OnClickToGame()
    {
        SceneManager.LoadScene("game");
    }

    //スタート画面に遷移
    public void OnClickToStart()
    {
        Debug.Log("clicked..");
        SceneManager.LoadScene("start");
    }
}
