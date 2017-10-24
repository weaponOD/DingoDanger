using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{
    [SerializeField]
    private Image DangerDingo;

    [SerializeField]
    private Image PlunderSail;

    [SerializeField]
    private Text PressAnyKey;

    [SerializeField]
    private Image FadePlan;

    private bool waitOnInput = false;

    private AsyncOperation async;

    [Header("Text Animation Stats")]

    [SerializeField]
    private float minSize;

    [SerializeField]
    private float maxSize;

    [SerializeField]
    private float speed;

    private float range;

    private void Start ()
    {
        StartCoroutine(LoadScene());
        StartCoroutine(IntroFadeIn());

        range = maxSize - minSize;
    }

    private void Update()
    {
        PressAnyKey.rectTransform.localScale = new Vector3(minSize + Mathf.PingPong(Time.time * speed, range), minSize + Mathf.PingPong(Time.time * speed, range), 1);

        if (waitOnInput)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(ChangeScenes());
            }
        }
    }

    private IEnumerator IntroFadeIn()
    {
        StartCoroutine(Fade(Color.clear, Color.white, 4, DangerDingo));

        yield return new WaitForSeconds(4f);

        StartCoroutine(Fade(Color.white, Color.clear, 3, DangerDingo));

        yield return new WaitForSeconds(2.8f);

        StartCoroutine(Fade(Color.clear, Color.white, 4, PlunderSail));

        yield return new WaitForSeconds(3.5f);

        StartCoroutine(Fade(Color.clear, Color.white, 2, PressAnyKey));

        yield return new WaitForSeconds(2f);

        waitOnInput = true;
    }

    // Fades the fadePlane image from a colour to another over x seconds.
    private IEnumerator Fade(Color from, Color to, float time, Image _plane)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            _plane.color = Color.Lerp(from, to, percent);

            yield return null;
        }
    }

    // Fades the fadePlane image from a colour to another over x seconds.
    private IEnumerator Fade(Color from, Color to, float time, Text _text)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            _text.color = Color.Lerp(from, to, percent);

            yield return null;
        }
    }

    private IEnumerator LoadScene()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");

        async = SceneManager.LoadSceneAsync(1);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            yield return null;
        }

        Debug.LogWarning("ASYNC LOAD FINISHED - SAFE TO EXIT PLAY MODE");
    }

    private IEnumerator ChangeScenes()
    {
        StartCoroutine(Fade(Color.white, Color.clear, 2, PlunderSail));
        StartCoroutine(Fade(Color.white, Color.clear, 2, PressAnyKey));

        yield return new WaitForSeconds(2f);

        async.allowSceneActivation = true;
    }
}