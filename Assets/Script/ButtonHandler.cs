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
        SceneManager.LoadScene("start");
    }

    // for 連鎖反応
    //URLのページに移動
    public void OnClickURL()
    {
        Application.OpenURL(DataManager.GetInstance().shopURL);
    }
}
