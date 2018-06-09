using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour {

    public UnityEngine.UI.Text scoreLabel;

	// Use this for initialization
	void Start () {
        scoreLabel.text = "SCORE：" + DataManager.GetInstance().score;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
