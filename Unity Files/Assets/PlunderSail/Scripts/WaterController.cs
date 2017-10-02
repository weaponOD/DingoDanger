using UnityEngine;
using System.Collections;

//Controlls the water
public class WaterController : MonoBehaviour
{
    public static WaterController current;

    public bool isMoving;

    //Wave height and speed
    public float scale = 0.1f;
    public float speed = 1.0f;
    //The width between the waves
    public float waveDistance = 1f;
    //Noise parameters
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;

    void Start()
    {
        current = this;
    }
}