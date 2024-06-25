using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CurrentStep { Step1, Step2, Step3, Step4 }

public class GameMgr : MonoBehaviour
{
    [HideInInspector] public CurrentStep Step = CurrentStep.Step1;
    public const int step2 = 2;
    public const int step3 = 7;
    public const int step4 = 18;

    public Text txt = null;

    public QTEvent[] NormalQTEvents = null;
    public QTEvent[] SpecialQTEvents = null;
    public QTEvent FinalQTEvents = null;

    int eventCount = 0;
    public int EventCount { get { return eventCount; } }
    int failCount = 0;
    public int FailCount
    {
        get { return failCount; }
        set { failCount = value; }
    }
    const int MaxFailCount = 3;

    float termTime = 0.0f;
    public float TermTime { set { termTime = value; } }
    float termTimer = 0.0f;
    [HideInInspector]
    public float[][] termTable = new float[][] {
        new float[]{ 3.0f, 3.0f },
        new float[]{ 3.0f, 0.5f, 1.5f, 3.0f },
        new float[]{ 3.0f, 1.0f, 0.5f, 1.5f, 0.5f, 1.5f, 4.0f },
        new float[]{ 0.5f, 1.5f, 0.5f, 1.5f, 0.5f, 0.5f, 1.5f,
            0.5f, 0.5f, 0.5f, 0.5f, 1.5f, 1, 0.5f, 0.5f, 1.5f, 5.0f }
    };
    int termCount = 0;
    public int TermCount
    {
        get { return termCount; }
        set { termCount = value; }
    }

    public static GameMgr Inst = null;

    void Awake() { Inst = this; }

    void Start()
    {
        txt.text = "Step1";
        termTime = Random.Range(2.0f, 3.0f); //시작시 랜덤 텀
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //    SpawnNormalEvent();

        if (IsMainThread())
        {
            termTimer += Time.deltaTime;

            if (termTime <= termTimer)
            {
                int qtIdx = Random.Range(0, NormalQTEvents.Length);
                int acIdx = Random.Range(0, (int)ActionType.Length);
                SpawnNormalEvent(qtIdx, acIdx);

                termTimer = 0.0f;
                //termTime = 0.0f;
            }
        }
        else
        {
            Debug.Log("Not Main Thread");
        }

        //if (MaxFailCount < failCount) // GameOver
        //{
        //    Debug.Log("GameOver");
        //    return;
        //}
    }

    bool IsMainThread()
    {
        for (int i = 0; i < NormalQTEvents.Length; i++)
        {
            if (NormalQTEvents[i].gameObject.activeSelf)
                return false;
        }

        return true;
    }

    void SpawnNormalEvent(int qtIdx, int acIdx)
    {
        NormalQTEvents[qtIdx].gameObject.SetActive(true);
        NormalQTEvents[qtIdx].SetEventInfo((ActionType)acIdx);
        //NormalQTEvents[qtIdx].SetEventInfo((ActionType)acIdx, eventCount);

        //NormalQTEvents[0].gameObject.SetActive(true);
        //NormalQTEvents[0].SetActionType(ActionType.H4s);
        //NormalQTEvents[0].SetActionType(ActionType.P20);
        StepEventCount();
    }

    void StepEventCount()
    {
        eventCount++;

        if (step2 == eventCount)
        {
            txt.text = CurrentStep.Step2.ToString();
            Step = CurrentStep.Step2;
            termCount = 0;
        }
        else if (step3 == eventCount)
        {
            txt.text = CurrentStep.Step3.ToString();
            Step = CurrentStep.Step3;
            termCount = 0;
        }
        else if (step4 == eventCount)
        {
            txt.text = CurrentStep.Step4.ToString();
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
}