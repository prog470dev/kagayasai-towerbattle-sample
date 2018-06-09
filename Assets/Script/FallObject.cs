using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObject : MonoBehaviour {

    private int id;
	// Use this for initialization
	void Start () {
        this.id = Random.Range(0,1000);
	}
	
	// Update is called once per frame
	void Update () {
        //if(GetComponent<Rigidbody2D>().IsSleeping()){
        //    Debug.Log(this.id);
        //}
	}

	private void FixedUpdate()
	{
		
	}
}
