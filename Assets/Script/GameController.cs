using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour {

    public GameObject fallObject;   //落とすオブジェクトの設定
    public Transform camera;

    private List<GameObject> gameObjects = new List<GameObject>();  //ステージ上に積まれているオブジェクトのリスト
    private GameObject standbyObject;   //現在の操作対象オブジェクト

    private bool canFall = true;

    // for 連鎖反応
    Dictionary<GameObject, List<GameObject>> hashMap = new Dictionary<GameObject, List<GameObject>>();
    public GameObject impala;
    bool isClear = false;
    public UnityEngine.UI.Text vegLableText;
    public UnityEngine.UI.Text shopTitleText;
    public UnityEngine.UI.Text shopDescText;
    public UnityEngine.UI.Button URLButton;
    public UnityEngine.UI.Button returnButton;
    private string[] vegNames = { "源助大根" };
    private string[] shopTitles = {"加賀野菜料理"};
    private string[] shopDescriptions = {"これは説明です。"};
    private string[] shopURLs = {"http://www.kanazawa-kagayasai.com/"};

	void Start () {
        //最初の操作オブジェクトを設定
        Vector3 standbyPosition = new Vector3(camera.position.x, camera.position.y+4, 0);
        standbyObject = Instantiate(fallObject, standbyPosition, new Quaternion());
        standbyObject.GetComponent<Rigidbody2D>().gravityScale = 0;

        // for 連鎖反応
        vegLableText.text = vegNames[0];
        shopTitleText.gameObject.SetActive(false);
        shopDescText.gameObject.SetActive(false);
        URLButton.gameObject.SetActive(false);
        returnButton.gameObject.SetActive(false);
	}
	
	void Update () {
        // for 連鎖反応
        // 野菜の説明を追跡させる
        if(!isClear){
            Vector3 pos = new Vector3(standbyObject.GetComponent<Transform>().position.x + (float)2.0,
                          standbyObject.GetComponent<Transform>().position.y - (float)3.0,
                          standbyObject.GetComponent<Transform>().position.z);
            vegLableText.gameObject.GetComponent<RectTransform>().position =
                            RectTransformUtility.WorldToScreenPoint(Camera.main, pos);
        }
                        
        //オブジェクトを落とす
        if (Input.GetMouseButtonUp(0) && canFall)
        {
            Drop(Input.mousePosition);
            canFall = false;
        }

        //オブジェクトを平行移動
        if (Input.GetMouseButton(0) && canFall)
        {
            Move(Input.mousePosition);
        }

        //オブジェクトを回転
        if (Input.GetKeyDown(KeyCode.Space) && canFall)
        {            
            standbyObject.GetComponent<Transform>().eulerAngles = new Vector3(0,0,(standbyObject.GetComponent<Transform>().eulerAngles.z + 30)%360); 
        }

        //ステージ上のオブジェクトの静止をチェック
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

            //全オブジェクトの静止が確認できたときの処理
            if (cnt == gameObjects.Count && !isClear)
            {
                if (CheckAttach())
                {
                    // for 連鎖反応
                    Debug.Log("連結成分発見");
                    isClear = true; //ここでゲーム終了

                    DataManager.GetInstance().shopURL = shopURLs[0];
                    shopTitleText.text = shopTitles[0];  //todo
                    shopDescText.text = shopDescriptions[0]; //todo
                    shopTitleText.gameObject.SetActive(true);
                    shopDescText.gameObject.SetActive(true);
                    URLButton.gameObject.SetActive(true);
                    returnButton.gameObject.SetActive(true);
                    vegLableText.gameObject.SetActive(false);
                }else{
                    DataManager.GetInstance().score = cnt;  //スコアを登録
                    canFall = true;
                    camera.GetComponent<Transform>().position = new Vector3(camera.GetComponent<Transform>().position.x,
                                                                            Math.Max(1, maxHeight),
                                                                            camera.GetComponent<Transform>().position.z);
                    Vector3 standbyPosition = new Vector3(camera.position.x, maxHeight + 4, 0);
                    standbyObject = Instantiate(fallObject, standbyPosition, new Quaternion());
                    standbyObject.GetComponent<Rigidbody2D>().gravityScale = 0;

                    // for 連鎖反応
                    vegLableText.text = vegNames[0];
                }

                //DataManager.GetInstance().score = cnt;  //スコアを登録
                //canFall = true;
                //camera.GetComponent<Transform>().position = new Vector3(camera.GetComponent<Transform>().position.x,
                //                                                        Math.Max(1, maxHeight),
                //                                                        camera.GetComponent<Transform>().position.z);
                //Vector3 standbyPosition = new Vector3(camera.position.x, maxHeight + 4, 0);
                //standbyObject = Instantiate(fallObject, standbyPosition, new Quaternion());
                //standbyObject.GetComponent<Rigidbody2D>().gravityScale = 0;
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


    // for 連鎖反応
    bool CheckAttach(){
        foreach (GameObject e in hashMap.Keys)
        {
            Vector3 position = e.GetComponent<Transform>().position;

            List<GameObject> list = LinkingFind(e, new List<GameObject>());
            if(list.Count >= 3){
                foreach(GameObject go in list){
                    Destroy(go);
                    gameObjects.Remove(go);
                    hashMap.Remove(go);
                }

                position.y += 2;    // 地面に埋まらないように調整（横にも調整が必要）

                Instantiate(impala, position, new Quaternion());

                return true;
            }
        }

        return false;
    }

    List<GameObject> LinkingFind(GameObject node, List<GameObject> visitedObjects){

        if(visitedObjects.Contains(node))
        {
            return visitedObjects; 
        }
        visitedObjects.Add(node);

        List<GameObject> retList = new List<GameObject>(visitedObjects); 

        foreach(GameObject e in hashMap[node])
        {
            List<GameObject> tmpList = LinkingFind(e, visitedObjects);
            if(retList.Count < tmpList.Count){
                retList = new List<GameObject>(tmpList); 
            }
        }

        return retList;
    }

    // 連結オブジェクト情報の登録   
    public void RegistAttachedObject(GameObject id1, GameObject id2){   //id1: 当て"られ"ている方, id2: 当たっている方
        if(!hashMap.ContainsKey(id1)){
            hashMap[id1] = new List<GameObject>();
        }
        if (!hashMap[id1].Contains(id2))
        {
            hashMap[id1].Add(id2);
        }
    }

    // 連結オブジェクト情報の削除
    public void RemoveAttachedObject(GameObject id1, GameObject id2)
    {   
        if (hashMap.ContainsKey(id1))
        {
            if(hashMap[id1].Contains(id2)){
                hashMap[id1].Remove(id2);
            }
        }
    }
}
