using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketCtrl : MonoBehaviour {
	public float accel = 1000.0f;

	// Top, Left, Rightのラケット 
	public List<Rigidbody> otherRackets;

	enum OtherRacketPosition {
		TOP,
		LEFT,
		RIGHT
	}

	Vector3 inputVector;
	Rigidbody rig;

	// Use this for initialization
	void Start () {
		rig = this.GetComponent<Rigidbody>();

		x_diff_horizontal = otherRackets[(int)OtherRacketPosition.RIGHT].transform.position.x - rig.transform.position.x;
		z_diff_horizontal = otherRackets[(int)OtherRacketPosition.RIGHT].transform.position.z - rig.transform.position.z;
		z_diff_vertical = otherRackets[(int)OtherRacketPosition.TOP].transform.position.z - rig.transform.position.z;
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
	public float Z_OFFSET = 7.5f;
	public bool isVelocity = false;

	void GetTouchPosition() {
		if (Input.GetMouseButton(0)) {
			Vector3 screenPos = Input.mousePosition;
			screenPos.z = Z_OFFSET; // オブジェクトのy座標固定位置をここで指定
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
	public float STAGE_WIDTH = 10f;
	public float STAGE_HEIGHT = 22f;
	float verticalRacketWidth;
	float horizontalRacketWidth;
	float x_diff_horizontal;
	float z_diff_horizontal;
	float z_diff_vertical;

	// Transformの方が適切な動きする。
	void MoveWithTransform(Vector3 worldPos) {
		range_x = STAGE_WIDTH / this.transform.localScale.x;
		Vector3 target = new Vector3(worldPos.x, 0, this.transform.position.z);
		if (target.x <= -range_x) {
			target.x = -range_x;
		} else if (target.x >= range_x) {
			target.x = range_x;
		}

		this.transform.position = target;

		MoveOtherRackets(target);
	}

	void MoveWithVelocity(Vector3 worldPos) {
		range_x = STAGE_WIDTH / this.transform.localScale.x;
		Vector3 target = new Vector3(worldPos.x, 0, worldPos.z);
		if (target.x <= -range_x) {
			target.x = -range_x;
		} else if (target.x >= range_x) {
			target.x = range_x;
		}

		float lastSqrMgr = Mathf.Infinity;
		Vector3 desiredVelocity = (target - transform.position).normalized * speed;

		float sqrMag = 0f;
		sqrMag = (target - transform.position).sqrMagnitude;
		if (sqrMag > lastSqrMgr) {
			desiredVelocity = Vector3.zero;
		}
		lastSqrMgr = sqrMag;
		this.GetComponent<Rigidbody>().velocity = desiredVelocity;

		MoveOtherRackets(desiredVelocity);
	}

	void MoveOtherRackets(Vector3 pVelocity) {
		for (int i = 0; i < otherRackets.Count; i++) {
			Vector3 v = exchangeVector(pVelocity, (OtherRacketPosition)i);
			if (isVelocity)
			{
				otherRackets[i].velocity = v;
			} else {
				otherRackets[i].transform.position = v;
			}
		}
	}

	Vector3 exchangeVector(Vector3 pVelocity, OtherRacketPosition pRacketPos) {
		Vector3 v = pVelocity;

		// 左右反転
		if (pRacketPos == OtherRacketPosition.TOP)
		{
			v.x = - v.x;
			v.z += z_diff_vertical;
		} 

		else if (pRacketPos == OtherRacketPosition.LEFT) {
			// XとZを交代
			float tmp = - (v.x - x_diff_horizontal) * STAGE_HEIGHT / STAGE_WIDTH;
			v.x = - x_diff_horizontal;
			v.z = v.z - z_diff_horizontal + tmp + STAGE_HEIGHT / 2;

		} else { // RIGHT
			float tmp = (v.x - x_diff_horizontal) * STAGE_HEIGHT / STAGE_WIDTH;
			v.x = x_diff_horizontal;
			v.z = z_diff_horizontal + tmp + STAGE_HEIGHT / 2;
		}

		return v;
	}
}