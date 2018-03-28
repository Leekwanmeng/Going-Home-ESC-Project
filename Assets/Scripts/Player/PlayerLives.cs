using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLives : MonoBehaviour {

	private int lives=3;
	private bool isDead;

	
	// Update is called once per frame
	void Update () {
		setIsDead (lives);
		if (isDead) {
			die();
		}
	}

	void die() {
	}


	//for testing purposes
	public void setLives(int i){
		lives = i;	
	}
	public int getLives(){
		return lives;
	}

	public void setIsDead(int lives){
		if (lives <= 0) {
			isDead = true;
		} else {
			isDead = false;
		}
	}

	public bool getIsDead(){
		return isDead;
	}

	
}
