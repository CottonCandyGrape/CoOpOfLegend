using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    //public Image GameBar_Img = null;
    //public Image EventBar_Img = null;
    public Text txt = null;

    public QTEvent[] QTEvents = null;

    int failCount = 0;
    float gameTime = 0.0f;

    public static GameMgr Inst = null;

    void Awake() { Inst = this; }


    void Start()
    {
        
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        txt.text = gameTime.ToString("F1");
    }
}