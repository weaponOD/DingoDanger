using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

//Creates an endless water system with squares
public class EndlessWaterSquare : MonoBehaviour
{
    //The object the water will follow
    public GameObject boatObj;
    //One water square
    public GameObject waterSqrObj;

    //Water square data
    private float squareWidth = 800f;
    private float innerSquareResolution = 5f;
    private float outerSquareResolution = 25f;

    //The list with all water mesh squares == the entire ocean we can see
    List<WaterSquare> waterSquares = new List<WaterSquare>();

    //Stuff needed for the thread
    //The timer that keeps track of seconds since start to update the water because we cant use Time.time in a thread
    float secondsSinceStart;
    //The position of the boat
    Vector3 boatPos;
    //The position of the ocean has to be updated in the thread because it follows the boat
    //Is not the same as pos of boat because it moves with the same resolution as the smallest water square resolution
    Vector3 oceanPos;
    //Has the thread finished updating the water so we can add the stuff from the thread to the main thread
    bool hasThreadUpdatedWater;

    void Start()
    {
        //Create the sea
        CreateEndlessSea();

        //Init the time
        secondsSinceStart = Time.time;
    }

    void Update()
    {
        //UpdateWaterNoThread();

        //Update these as often as possible because we don't know when the thread will run because of pooling
        //and we always need the latest version

        //Update the time since start to get correct wave height which depends on time since start
        secondsSinceStart = Time.time;

        //Update the position of the boat to see if we should move the water
        boatPos = boatObj.transform.position;

        //transform.position = 
    }

    //Update the water with no thread to compare 
    void UpdateWaterNoThread()
    {
        //Update the position of the boat
        boatPos = boatObj.transform.position;

        //Move the water to the boat
        MoveWaterToBoat();

        //Add the new position of the ocean to this transform
        transform.position = oceanPos;
    }

    //Move the endless water to the boat's position in steps that's the same as the water's resolution
    void MoveWaterToBoat()
    {
        //Round to nearest resolution
        float x = innerSquareResolution * (int)Mathf.Round(boatPos.x / innerSquareResolution);
        float z = innerSquareResolution * (int)Mathf.Round(boatPos.z / innerSquareResolution);

        //Should we move the water?
        if (oceanPos.x != x || oceanPos.z != z)
        {
            //Debug.Log("Moved sea");

            oceanPos = new Vector3(x, oceanPos.y, z);
        }
    }

    //Init the endless sea by creating all squares
    void CreateEndlessSea()
    {
        //The center piece
        AddWaterPlane(0f, 0f, 0f, squareWidth, innerSquareResolution);

        //The 8 squares around the center square
        for (int x = -1; x <= 1; x += 1)
        {
            for (int z = -1; z <= 1; z += 1)
            {
                //Ignore the center pos
                if (x == 0 && z == 0)
                {
                    continue;
                }

                //The y-Pos should be lower than the square with high resolution to avoid an ugly seam
                float yPos = -0.5f;
                AddWaterPlane(x * squareWidth, z * squareWidth, yPos, squareWidth, outerSquareResolution);
            }
        }
    }

    //Add one water plane
    void AddWaterPlane(float xCoord, float zCoord, float yPos, float squareWidth, float spacing)
    {
        GameObject waterPlane = Instantiate(waterSqrObj, transform.position, transform.rotation) as GameObject;

        waterPlane.SetActive(true);

        //Change its position
        Vector3 centerPos = transform.position;

        centerPos.x += xCoord;
        centerPos.y = yPos;
        centerPos.z += zCoord;

        waterPlane.transform.position = centerPos;

        //Parent it
        waterPlane.transform.parent = transform;

        //Give it moving water properties and set its width and resolution to generate the water mesh
        WaterSquare newWaterSquare = new WaterSquare(waterPlane, squareWidth, spacing);

        waterSquares.Add(newWaterSquare);
    }
}