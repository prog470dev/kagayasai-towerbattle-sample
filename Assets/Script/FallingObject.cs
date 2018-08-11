using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour {

    protected int kagaId = -1;

    // for 連鎖反応
    //連結情報の登録
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "FallObject")
        {
            return;   
        }

        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (this.kagaId == collision.gameObject.GetComponent<FallingObject>().kagaId)
        {
            GameObject id1 = this.gameObject;
            GameObject id2 = collision.gameObject;
            gameController.RegistAttachedObject(id1, id2);
        }
    }

    // for 連鎖反応
	//連結情報の削除
	private void OnCollisionExit2D(Collision2D collision)
	{
        if (collision.gameObject.tag != "FallObject")
        {
            return;
        }


        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

        if (this.kagaId == collision.gameObject.GetComponent<FallingObject>().kagaId)
        {
            GameObject id1 = this.gameObject;
            GameObject id2 = collision.gameObject;
            gameController.RemoveAttachedObject(id1, id2);
        }
	}
}
