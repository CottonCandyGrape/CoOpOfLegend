using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EventType
{
    Call,
    KakaoTalk,
    Instagram,
    X,
    Special,
    Final
}

public enum ActionType { P20, P30, H3s, H4s, Length }

public class QTEvent : MonoBehaviour
{
    public Image EventBar_Img = null;
    public Image HoldBar_Img = null;
    public Text Type_Txt = null;
    public Text Clear_Txt = null;

    public EventType EType;
    //public EventType EType { set { eType = value; } }

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
        EventBar_Img.fillAmount = eventTimer / eventTime;
        HoldBar_Img.fillAmount = keyHoldTimer / keyHoldTime;
        //Debug.Log(keyHoldTimer);
        //Debug.Log(keyDownCount);

        if (eventTime <= eventTimer)
        {
            if (gameObject.activeSelf)
            {
                if (!isClear) //event 시간 끝났을때 clear 하지 못했으면
                    GameMgr.Inst.FailCount += 1;

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
                GameMgr.Inst.txt.text = "Clear!!!";
                gameObject.SetActive(false);
            }
        }
    }

    void InitQTEvnet()
    {
        switch (EType)
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
        Type_Txt.text = actionType.ToString() + "\n" + EType.ToString();

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

        if (EType == EventType.X)
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

        if (EType == EventType.X)
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

        if (keyHoldTimer <= 0.0f) keyHoldTimer = 0.0f; //최소값

        if (keyHoldTime <= keyHoldTimer)
            isClear = true;
    }

    void OnDisable()
    {
        eventTimer = 0.0f;
        keyHoldTimer = 0.0f;
        keyDownCount = 0;
        pause = false;
        isClear = false;
    }

    void SetTermTimer()
    {
        if (GameMgr.Inst.Step == CurrentStep.Step1)
        {
            GameMgr.Inst.TermTime = 3.0f;
        }
        else if (GameMgr.Inst.Step == CurrentStep.Step2)
        { }
        else if (GameMgr.Inst.Step == CurrentStep.Step3)
        { }
        else if (GameMgr.Inst.Step == CurrentStep.Step4)
        { }
    }

    void SetClearText()
    {
        if (isClear)
            GameMgr.Inst.txt.text = "Clear!!!";
        else
            GameMgr.Inst.txt.text = "Fail!!!";
    }
}