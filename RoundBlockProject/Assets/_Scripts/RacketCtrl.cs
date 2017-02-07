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

		GetTouchPosition();
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

	public GameObject _touchPositionMarker;

	public float speed = 20;
	public bool isVelocity = false;

	void GetTouchPosition() {
		if (Input.GetMouseButton(0)) {
			Debug.Log("mousePosition: "+Input.mousePosition);
			Vector3 screenPos = Input.mousePosition;
			screenPos.z = 7.5f; // オブジェクトのy座標固定位置をここで指定
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
			_touchPositionMarker.transform.position = worldPos;

			// MOVE VELOCITY
			if (isVelocity)
			{
				MoveWithVelocity(worldPos);
			}
			else {
				MoveWithTransform(worldPos);
			}
		}
	}

	float range_x = 5.0f;

	// Transformの方が適切な動きする。
	void MoveWithTransform(Vector3 worldPos) {
		range_x = 10 / this.transform.localScale.x;
		Vector3 target = new Vector3(worldPos.x, 0, this.transform.position.z);
		if (target.x <= -range_x)
		{
			target.x = -range_x;
		}
		else if (target.x >= range_x) {
			target.x = range_x;
		}

		this.transform.position = target;
	}

	void MoveWithVelocity(Vector3 worldPos) {
		Vector3 target = new Vector3(worldPos.x, 0, worldPos.z);
		float lastSqrMgr = Mathf.Infinity;
		Vector3 desiredVelocity = (target - transform.position).normalized * speed;

		float sqrMag = 0f;
		sqrMag = (target - transform.position).sqrMagnitude;
		if (sqrMag > lastSqrMgr)
		{
			desiredVelocity = Vector3.zero;
		}
		lastSqrMgr = sqrMag;
		this.GetComponent<Rigidbody>().velocity = desiredVelocity;

	}
}
