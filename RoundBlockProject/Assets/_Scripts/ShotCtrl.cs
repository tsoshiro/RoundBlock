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

	void activate() {
		
	}

	IEnumerator shootLoop() {
		while (isActive) {
			shoot ();
			yield return new WaitForSeconds (interval);
		}
	}

	void shoot() {
		// DirectionとMagnitudeを設定し、発射元から発射
		Vector3 direction = Vector3.forward;
		bullets [counter].gameObject.SetActive (true);
		bullets [counter].transform.position = _gameManager._racket.gameObject.transform.position;
		bullets [counter].Init (direction, magnitude, _gameManager._racket.gameObject);

		counter++;
		if (counter >= bulletNum) {
			counter = 0;
		}
	}

	public void inactivate(Bullet pBullet) {
		pBullet.transform.position = _bulletPool.transform.position;
		pBullet.gameObject.SetActive (false);
	}
}