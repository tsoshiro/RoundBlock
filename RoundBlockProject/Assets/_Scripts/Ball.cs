using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public float speed = 20.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Shot();
		}
	}

	void Shot() {
		this.GetComponent<Rigidbody>().AddForce(
			(transform.forward + transform.right) * speed,
			ForceMode.VelocityChange);
	}
}
