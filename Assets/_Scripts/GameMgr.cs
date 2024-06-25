using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CurrentStep { Step1, Step2, Step3, Step4, Step5 }

public class GameMgr : MonoBehaviour
{
    [HideInInspector] public CurrentStep Step = CurrentStep.Step1;
    public const int step2 = 2;
    public const int step3 = 7;
    public const int step4 = 18;
    public const int step5 = 39;

    public Text EventNum_txt = null;
    //public Text Clear_txt = null;

    public QTEvent[] NormalQTEvents = null;
    //public QTEvent[] SpecialQTEvents = null;
    public GameObject FinalQTEvents = null;

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
    //[HideInInspector]
    //public float[][] termTable = new float[][] {
    //    new float[]{ 0.1f, 0.1f },
    //    new float[]{ 0.1f, 0.1f, 0.1f, 0.1f, 0.1f },
    //    new float[]{ 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f },
    //    new float[]{ 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 
    //        0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f, 0.1f }
    //};
    [HideInInspector]
    public float[][] termTable = new float[][] {
        new float[]{ 3.0f, 3.0f },
        new float[]{ 3.0f, 1.5f, 1.5f, 3.0f, 3.0f },
        new float[]{ 3.0f, 3.0f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 4.0f, 4.0f, 4.0f },
        new float[]{ 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 
            1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 1.5f, 6.0f, 6.0f, 6.0f, 6.0f }
    };
    int termCount = 0;

    [HideInInspector] public bool start = false;

    public Transform canvas = null;

    public static GameMgr Inst = null;

    void Awake() { Inst = this; }

    void Start()
    {
        termCount = 0;
        //EventNum_txt.text = "Step1";
        termTime = Random.Range(2.0f, 3.0f); //시작시 랜덤 텀
    }

    void Update()
    {
        //if (MaxFailCount < failCount) // GameOver
        //{
        //    SceneManager.LoadScene("GameOver");
        //    return;
        //}

        //EventNum_txt.text = eventCount.ToString(); //TODO : 지워야함.

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
                if (MaxEventCount < eventCount)
                {
                    Debug.Log("asdlfkjadsf");
                    StepEventCount();
                    SpawnFinalEvent();
                }
                else
                    SpawnEvent();

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

    public void SpawnEvent()
    {
        int qtIdx = GetInactiveNorQTEventIdx();
        int acIdx = Random.Range(0, (int)ActionType.Length);

        NormalQTEvents[qtIdx].enabled = true;
        NormalQTEvents[qtIdx].SetEventInfo((ActionType)acIdx);
        NormalQTEvents[qtIdx].TermCount = termCount;
        NormalQTEvents[qtIdx].DelayTime = GetDelayTimer();

        StepEventCount();
        termCount++;
        EventNum_txt.text = eventCount + " / " + termCount;

        //NormalQTEvents[3].gameObject.SetActive(true);
        //NormalQTEvents[3].SetEventInfo(ActionType.H4s);
    }

    void SpawnFinalEvent()
    {
        Debug.Log("SpawnFinalEvent");
        GameObject finalObj = Instantiate(FinalQTEvents, canvas);
        QTEvent final = finalObj.GetComponent<QTEvent>();
        if (final != null) final.SetFinalInfo();
    }

    public float GetDelayTimer()
    {
        float delayTime = -1.0f;

        if (Step == CurrentStep.Step2)
        {
            if (termCount == 1)
                return 0.5f;
            else if (termCount == 3)
                return 0.0f;
        }
        else if (Step == CurrentStep.Step3)
        {
            if (termCount == 0 || termCount == 5 || termCount == 8 || termCount == 9)
                return 0.0f;
            else if (termCount == 3 || termCount == 6)
                return 0.5f;
            else if (termCount == 2)
                return 1.0f;
        }
        else if (Step == CurrentStep.Step4)
        {
            if (termCount == 0 || termCount == 7 || termCount == 17 || termCount == 18 || termCount == 19)
                return 0.0f;
            else if (termCount == 1 || termCount == 3 || termCount == 5 || termCount == 6 ||
                termCount == 9 || termCount == 10 || termCount == 11 || termCount == 14 || termCount == 15)
                return 0.5f;
            else if (termCount == 13)
                return 1.0f;
        }

        return delayTime;
    }

    void StepEventCount()
    {
        eventCount++;
        Debug.Log(Step.ToString() + " : " + eventCount);

        if (eventCount == step2 + 1)
        {
            //EventNum_txt.text = CurrentStep.Step2.ToString();
            Step = CurrentStep.Step2;
            termCount = 0;
        }
        else if (eventCount == step3 + 1)
        {
            //EventNum_txt.text = CurrentStep.Step3.ToString();
            Step = CurrentStep.Step3;
            termCount = 0;
        }
        else if (eventCount == step4 + 1)
        {
            //EventNum_txt.text = CurrentStep.Step4.ToString();
            Step = CurrentStep.Step4;
            termCount = 0;
        }
        else if (eventCount == step5 + 1)
        {
            //EventNum_txt.text = CurrentStep.Step4.ToString();
            Step = CurrentStep.Step5;
            termCount = 0;
        }
    }

    int GetInactiveNorQTEventIdx()
    {
        List<int> tmp = new List<int>();

        for (int i = 0; i < NormalQTEvents.Length; i++)
        {
            if (!NormalQTEvents[i].enabled)
                tmp.Add(i);
        }

        if (tmp.Count <= 0)
            Debug.Log("tmp count is " + tmp.Count);

        int idx = Random.Range(0, tmp.Count);
        return tmp[idx];
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