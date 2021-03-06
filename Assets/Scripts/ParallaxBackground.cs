﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour {

	public Transform[] backgrounds;				// Array of backgrounds to be parallaxed
	public float smoothing = 1f;				// Magnitude of parallax

	private Transform cam;
	private float[] parallaxScaleArray;			// Proportion of camera movement
	private Vector3 prevCamPos;


	void Awake() {
		cam = Camera.main.transform;
	}

	// Use this for initialization
	void Start () {
		// Set previous frame to current cam.position
		prevCamPos = cam.position;

		// Assign parallaxScales to relative z-position of backgrounds
		parallaxScaleArray = new float[backgrounds.Length];
		for (int i=0; i<backgrounds.Length; i++) {
			parallaxScaleArray[i] = backgrounds[i].position.z*-1;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i=0; i<backgrounds.Length; i++) {
			float parallax = (prevCamPos.x - cam.position.x) * parallaxScaleArray[i];
			float backgroundTarget = backgrounds[i].position.x + parallax;
			Vector3 backgroundTargetPos = new Vector3(backgroundTarget, backgrounds[i].position.y, backgrounds[i].position.z);

			// Interpolates between backgrounds[i].position & backgroundTargetPos 
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		prevCamPos = cam.position;
	}
}
