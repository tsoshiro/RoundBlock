using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public float VEC_X = 5f;
	public float VEC_Z = 5f;

	public float magnitude = 20f;

	public float angleAdjustValue = 100f;

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
			(transform.forward + transform.right) * magnitude,
			ForceMode.VelocityChange);
	}

	void FixedUpdate() {
		//checkVelocity();
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

	void HitRacket(GameObject pGameObject) {
		float distanceFromCenter = 0;
		float velocityValue = 0;

		Vector3 v;
		float width = pGameObject.transform.localScale.x;

		if (pGameObject.name == "Racket" || pGameObject.name == "Racket_Top")
		{
			// HORIZONTAL
			distanceFromCenter = this.transform.position.x - pGameObject.transform.position.x;

			velocityValue = distanceFromCenter * width;

			v = new Vector3(velocityValue, rigid.velocity.y, rigid.velocity.z);
		} else {
			// VERTICAL
			distanceFromCenter = this.transform.position.z - pGameObject.transform.position.z;

			velocityValue = distanceFromCenter * width;

			v = new Vector3(rigid.velocity.x, rigid.velocity.y, velocityValue);
		}

		// 平均化
		setMagnitude(v);
	}

	void OnCollisionEnter(Collision pCollision) {
		if (pCollision.gameObject.tag == "Racket") {
			HitRacket(pCollision.gameObject);
		}
	}
}
