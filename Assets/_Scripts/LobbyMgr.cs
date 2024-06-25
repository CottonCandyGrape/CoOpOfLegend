using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMgr : MonoBehaviour
{
    AudioSource audioSrc = null;
    public AudioClip TitleClip = null;

    void Start()
    {
        if (audioSrc == null)
            audioSrc = GetComponent<AudioSource>();

        audioSrc.clip = TitleClip;
        audioSrc.volume = 1.0f;
        audioSrc.loop = true;
        audioSrc.Play();
    }

    void Update()
    {
        if(Input.anyKeyDown)
            SceneManager.LoadScene("InGame");
    }
}