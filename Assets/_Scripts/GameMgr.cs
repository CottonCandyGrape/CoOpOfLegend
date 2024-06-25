using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CurrentStep { Step1, Step2, Step3, Step4 }

public class GameMgr : MonoBehaviour
{
    [HideInInspector] public CurrentStep Step = CurrentStep.Step1;
    public const int step2 = 2;
    public const int step3 = 7;
    public const int step4 = 18;

    public Text EventNum_txt = null;
    public Text Clear_txt = null;

    public QTEvent[] NormalQTEvents = null;
    public QTEvent[] SpecialQTEvents = null;
    public QTEvent FinalQTEvents = null;

    const int MaxEventCount = 38;
    int eventCount = 0;
    public int EventCount { get { return eventCount; } }

    int failCount = 0;
    public int FailCount
    {
        get { return failCount; }
        set { failCount = value; }
    }
    const int MaxFailCount = 3;

    float termTimer = 0.0f;
    float termTime = 0.0f;
    public float TermTime { set { termTime = value; } }
    [HideInInspector]
    public float[][] termTable = new float[][] {
        new float[]{ 3.0f, 3.0f },
        new float[]{ 3.0f, 0.5f, 1.5f, 0.0f, 3.0f },
        new float[]{ 0.0f, 3.0f, 1.0f, 0.5f, 1.5f, 0.0f, 0.5f, 1.5f, 0.0f, 0.0f, 4.0f },
        new float[]{ 0.0f, 0.5f, 1.5f, 0.5f, 1.5f, 0.5f, 0.5f, 0.0f, 1.5f, 
            0.5f, 0.5f, 0.5f, 0.5f, 1.5f, 1, 0.5f, 0.5f, 0.0f, 1.5f, 0.0f, 5.0f }
    };
    int termCount = 0;
    public int TermCount
    {
        get { return termCount; }
        set { termCount = value; }
    }

    [HideInInspector] public bool start = false;

    public static GameMgr Inst = null;

    void Awake() { Inst = this; }

    void Start()
    {
        termCount = 0;
        EventNum_txt.text = "Step1";
        termTime = Random.Range(2.0f, 3.0f); //시작시 랜덤 텀
    }

    void Update()
    {
        if (MaxFailCount < failCount || MaxEventCount < eventCount) // GameOver
        {
            SceneManager.LoadScene("GameOver");
            return;
        }

        EventNum_txt.text = eventCount.ToString();

        //if (Input.GetKeyDown(KeyCode.P))
        //    SpawnNormalEvent();

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    start = true;
        //    Debug.Log("Start");
        //}
        //if (!start) return; //이걸 써야 인덱스 에러 안쌓이고 안난다. //nono. disable 때문인데 스크립트 끄고 시작하면됨.

        if (IsMainThread())
        {
            termTimer += Time.deltaTime;

            if (termTime <= termTimer)
            {
                SpawnNormalEvent();

                termTimer = 0.0f;
            }
        }
    }

    bool IsMainThread()
    {
        for (int i = 0; i < NormalQTEvents.Length; i++)
        {
            if (NormalQTEvents[i].enabled)
                return false;
        }

        return true;
    }

    //void SpawnNormalEvent(int qtIdx, int acIdx)
    void SpawnNormalEvent()
    {
        int qtIdx = Random.Range(0, NormalQTEvents.Length);
        int acIdx = Random.Range(0, (int)ActionType.Length);
        //NormalQTEvents[qtIdx].gameObject.SetActive(true);
        NormalQTEvents[qtIdx].enabled = true;

        NormalQTEvents[qtIdx].SetEventInfo((ActionType)acIdx);
        StepEventCount();

        //NormalQTEvents[qtIdx].SetEventInfo((ActionType)acIdx, eventCount);
        //NormalQTEvents[3].gameObject.SetActive(true);
        //NormalQTEvents[3].SetEventInfo(ActionType.H4s);
        //NormalQTEvents[0].SetActionType(ActionType.P20);
    }

    void StepEventCount()
    {
        eventCount++;

        if (eventCount == step2 + 1)
        {
            EventNum_txt.text = CurrentStep.Step2.ToString();
            Step = CurrentStep.Step2;
            termCount = 0;
        }
        else if (eventCount == step3 + 1)
        {
            EventNum_txt.text = CurrentStep.Step3.ToString();
            Step = CurrentStep.Step3;
            termCount = 0;
        }
        else if (eventCount == step4 + 1)
        {
            EventNum_txt.text = CurrentStep.Step4.ToString();
            Step = CurrentStep.Step4;
            termCount = 0;
        }
    }

    List<int> GetDiffInt(int n, int r)
    {
        if (n < r) return new List<int>();

        List<int> result = new List<int>();

        List<int> tmp = new List<int>();
        for (int i = 0; i < n; i++)
            tmp.Add(i);

        for (int i = 0; i < r; i++)
        {
            int idx = Random.Range(0, tmp.Count);
            result.Add(tmp[idx]);
            tmp.RemoveAt(idx);
        }

        return result;
    }

    /*
    int GetRepeatNum()
    {
        if (eventCount == 4 || eventCount == 6 || eventCount == 11 || eventCount == 17
            || eventCount == 24 || eventCount == 33 || eventCount == 35)
            return 2;
        else if (eventCount == 14)
            return 3;

        return -1;
    }
    */
}