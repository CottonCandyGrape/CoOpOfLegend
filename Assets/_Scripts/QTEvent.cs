using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EventType
{
    KakaoTalk,
    Call,
    Instagram
}

public class QTEvent : MonoBehaviour
{
    public Image Bar_Img = null;

    public EventType eType;
    public EventType EType { set { eType = value; } }

    string eName;
    public string EventName
    {
        get { return eName; }
        set { eName = value; }
    }

    float eventTime = 0.0f;
    float eventTimer = 0.0f;

    bool isClear = false;

    void Start()
    {
        SetEventTime();
    }

    void Update()
    {
        if (eventTime <= eventTimer) return;

        eventTimer += Time.deltaTime;
        Bar_Img.fillAmount = eventTimer / eventTime;
        GameMgr.Inst.txt.text = eventTimer.ToString();
    }

    void SetEventTime()
    {
        switch (eType)
        {
            case EventType.KakaoTalk:
                eventTime = 5.0f;
                break;

            case EventType.Call:
                eventTime = 3.0f;
                break;

            case EventType.Instagram:
                eventTime = 10.0f;
                break;
        }
    }
}