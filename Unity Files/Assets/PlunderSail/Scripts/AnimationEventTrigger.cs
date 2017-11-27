using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventTrigger : MonoBehaviour {

   public GameObject hammerParticle;

    void Start()
    {
        //hammerParticle.SetActive(true);
    }

    public void ToggleParticle () {
        hammerParticle.SetActive(true);	
	}
}
