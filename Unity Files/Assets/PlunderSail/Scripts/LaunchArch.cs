using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LaunchArch : MonoBehaviour {

	LineRenderer arcLineRenderer;

	//**********************************************************************************
	//**May want to create a method that populates all this info from another script ***
	//**********************************************************************************

	[SerializeField]
	[Range (0, 40)]
	private float velocity;

	[SerializeField]
	[Range (0, 85)]
	private float angle;

	[SerializeField]
	private int divisions = 10; //Resolution of the curve,or how many divisions will be in it. Higher number creates a smoother arc

	private float gravity; //gravity force on the Y axis

	private float radianAngle;

	//**********************************************************************************
	private void Awake(){
		arcLineRenderer = GetComponent < LineRenderer> ();

		//Find the absolute value of gravity, not negative/positive
		gravity =Mathf.Abs(Physics.gravity.y);

	}

	//event listener
	private void OnValidate(){
		//Check that line renderer is not null, and that the game is playing
		if(arcLineRenderer != null && Application.isPlaying){
			RenderArc ();
		}
	}

	private void Start(){
		RenderArc ();
	}

	//populating the line renderer with the appropriate settings
	private void RenderArc(){
		
		//we add 1 because the arc needs an endpoint also
		arcLineRenderer.SetVertexCount (divisions + 1); 

		arcLineRenderer.SetPositions (CalcArcArray () );
	}

	//create an array of Vector3 positions for Arc
	private Vector3[] CalcArcArray(){

		//same number as render arc array
		Vector3[] arcArray = new Vector3[divisions + 1]; 

		radianAngle = Mathf.Deg2Rad * angle;
		float maxDistance = (velocity * velocity * Mathf.Sin (2 * radianAngle)) / gravity;

		//<= for extra piece at end (end point)
		for (int i = 0; i <= divisions; i++) {
			//determine how far along in arc we are
			float t = (float)i / (float)divisions;

			arcArray [i] = CalculateArcPoint (t, maxDistance);
		}

		return arcArray;
	}

	//Calculate height adn Distance of each vertex point in array

	// t = distance along array
	private Vector3 CalculateArcPoint(float _t, float _maxDistance ){
		float x = _t * _maxDistance;
		//get height at any point
		float y = x * Mathf.Tan (radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos (radianAngle) * Mathf.Cos (radianAngle)));

		//Do not need z
		return new Vector3 (x, y);
	}
}
