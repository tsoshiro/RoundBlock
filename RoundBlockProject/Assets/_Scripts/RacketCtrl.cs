using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketCtrl : MonoBehaviour {
	public float accel = 1000.0f;
	Vector3 inputVector;
	Rigidbody rig;

	// Use this for initialization
	void Start () {
		rig = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		GetInputVector();
	}

	void FixedUpdate() {
		Move();
	}

	void GetInputVector() {
		inputVector = transform.right * Input.GetAxisRaw("Horizontal") * accel;
	}

	void Move() {
		rig.AddForce(inputVector, ForceMode.Impulse);
	}
}
