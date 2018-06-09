using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

    public GameObject fallObject;
    private List<GameObject> gameObjects = new List<GameObject>();

    public Transform camera;

    private bool canFall = true;

    private GameObject standbyObject;

	// Use this for initialization
	void Start () {
        Vector3 standbyPosition = new Vector3(camera.position.x, camera.position.y+4, 0);
        standbyObject = Instantiate(fallObject, standbyPosition, new Quaternion());
        standbyObject.GetComponent<Rigidbody2D>().gravityScale = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0) && canFall)
        {

            Drop(Input.mousePosition);
            canFall = false;
        }

        if (Input.GetMouseButton(0) && canFall)
        {
            Move(Input.mousePosition);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canFall)
        {
            Debug.Log(standbyObject.GetComponent<Transform>().eulerAngles);
            standbyObject.GetComponent<Transform>().eulerAngles = new Vector3(0,0,(standbyObject.GetComponent<Transform>().eulerAngles.z + 30)%360); 
        }

        if(!canFall){
            int cnt = 0;
            float maxHeight = 0;
            foreach (GameObject e in gameObjects)
            {
                if (e.GetComponent<Rigidbody2D>().IsSleeping())
                {
                    cnt++;
                }
                maxHeight = Math.Max(maxHeight, e.GetComponent<Transform>().position.y);
            }
            if (cnt == gameObjects.Count)
            {
                DataManager.GetInstance().score = cnt;
                Debug.Log("all Objects stoped!");
                canFall = true;
                camera.GetComponent<Transform>().position = new Vector3(camera.GetComponent<Transform>().position.x,
                                                                        Math.Max(1, maxHeight),
                                                                        camera.GetComponent<Transform>().position.z);
                Vector3 standbyPosition = new Vector3(camera.position.x, maxHeight + 4, 0);
                standbyObject = Instantiate(fallObject, standbyPosition, new Quaternion());
                standbyObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                
            }

        }

	}

    void Drop(Vector3 touchPosition)
    {
        Vector3 dropPosition = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x, //ワールド座標へ変換(カメラはorthographic)
                                           standbyObject.GetComponent<Transform>().position.y,
                                           standbyObject.GetComponent<Transform>().position.z);
        
        standbyObject.GetComponent<Transform>().position = dropPosition;
        standbyObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        gameObjects.Add(standbyObject);
    }

    void Move(Vector3 touchPosition)
    {
        Vector3 dropPosition = new Vector3(Camera.main.ScreenToWorldPoint(touchPosition).x, //ワールド座標へ変換(カメラはorthographic)
                                           standbyObject.GetComponent<Transform>().position.y,
                                           standbyObject.GetComponent<Transform>().position.z);

        standbyObject.GetComponent<Transform>().position = dropPosition;
    }

}
