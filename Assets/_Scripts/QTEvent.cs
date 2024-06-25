using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public EventType EType;
    //public EventType EType { set { eType = value; } }

    ActionType aType;
    //public ActionType AType { set { aType = value; } }

    float eventTime = 0.0f;
    float eventTimer = 0.0f;

    float delayTimer = 0.0f;
    float delayTime;
    public float DelayTime
    {
        get { return delayTime; }
        set { delayTime = value; }
    }

    int termCount;
    public int TermCount { set { termCount = value; } }

    //연타 
    int keyDownCount = 0;
    int targetCount = 0;
    //연타 

    //홀드
    float keyHoldTime = 0.0f;
    float keyHoldTimer = 0.0f;
    //홀드

    KeyCode[] keys;

    //bool pause = false;
    bool isClear = false;

    void OnEnable()
    {
        SoundMgr.Inst.PlaySfxSound(SfxType.QTEStart);
    }

    void Start() { InitQTEvnet(); }

    void Update()
    {
        //if (pause) return; //일시정지. TODO : 고쳐야 할수도

        eventTimer += Time.deltaTime;
        EventBar_Img.fillAmount = eventTimer / eventTime;

        //UI 게이지바 
        if (aType == ActionType.P20 || aType == ActionType.P30)
        {
            if (targetCount != 0)
                HoldBar_Img.fillAmount = (float)keyDownCount / targetCount;
        }
        else if (aType == ActionType.H3s || aType == ActionType.H4s)
            HoldBar_Img.fillAmount = keyHoldTimer / keyHoldTime;
        //UI 게이지바 

        if (eventTime <= eventTimer)
        {
            if (gameObject.activeSelf)
            {
                if (!isClear) //event 시간 끝났을때 clear 하지 못했으면
                {
                    GameMgr.Inst.FailCount += 1;
                    SoundMgr.Inst.PlaySfxSound(SfxType.QTEFail);
                }

                enabled = false;
            }

            return;
        }

        //입력 받는 부분
        if (aType == ActionType.P20 || aType == ActionType.P30)
            KeyDown(keys);
        else if (aType == ActionType.H3s || aType == ActionType.H4s)
            KeyHold(keys);
        //입력 받는 부분

        QTDelayTimer();

        //클리어시
        if (isClear)
        {
            if (gameObject.activeSelf)
            {
                SoundMgr.Inst.PlaySfxSound(SfxType.QTESuccess);
                enabled = false;
            }
        }
        //클리어시
    }

    void QTDelayTimer()
    {
        if (delayTime < 0) return;

        delayTimer += Time.deltaTime;

        if (delayTime <= delayTimer)
        {
            delayTimer = 0.0f;
            delayTime = -1.0f;
            GameMgr.Inst.SpawnEvent();
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
                break;
        }
    }

    public void SetEventInfo(ActionType actionType)
    {
        aType = actionType;
        Type_Txt.text = actionType.ToString().Substring(0, 1);

        switch (aType)
        {
            case ActionType.P20:
                eventTime = 3.0f;
                //eventTime = 0.1f;
                targetCount = 15;
                break;

            case ActionType.P30:
                eventTime = 4.0f;
                //eventTime = 0.1f;
                targetCount = 20;
                break;

            case ActionType.H3s:
                eventTime = 3.0f;
                //eventTime = 0.1f;
                keyHoldTime = 1.5f;
                break;

            case ActionType.H4s:
                eventTime = 4.0f;
                //eventTime = 0.1f;
                keyHoldTime = 2.0f;
                break;
        }
    }

    public void SetFinalInfo()
    {
        eventTime = 5.0f;
        targetCount = 100;
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
                {
                    keyDownCount++;
                    break;
                }
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
                keyHoldTimer = keyHoldTimer <= 0 ? 0 : keyHoldTimer - Time.deltaTime;
        }
        else
        {
            if (keyArr.Any(key => Input.GetKey(key)))
                keyHoldTimer += Time.deltaTime;
            else
                keyHoldTimer = keyHoldTimer <= 0 ? 0 : keyHoldTimer - Time.deltaTime;
        }

        if (keyHoldTime <= keyHoldTimer)
            isClear = true;
    }

    void OnDisable()
    {
        eventTimer = 0.0f;
        keyHoldTimer = 0.0f;
        keyDownCount = 0;
        //pause = false;
        isClear = false;
        Type_Txt.text = string.Empty;

        EventBar_Img.fillAmount = 0.0f;
        HoldBar_Img.fillAmount = 0.0f;

        SetTermTimer();
    }

    void SetTermTimer()
    {
        if (!GameMgr.Inst.start) return;

        GameMgr.Inst.TermTime =
            GameMgr.Inst.termTable[(int)GameMgr.Inst.Step][termCount];
    }

    //void SetClearText()
    //{
    //    if (isClear)
    //        GameMgr.Inst.Clear_txt.text = "Clear!!!";
    //    else
    //        GameMgr.Inst.Clear_txt.text = "Fail!!!";
    //}
}