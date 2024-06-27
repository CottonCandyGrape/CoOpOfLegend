using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMgr : MonoBehaviour
{
    public GameObject TutorialObj = null;
    float tutorialTime = 15.0f;
    float tutorialTimer = 0.0f;

    AudioSource audioSrc = null;
    public AudioClip TitleClip = null;

    void Start()
    {
        Time.timeScale = 1.0f;

        if (audioSrc == null)
            audioSrc = GetComponent<AudioSource>();

        audioSrc.clip = TitleClip;
        audioSrc.volume = 1.0f;
        audioSrc.loop = true;
        audioSrc.Play();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            TutorialObj.SetActive(true);

        if (TutorialObj.activeSelf)
        {
            tutorialTimer += Time.deltaTime;
            if (tutorialTime <= tutorialTimer)
            {
                tutorialTimer = 0.0f;
                SceneManager.LoadScene("InGame");
            }
        }
    }
}