using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	Vector3 direction;
	float magnitude;
	GameObject parentRacket;
	Rigidbody rigid;
	RacketCtrl.RacketPosition racketPosition;

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

	float DIST_SETTING = 40;

	/// <summary>
	/// Checks the in area.
	/// </summary>
	/// <returns><c>true</c>, if in area was checked, <c>false</c> otherwise.</returns>
	bool checkInArea() {
		// 仮処理 TODO Wallsからの距離を測る。Ballクラスの処理を外部に切り出して流用したい。
		float dist = Vector3.Distance (this.transform.position, parentRacket.transform.position);
		if (Mathf.Abs(dist) <= DIST_SETTING) {		
			return true;
		}
		return false;
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
		_shotCtrl.inactivateBullet (this);
	}

	public void setRacketPosition(RacketCtrl.RacketPosition pRacketPosition) {
		racketPosition = pRacketPosition;
	}

	public RacketCtrl.RacketPosition getRacketPosition() {
		return racketPosition;
	}

	public void setHard(bool pFlg) {
		isHard = pFlg;
	}
}