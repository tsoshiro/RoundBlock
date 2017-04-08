using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	Vector3 defaultPosition;
	BlockManager _blockManager;
	Item _item;

	Color defColor;

	// Use this for initialization
	void Start () {
		defaultPosition = transform.position;
		_blockManager = transform.parent.GetComponent<BlockManager>();
		_item = this.gameObject.AddComponent<Item> ();

		defColor = this.GetComponent<MeshRenderer>().material.color;
	}

	/// <summary>
	/// ItemTypeの内容に応じて、ブロックの色を変更する
	/// </summary>
	public void setBlockColor() {
		Const.ItemType itemType = _item.getItemType ();
		Color color = defColor;
		switch (itemType) {
		case Const.ItemType.ADD_BALL:
			color = Color.red;
			break;
		case Const.ItemType.HARD:
			color = Color.blue;
			break;
		case Const.ItemType.WIDER_RACKET:
			color = Color.cyan;
			break;
		case Const.ItemType.SHOT:
			color = Color.magenta;
			break;
		case Const.ItemType.SUPER_SHOT:
			color = Color.green;
			break;
		case Const.ItemType.FEVER:
			color = Color.black;
			break;
		default:
			color = defColor;
			break;
		}

		this.GetComponent<MeshRenderer> ().material.color = color;
	}


	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Ball") {
			Ball ball = collision.gameObject.GetComponent<Ball> ();
			HitBy (ball);
		}
	}

	public void HitBy(Ball pBall) {
		if (_item.getItemType() != Const.ItemType.NONE) { // itemならば
			_item.hit (pBall);
		}

		_blockManager.removeBlock(this);
	}

	public void HitBy(Bullet pBullet) {
		_blockManager.removeBlock(this);

//		if (_item.getItemType () != Const.ItemType.NONE) {
//			_item.hit (pBullet);
//		}
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

	public void setItemType(Const.ItemType pItemType) {
		_item.setItemType (pItemType);
	}

	public Const.ItemType getItemType() {
		return _item.getItemType ();
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
