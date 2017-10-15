using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class LaunchArcMesh : MonoBehaviour {

	Mesh mesh;

	[SerializeField]
	private float meshWidth;

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
		mesh = GetComponent<MeshFilter> ().mesh;

		//Find the absolute value of gravity, not negative/positive
		gravity =Mathf.Abs(Physics.gravity.y);

	}

	//event listener
	private void OnValidate(){
		//Check that mesh is not null, and that the game is playing
		if(mesh != null && Application.isPlaying){
			CreateArcMesh (CalcArcArray());
		}
	}

	private void Start(){
		CreateArcMesh (CalcArcArray());
	}
		
	private void CreateArcMesh(Vector3[] _arcVertices){
		//Clear out previous vertex and triangle information 
		mesh.Clear ();

		//set size of vertices and triangles based on divisions variable

		Vector3[] vertices = new Vector3[(divisions + 1) * 2];

		//every quad is two triangles (6 vertices)
		int[] triangles = new int[divisions * 6 * 2]; //multiply again to make meshes double sided

		for (int i = 0; i <= divisions; i++) {
			//set vertices, create one side of the arc mesh
			vertices[i * 2] = new Vector3(meshWidth * 0.5f, _arcVertices[i].y, _arcVertices[i].x); // x is our width, y is height, and z is distance 
			//create the opposite side, using negatives and odd numbers (+1)
			vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, _arcVertices[i].y, _arcVertices[i].x); // x is our width, y is height, and z is distance 

				//set triangles

				//we are'nt on last iteration, make sure we skip last iteration
				if(i != divisions){
					triangles[i * 12] = i * 2;
					triangles[i * 12 + 1] = triangles [i * 12 + 4] = i * 2 + 1;
					triangles[i * 12 + 2] = triangles [i * 12 + 3] = (i + 1) * 2;
					triangles[i * 12 + 5] = (i + 1) * 2 +1;

					triangles[i * 12 + 6] = i * 2;
					triangles[i * 12 + 7] = triangles [i * 12 + 10] = (i + 1) * 2;
					triangles[i * 12 + 8] = triangles [i * 12 + 9] = i * 2 + 1;
					triangles[i * 12 + 11] = (i + 1) * 2 +1;
				}

			mesh.vertices = vertices;
			mesh.triangles = triangles;



		}

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
