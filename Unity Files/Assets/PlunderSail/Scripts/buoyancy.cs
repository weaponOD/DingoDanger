using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script component that will add a buoyancy effect.
/// </summary>
public class buoyancy : MonoBehaviour
{

    [SerializeField]
    [Range(0, 1)]
    [Tooltip("Height of the sin wave that makes the object bob up and down.")]
    private float OriginalwaveHeight = 0.35f;

    private Vector3 OriginalPos;

    private float waveHeight = 0.35f;

    private void Awake()
    {
        OriginalPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, OriginalPos.y + Mathf.Sin(Time.time) * waveHeight - 0.7f, transform.position.z);
    }

    public float WaveHeight
    {
        get { return waveHeight; }

        set {

            if(value > 0)
            {
                waveHeight = value;
            }
        }
    }

    public void ResetWaveHeight()
    {
        WaveHeight = OriginalwaveHeight;
    }
}
