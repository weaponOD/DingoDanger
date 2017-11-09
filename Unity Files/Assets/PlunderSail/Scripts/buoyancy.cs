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

    [SerializeField]
    [Range(0, 1)]
    private float speed = 0.35f;

    private Vector3 targetPos;

    private float waveHeight = 0.35f;

    private void Start()
    {
        waveHeight = OriginalwaveHeight;
    }

    void Update()
    {
        targetPos = new Vector3(transform.position.x, Mathf.Cos(Time.time) * waveHeight, transform.position.z);
        Vector3 vecBetween = targetPos - transform.position;

        transform.position += vecBetween.normalized * Time.deltaTime * speed;

        //if (vecBetween.magnitude > 0.2f)
        //{
            
        //}
        //else
        //{
        //    transform.position = new Vector3(transform.position.x, -1 + Mathf.Sin(Time.time) * waveHeight - 0.7f, transform.position.z);
        //}
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
