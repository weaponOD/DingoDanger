using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour {

	[SerializeField]
	private Vector3 northDir;

	[SerializeField]
	private Transform player;

	[SerializeField]
	private Quaternion objectiveDir;

	[SerializeField]
	public RectTransform northArrow;

	[SerializeField]
	public RectTransform objectiveArrow;

	[SerializeField]
	public Transform objectiveDestination;
	

	private void Update () {
		ChangeNorthDirection ();
		ChangeObjectiveDirection ();
	}

	public void ChangeNorthDirection(){
		northDir.z = player.eulerAngles.y;

		northArrow.localEulerAngles = northDir;
	}

	public void ChangeObjectiveDirection(){
		
		Vector3 dir = transform.position - objectiveDestination.position;

		objectiveDir = Quaternion.LookRotation (dir);

		objectiveDir.z = -objectiveDir.y;

		objectiveDir.x = 0;

		objectiveDir.y = 0;

		objectiveArrow.localRotation = objectiveDir * Quaternion.Euler (northDir);
	}
}
