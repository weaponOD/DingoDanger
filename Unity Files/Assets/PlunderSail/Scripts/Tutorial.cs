using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance = null;

    [Header("Seconds before hint")]

    [SerializeField]
    private float firingDelay = 10f;

    [SerializeField]
    [Tooltip("The text gameObject that will show the tip")]
    private GameObject firingTip = null;

    [SerializeField]
    private float aimingDelay = 10f;

    [SerializeField]
    [Tooltip("The text gameObject that will show the tip")]
    private GameObject aimingTip = null;


    private bool hasFired = false;
    private bool hasAimed = false;

    private void Awake()
    {
        instance = this;
    }

    public void StartTimer()
    {
        Invoke("ShowFiringTip", firingDelay);
        Invoke("ShowAimingTip", aimingDelay);
    }

    private void ShowFiringTip()
    {
        if (!hasFired)
        {
            // Show fire tip
            if (firingTip != null)
            {
                StartCoroutine(FadeTextToFullAlpha(2f, firingTip.GetComponent<Text>()));

                StartCoroutine(FadeTextToZeroAlpha(3f, 2f, firingTip.GetComponent<Text>()));
            }
        }
    }

    private void ShowAimingTip()
    {
        if (!hasAimed)
        {
            // Show aim tip

            if (aimingTip != null)
            {
                StartCoroutine(FadeTextToFullAlpha(2f, aimingTip.GetComponent<Text>()));

                StartCoroutine(FadeTextToZeroAlpha(3f, 2f, aimingTip.GetComponent<Text>()));
            }
        }
    }

    public bool HasFired
    {
        set { hasFired = value; }
    }

    public bool HasAimed
    {
        set { hasAimed = value; }
    }

    private IEnumerator FadeTextToFullAlpha(float _time, Text _text)
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
        while (_text.color.a < 1.0f)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a + (Time.deltaTime / _time));
            yield return null;
        }
    }

    private IEnumerator FadeTextToZeroAlpha(float _delay, float _time, Text _text)
    {
        yield return new WaitForSeconds(_delay);

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);
        while (_text.color.a > 0.0f)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - (Time.deltaTime / _time));
            yield return null;
        }
    }
}