using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour
{
    List<QTEvent> qtEventList = new List<QTEvent>();

    public Text txt = null;

    public static GameMgr Inst = null;

    void Awake() { Inst = this; }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            QTEvent qtEvent = new QTEvent();
            qtEvent.EventName = "test Event";
            txt.text = qtEvent.EventName;
            qtEventList.Add(qtEvent);
        }
    }
}