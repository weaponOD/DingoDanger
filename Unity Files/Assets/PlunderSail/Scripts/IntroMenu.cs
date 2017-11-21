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
    private Image FadePlane;

    [SerializeField]
    private Image loading;

    private bool waitOnInput = false;

    private AsyncOperation async;

    public float progress;

    public bool isDone = false;

    private void Start ()
    {
        StartCoroutine(LoadScene());

        waitOnInput = true;
    }

    private void Update()
    {
        isDone = async.isDone;

        if (waitOnInput)
        {
            if (Input.anyKeyDown)
            {
                StartCoroutine(ChangeScenes());
                waitOnInput = false;
            }
        }
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
    }

    private IEnumerator ChangeScenes()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1, FadePlane));
        StartCoroutine(Fade(Color.clear, Color.white, 1, loading));

        yield return new WaitForSeconds(1f);

        async.allowSceneActivation = true;
    }
}