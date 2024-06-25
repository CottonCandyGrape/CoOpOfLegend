using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    QTEStart,
    QTESuccess,
    QTEFail,
    StageClear,
    StageFail
}

public class SoundMgr : MonoBehaviour
{
    public AudioClip InGameBgmClip = null;
    AudioSource audioSrc = null;

    public AudioClip[] adClipArr = null;
    AudioSource[] sfxSrcList = new AudioSource[10];

    const int MaxSfxCnt = 5;
    int sfxCnt = 0;

    public static SoundMgr Inst = null;

    void Awake() { Inst = this; }

    void Start()
    {
        if (audioSrc == null)
            audioSrc = GetComponent<AudioSource>();

        audioSrc.clip = InGameBgmClip;
        audioSrc.volume = 1.0f;
        audioSrc.loop = true;
        audioSrc.Play();

        SetSfxSrc();
    }

    void SetSfxSrc()
    {
        for (int i = 0; i < MaxSfxCnt; i++)
        {
            GameObject newSoundObj = new GameObject(); // empty gameobject 만들기
            newSoundObj.transform.SetParent(this.transform);
            newSoundObj.transform.localPosition = Vector3.zero; //transform reset
            AudioSource a_AudioSrc = newSoundObj.AddComponent<AudioSource>();
            a_AudioSrc.playOnAwake = false;
            a_AudioSrc.loop = false;
            newSoundObj.name = "SoundEffObj";

            sfxSrcList[i] = a_AudioSrc;
        }
    }

    public void TurnOffBgm()
    {
        if(audioSrc.clip != null)
        {
            audioSrc.Stop();
            audioSrc.time = 0.0f;
        }
    }

    public void PlaySfxSound(SfxType sType)
    {
        AudioClip clip = null;
        clip = adClipArr[(int)sType];

        if (sfxSrcList[sfxCnt] != null)
        {
            sfxSrcList[sfxCnt].volume = 1.0f;
            sfxSrcList[sfxCnt].PlayOneShot(clip, 1.0f);
        }

        sfxCnt++;
        if (MaxSfxCnt <= sfxCnt)
            sfxCnt = 0;
    }
}