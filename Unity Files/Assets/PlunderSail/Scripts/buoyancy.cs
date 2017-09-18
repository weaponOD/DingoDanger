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
    private float waveHeight = 1f;

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * waveHeight, transform.position.z);
    }
}
