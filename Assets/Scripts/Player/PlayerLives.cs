using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

	public int lives = 3;

	
	// Update is called once per frame
	void Update () {
		if (lives <= 0) {
			die();
		}
	}

	void die() {
	}
}
