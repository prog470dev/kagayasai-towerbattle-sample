using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingCatcher : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("FallObject"))
        {
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;
            //SceneManager.LoadScene(sceneIndex);
            SceneManager.LoadScene("result");
        }	
	}

}
