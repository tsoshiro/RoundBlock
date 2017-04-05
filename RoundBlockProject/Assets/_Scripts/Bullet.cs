using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	Vector3 direction;
	float magnitude;
	GameObject parentRacket;
	Rigidbody rigid;

	bool isHard = false;

	GameManager _gameManager;
	// Shot元
	public ShotCtrl _shotCtrl;

	void Awake () {
		rigid = this.gameObject.GetComponent<Rigidbody> ();
	}

	public void Init(Vector3 pDirection, float pMagnitude, GameObject pParentRacket) {
		direction = pDirection;
		magnitude = pMagnitude;
		parentRacket = pParentRacket;

		Move ();
	}

	// Update is called once per frame
	void Update () {
		if (!checkInArea ()) {
			finishSelf ();
		}
	}

	// 進む
	void Move() {
		Vector3 v = direction;
		v.Normalize();
		rigid.velocity = v * magnitude;
	}

	bool checkInArea() {
		return true;
	}

	void OnTriggerEnter(Collider pCollider) {
		if (pCollider.gameObject.tag == "Block") {
			pCollider.gameObject.GetComponent<Block> ().HitBy(this);

			if (!isHard) {
				finishSelf ();
			}
		}
	}

	// 自分を	InActiveにし、Poolに戻る
	void finishSelf() {
		rigid.velocity = Vector3.zero;
		_shotCtrl.inactivate (this);
	}
}
