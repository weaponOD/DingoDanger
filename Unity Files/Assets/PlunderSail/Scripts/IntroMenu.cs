using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroMenu : MonoBehaviour
{
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

    public float progress;

    public bool isDone = false;

    private void Start ()
    {
        StartCoroutine(LoadScene());
        StartCoroutine(IntroFadeIn());

        range = maxSize - minSize;
    }

    private void Update()
    {
        PressAnyKey.rectTransform.localScale = new Vector3(minSize + Mathf.PingPong(Time.time * speed, range), minSize + Mathf.PingPong(Time.time * speed, range), 1);

        
        isDone = async.isDone;

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
        StartCoroutine(Fade(Color.clear, Color.white, 4, PlunderSail));

        yield return new WaitForSeconds(3.5f);

        waitOnInput = true;

        StartCoroutine(Fade(Color.clear, Color.white, 2, PressAnyKey));

        yield return new WaitForSeconds(2f);
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

        async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (async.progress <= 0.89f)
        {
            progress = async.progress;
            yield return null;
        }

        async.allowSceneActivation = true;
    }

    private IEnumerator ChangeScenes()
    {
        StartCoroutine(Fade(Color.white, Color.clear, 2, PlunderSail));
        StartCoroutine(Fade(Color.white, Color.clear, 2, PressAnyKey));

        yield return new WaitForSeconds(3f);
    }
}