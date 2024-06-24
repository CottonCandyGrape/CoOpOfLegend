using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EventType
{
    KakaoTalk,
    Call,
    Instagram,
    X,
    Special,
    Final
}

public enum ActionType { P20, P30, H3s, H4s }

public class QTEvent : MonoBehaviour
{
    public Image eventBar_Img = null;
    public Image holdBar_Img = null;
    public Text clear_Txt = null;

    EventType eType;
    public EventType EType { set { eType = value; } }

    ActionType aType;
    //public ActionType AType { set { aType = value; } }

    float eventTime = 0.0f;
    float eventTimer = 0.0f;

    //연타 
    int keyDownCount = 0;
    int targetCount = 0;
    //연타 

    //홀드
    float keyHoldTime = 0.0f;
    float keyHoldTimer = 0.0f;
    //홀드

    KeyCode[] keys;

    bool pause = false;
    bool isClear = false;

    void Start()
    {
        InitQTEvnet();
    }

    void Update()
    {
        if (pause) return; //일시정지. TODO : 고쳐야 할수도

        eventTimer += Time.deltaTime;
        eventBar_Img.fillAmount = eventTimer / eventTime;
        holdBar_Img.fillAmount = keyHoldTimer / keyHoldTime;

        if (eventTime <= eventTimer)
        {
            if (gameObject.activeSelf)
            {
                SetClearText(); //TODO : 없애기
                gameObject.SetActive(false);
            }

            return;
        }

        if (aType == ActionType.P20 || aType == ActionType.P30)
            KeyDown(keys);
        else if (aType == ActionType.H3s || aType == ActionType.H4s)
            KeyHold(keys);

        if (isClear)
        {
            if (gameObject.activeSelf)
            {
                clear_Txt.text = "Clear!!!";
                gameObject.SetActive(false);
            }
        }
    }

    void InitQTEvnet()
    {
        switch (eType)
        {
            case EventType.Call:
                keys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };
                break;

            case EventType.KakaoTalk:
                keys = new KeyCode[] { KeyCode.K, KeyCode.O };
                break;

            case EventType.Instagram:
                keys = new KeyCode[] { KeyCode.T, KeyCode.Space };
                break;

            case EventType.X:
                //TODO : 마우스 넣기
                break;

            case EventType.Special:
                keys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R };
                targetCount = 60;
                break;

            case EventType.Final:
                keys = new KeyCode[] { KeyCode.Q, KeyCode.W, KeyCode.E, KeyCode.R, KeyCode.Return };
                targetCount = 100;
                break;
        }
    }

    public void SetActionType(ActionType actionType)
    {
        aType = actionType;

        switch (aType)
        {
            case ActionType.P20:
                eventTime = 3.0f;
                targetCount = 20;
                break;

            case ActionType.P30:
                eventTime = 4.0f;
                targetCount = 30;
                break;

            case ActionType.H3s:
                eventTime = 3.0f;
                keyHoldTime = 2.0f;
                break;

            case ActionType.H4s:
                eventTime = 4.0f;
                keyHoldTime = 3.0f;
                break;
        }
    }

    void KeyDown(KeyCode[] keyArr)
    {
        if (aType == ActionType.H3s || aType == ActionType.H4s)
            return;

        if (eType == EventType.X)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                keyDownCount++;
        }
        else
        {
            for (int i = 0; i < keyArr.Length; i++)
            {
                if (Input.GetKeyDown(keyArr[i]))
                    keyDownCount++;
            }
        }

        if (targetCount <= keyDownCount)
            isClear = true;
    }

    void KeyHold(KeyCode[] keyArr)
    {
        if (aType == ActionType.P20 || aType == ActionType.P30)
            return;

        if (eType == EventType.X)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
                keyHoldTimer += Time.deltaTime;
            else
                keyHoldTimer -= Time.deltaTime;
        }
        else
        {
            for (int i = 0; i < keyArr.Length; i++)
            {
                if (Input.GetKey(keyArr[i]))
                    keyHoldTimer += Time.deltaTime;
                else
                    keyHoldTimer -= Time.deltaTime;
            }
        }

        if (keyHoldTime <= keyHoldTimer)
            isClear = true;
    }

    void OnDisable()
    {
        eventTimer = 0.0f;
        keyHoldTimer = 0.0f;
        keyDownCount = 0;
        pause = false;
    }

    void SetClearText()
    {
        if (isClear)
            clear_Txt.text = "Clear!!!";
        else
            clear_Txt.text = "Fail!!!";
    }
}