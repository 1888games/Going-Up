﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InAudioSystem;

public class SoundController : MonoBehaviourSingleton<SoundController> {

	public InAudioNode music;

	public InAudioNode complaints;

	public float complainTimer = 0f;

	public InAudioNode ding;
	public InAudioNode doors;
	
	
	// Use this for initialization
	void Start () {

		Invoke ("StartMusic", 0.1f);
		
	}

	void StartMusic () {

		InAudio.Play (this.gameObject, music);
	}
	
	// Update is called once per frame
	void Update () {

		complainTimer += Time.deltaTime;
		
	}

	public void Complain () {

		if (complainTimer > 2f) {

			InAudio.Play (this.gameObject, complaints);
			complainTimer = 0f;

		}

	}

	public void Ding () {

		InAudio.Play (this.gameObject, ding);

	}
	
	public void Doors () {

		InAudio.Play (this.gameObject, doors);

	}
}
