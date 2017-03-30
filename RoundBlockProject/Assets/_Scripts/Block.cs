using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	Vector3 defaultPosition;
	BlockManager _blockManager;
	Item _item;
	bool isItem;

	Color defColor;

	// Use this for initialization
	void Start () {
		defaultPosition = transform.position;
		_blockManager = transform.parent.GetComponent<BlockManager>();
		_item = this.gameObject.AddComponent<Item> ();

		defColor = this.GetComponent<MeshRenderer>().material.color;
	}

	public void setBlockColor() {
		if (isItem) {
			this.GetComponent<MeshRenderer> ().material.color = Color.red;
		} else {
			this.GetComponent<MeshRenderer> ().material.color = defColor;
		}
	}


	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Ball") {
			Ball ball = collision.gameObject.GetComponent<Ball> ();
			HitByBall (ball);
		}
	}

	public void HitByBall(Ball pBall) {
		if (isItem) { // itemならば
			_item.hit (pBall);
		}

		_blockManager.removeBlock(this);
	}

	bool checkIsBallHard(Ball pBall) {
		if (pBall.myBallState == Ball.BallState.HARD) {
			return true;	
		}
		return false;
	}

	void removeColliderMaterial() {
		//this.GetComponent<Collider>().material = null;
	}

	public void setDefaultPosition() {
		this.transform.position = defaultPosition;
	}

	public void setIsItem(bool pFlg) {
		isItem = pFlg;
	}

	public bool getIsItem() {
		return isItem;
	}


	#region ANIMATION
	void playAnimation(GameObject pFxObj)
	{
		Vector3 pos = this.transform.position;
		pos.y -= 0.5f;
		pFxObj.transform.position = pos;

		// 演出再生
		pFxObj.SetActive(true);
	}

	public IEnumerator animationCoroutine(GameObject pFxObj, int pCounter)
	{
		playAnimation(pFxObj);

		yield return new WaitForSeconds(3f);

		_blockManager.fxReset(pCounter);
	}
	#endregion
}
