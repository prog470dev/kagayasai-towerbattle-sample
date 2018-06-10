using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingCatcher : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("FallObject"))
        {
            SceneManager.LoadScene("result");
        }	
	}

}
