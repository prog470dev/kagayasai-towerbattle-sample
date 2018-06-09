using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    private static DataManager _singleInstance = new DataManager();

    public int score = 0;

    public static DataManager GetInstance()
    {
        return _singleInstance;
    }

    private DataManager(){}

}
