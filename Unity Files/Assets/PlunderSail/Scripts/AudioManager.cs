using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//List of sounds that we can add or remove as we go, each sound will have options to adjust volume, pitch etc. 
//During Start of game we go through list of sounds and find one we need, and call Play Fiunction to play sounds in other scripts


//Call sound effects using this line				AudioManager.instance.PlaySound("soundClassname here");

public class AudioManager : MonoBehaviour
{
	[Tooltip("Add sounds to this Array, their Name will be used to call them.")]
	//Array of Sounds we can access
	public SoundClass [] sounds;

	//Static ref to audio manager
	public static AudioManager instance;

	//Add a audio source to each clip at the beginning of the game.
	void Awake ()
    {

		if(instance == null)
			instance = this;
		else
		{
			//stop the game from having more than one audio manager
			Destroy(gameObject);
			return;
		}

		//Don't destroy the Audio Manager in scene
		DontDestroyOnLoad (gameObject);

		foreach (SoundClass _sounds in sounds)
        {
			_sounds.audioSource =  gameObject.AddComponent<AudioSource> ();

			_sounds.audioSource.volume = _sounds.volume;
			_sounds.audioSource.pitch = _sounds.pitch;
			_sounds.audioSource.loop = _sounds.looping;
		}
	}

	//Call this function through other script
	//e.g AudioManager.instance.Play("CannonImpact");
	public void PlaySound(string _name)
    {
		SoundClass soundClass = Array.Find (sounds, sound => sound.name == _name);

        if (soundClass == null)
        {
			Debug.LogWarning ("Sound: " + _name + " not found.");
			return;
		}

		if(soundClass.audioClip.Length > 1)
        {
            soundClass.audioSource.PlayOneShot(soundClass.audioClip[UnityEngine.Random.Range(0, soundClass.audioClip.Length)]);
        }
        else if (soundClass.audioClip.Length > 0)
        {
            soundClass.audioSource.PlayOneShot(soundClass.audioClip[0]);
        }
        else
        {
            Debug.LogError("No sound was played");
        }
	}
}

[System.Serializable]
public class SoundClass
{
    public string name;

    public AudioClip[] audioClip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.5f, 2f)]
    public float pitch = 1f;

    public bool looping;

    [HideInInspector]
    public AudioSource audioSource;
}