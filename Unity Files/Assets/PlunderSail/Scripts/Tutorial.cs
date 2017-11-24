using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public static Tutorial instance = null;

    [Header("has no sails")]
    [SerializeField]
    private Image noSailsHint;

    [Header("hasn't used aimed")]
    [SerializeField]
    private Image hasntAimedHint;

    [SerializeField]
    private float hasntAimedTime;

    [Header("hasn't used map")]
    [SerializeField]
    private Image hasntUsedMapHint;

    [SerializeField]
    private float hasntUsedMapTime;

    [Header("hasn't used Idle")]
    [SerializeField]
    private Image usedIdleHint;

    [SerializeField]
    private float usedIdleTime;

    [Header("hasn't changed Sail States")]
    [SerializeField]
    private Image changedStateHint;

    [SerializeField]
    private float changedStateTime;

    [Header("hasn't built cannons")]
    [SerializeField]
    private Image noCannonsHint;

    [SerializeField]
    private float noCannonsTime;

    private bool hasAimed = false;

    private bool hasUsedMap = false;

    private bool usedIdle = false;

    private bool changedState = false;

    private bool hasCannons = false;

    public bool HasAimed
    {
        set { hasAimed = value; }
    }

    public bool HasUsedMap
    {
        set { hasUsedMap = value; }
    }

    public bool UsedIdle
    {
        set { usedIdle = value; }
    }

    public bool ChangedState
    {
        set { changedState = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    public void StartTimer()
    {
        
    }

    private void ShowAimingTip()
    {
        if (!hasAimed)
        {
            // Show aim tip

            if (hasntAimedHint != null)
            {
                hasntAimedHint.gameObject.SetActive(true);

                StartCoroutine(Fade(hasntAimedHint, Color.clear, Color.white, 1f, 0f));

                StartCoroutine(Fade(hasntAimedHint, Color.white, Color.clear,1f, 2f));
            }
        }
    }

    private void ShowMapTip()
    {
        if (!hasUsedMap)
        {
            // Show map tip

            if (hasntUsedMapHint != null)
            {
                hasntUsedMapHint.gameObject.SetActive(true);

                StartCoroutine(Fade(hasntUsedMapHint, Color.clear, Color.white, 1f, 0f));

                StartCoroutine(Fade(hasntUsedMapHint, Color.white, Color.clear, 1f, 2f));
            }
        }
    }

    private void ShowIdleTip()
    {
        if (!usedIdle)
        {
            // Show map tip

            if (usedIdleHint != null)
            {
                usedIdleHint.gameObject.SetActive(true);

                StartCoroutine(Fade(usedIdleHint, Color.clear, Color.white, 1f, 0f));

                StartCoroutine(Fade(usedIdleHint, Color.white, Color.clear, 1f, 2f));
            }
        }
    }

    private void ShowStateTip()
    {
        if (!changedState)
        {
            // Show map tip

            if (changedStateHint != null)
            {
                changedStateHint.gameObject.SetActive(true);

                StartCoroutine(Fade(changedStateHint, Color.clear, Color.white, 1f, 0f));

                StartCoroutine(Fade(changedStateHint, Color.white, Color.clear, 1f, 2f));
            }
        }
    }

    private void ShowCannonsTip()
    {
        if (!hasCannons)
        {
            // Show map tip

            if (noCannonsHint != null)
            {
                noCannonsHint.gameObject.SetActive(true);

                StartCoroutine(Fade(noCannonsHint, Color.clear, Color.white, 1f, 0f));

                StartCoroutine(Fade(noCannonsHint, Color.white, Color.clear, 1f, 2f));
            }
        }
    }

    // Fades the fadePlane image from a colour to another over x seconds.
    private IEnumerator Fade(Image _image, Color _from, Color _to, float _time, float _delay)
    {
        yield return new WaitForSecondsRealtime(_delay);

        float speed = 1 / _time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            _image.color = Color.Lerp(_from, _to, percent);

            yield return null;
        }

        _image.gameObject.SetActive(false);
    }
}