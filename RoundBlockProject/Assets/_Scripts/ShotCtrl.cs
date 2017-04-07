using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotCtrl : MonoBehaviour {
	GameManager _gameManager;
	List<Bullet> bullets = new List<Bullet>();
	int bulletNum = 50;
	int counter = 0;

	float interval = 5f;
	public bool isActive = false;

	public GameObject _bulletPrefab;
	public GameObject _bulletPool;


	public float magnitude = 10f;

	// Use this for initialization
	void Start () {
		_gameManager = this.GetComponent<GameManager> ();
		createBullets ();
		StartCoroutine (shootLoop ());
	}

	void createBullets() {
		for (int i = 0; i < bulletNum; i++) {
			GameObject go = (GameObject)Instantiate(_bulletPrefab,
													_bulletPrefab.transform.position,
													_bulletPrefab.transform.rotation);
			go.name = "BULLET_" + i.ToString ("D2");
			go.transform.parent = _bulletPool.transform;
			Bullet bullet = go.GetComponent<Bullet> ();
			bullet._shotCtrl = this;
			bullets.Add (bullet);
			go.SetActive (false);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	// タイマー処理
	void activate() {
		
	}

	IEnumerator shootLoop() {
		while (isActive) {
//			loopShot ();
			yield return StartCoroutine(loopShotCoroutine());
			yield return new WaitForSeconds (interval);
		}
	}

	IEnumerator loopShotCoroutine() {
		for (int i = 0; i < _gameManager._racket._rackets.Count; i++) {
			shoot((RacketCtrl.RacketPosition)i);
			yield return new WaitForSeconds (0.5f);
		}
	}

	void loopShot() {
		for (int i = 0; i < _gameManager._racket._rackets.Count; i++) {
			shoot((RacketCtrl.RacketPosition)i);
		}
	}

	Vector3 getDirection(RacketCtrl.RacketPosition pRacketPosition) {
		Vector3 v = Vector3.forward;
		switch (pRacketPosition) {
		case RacketCtrl.RacketPosition.BOTTOM:
			v = Vector3.forward;
			break;
		case RacketCtrl.RacketPosition.TOP:
			v = Vector3.back;
			break;
		case RacketCtrl.RacketPosition.LEFT:
			v = Vector3.right;
			break;
		case RacketCtrl.RacketPosition.RIGHT:
			v = Vector3.left;
			break;
		}
		return v;
	}

	void shoot(RacketCtrl.RacketPosition pRacketPosition) {
		// DirectionとMagnitudeを設定し、発射元から発射
		Vector3 direction = getDirection(pRacketPosition);
		bullets [counter].gameObject.SetActive (true);
		bullets [counter].transform.position = _gameManager._racket._rackets[(int)pRacketPosition].gameObject.transform.position;
		bullets [counter].setRacketPosition (pRacketPosition);

		// 角度管理
		if (pRacketPosition == RacketCtrl.RacketPosition.LEFT ||
//		) {
//			bullets [counter].transform.Rotate (0, -90, 0);
//		} else if (
			pRacketPosition == RacketCtrl.RacketPosition.RIGHT) {
			bullets [counter].transform.Rotate (0, 90, 0);
		}

		bullets [counter].Init (direction, magnitude, _gameManager._racket.gameObject);

		counter++;
		if (counter >= bulletNum) {
			counter = 0;
		}
	}

	public void inactivate(Bullet pBullet) {
		pBullet.transform.position = _bulletPool.transform.position;

		// 元に戻す
		if (pBullet.getRacketPosition () == RacketCtrl.RacketPosition.LEFT ||
		    pBullet.getRacketPosition () == RacketCtrl.RacketPosition.RIGHT) 
		{
			pBullet.transform.Rotate (0, -90, 0);
		}

		pBullet.gameObject.SetActive (false);
	}
}