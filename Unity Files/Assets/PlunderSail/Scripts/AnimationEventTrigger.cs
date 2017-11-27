using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour {

   public GameObject hammerParticle;


	void ToggleParticle () {
        hammerParticle.SetActive(true);	
	}
}
