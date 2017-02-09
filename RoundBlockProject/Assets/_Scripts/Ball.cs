using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public float speed = 20.0f;

	public float VEC_X = 5f;
	public float VEC_Z = 5f;

	public float magnitude = 20f;
	public float magnitude_max = 20f;

	Rigidbody rigid;

	// Use this for initialization
	void Start () {
		rigid = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			Shot();
		}
	}

	void Shot() {
		rigid.AddForce(
			(transform.forward + transform.right) * speed,
			ForceMode.VelocityChange);
	}

	void FixedUpdate() {
		checkVelocity();
	}

	void setMagnitude(Vector3 v) {
		v.Normalize();

		rigid.velocity = v * magnitude;
	}

	void checkVelocity() {
		Vector3 v = rigid.velocity;

		if (- VEC_X < v.x && v.x <= 0)	{
			v.x = - VEC_X;
		}
		else if (VEC_X > v.x && v.x >= 0) {
			v.x = VEC_X;
		}

		if (- VEC_Z < v.z && v.z <= 0){
			v.z = - VEC_Z;
		}
		else if (VEC_Z > v.z && v.z >= 0) {
			v.z = VEC_Z;
		}

		// 平均化
		setMagnitude(v);
	}
}
