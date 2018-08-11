using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour {

    public UnityEngine.UI.Text scoreLabel;

	void Start () {
        scoreLabel.text = "SCORE：" + DataManager.GetInstance().score;
    }
	
	void Update () {
		
	}
}
