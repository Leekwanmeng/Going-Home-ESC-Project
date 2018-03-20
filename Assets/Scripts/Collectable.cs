using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	public bool interacted = false;
	public bool triggered = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (triggered && Input.GetButtonDown("Fire1")) {
			interacted = true;
		}
		if (interacted) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			triggered = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.CompareTag("Player")) {
			triggered = false;
			interacted = false;
		}
	}
}
