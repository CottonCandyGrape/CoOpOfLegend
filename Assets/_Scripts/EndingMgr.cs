using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingMgr : MonoBehaviour
{
    public Image Family = null;
    public Image Credit = null;
    public Sprite[] CreditArr = null;

    WaitForSeconds waitSec = new WaitForSeconds(5.0f);
    WaitForSeconds waitSec2 = new WaitForSeconds(10.0f);

    public AudioClip EndingBgmClip = null;
    AudioSource audioSrc = null;

    Color tmp;
    float fadeSpeed = 0.5f;

    void Start()
    {
        Time.timeScale = 1.0f;

        if (audioSrc == null)
            audioSrc = GetComponent<AudioSource>();

        audioSrc.clip = EndingBgmClip;
        audioSrc.volume = 1.0f;
        audioSrc.loop = true;
        audioSrc.Play();

        StartCoroutine(EndingCredit());
    }

    //void Update() { }

    IEnumerator EndingCredit()
    {
        StartCoroutine(BlinkImage(Credit));
        yield return waitSec2;

        Credit.sprite = CreditArr[1];
        StartCoroutine(BlinkImage(Credit));
        yield return waitSec2;

        Credit.sprite = CreditArr[2];
        StartCoroutine(BlinkImage(Credit));
        yield return waitSec2;

        StartCoroutine(FadeOut(Family));
        yield return waitSec;

        SceneManager.LoadScene("Lobby");
    }

    IEnumerator BlinkImage(Image img)
    {
        tmp = img.color;
        tmp.a = 0;
        img.color = tmp;

        while (tmp.a < 1.0f)
        {
            tmp.a += fadeSpeed * Time.deltaTime;
            img.color = tmp;
            yield return null;
        }

        yield return waitSec;

        while (tmp.a > 0.0f)
        {
            tmp.a -= fadeSpeed * Time.deltaTime;
            img.color = tmp;
            yield return null;
        }
    }

    IEnumerator FadeOut(Image img)
    {
        tmp = img.color;
        while (tmp.a > 0.0f)
        {
            tmp.a -= fadeSpeed * Time.deltaTime;
            img.color = tmp;
            yield return null;
        }
    }
}