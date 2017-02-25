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

		defColor = this.GetComponent<MeshRenderer>().material.color;
	}

	public void setItem() {
		if (this.GetComponent<Item>())
		{
			_item = this.GetComponent<Item>();
			this.GetComponent<MeshRenderer>().material.color = Color.red;
		}
		else {
			this.GetComponent<MeshRenderer>().material.color = defColor;
		}
	}


	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Ball") {
			//this.transform.position = this.transform.position + Vector3.up * 100;
			if (checkIsBallHard(collision.gameObject.GetComponent<Ball>())) {
				Debug.Log("IS HARD!");
				removeColliderMaterial();
			}

			if (_item != null) { // itemならば
				_item.hit(collision.gameObject.GetComponent<Ball>());
			}

			_blockManager.removeBlock(this);
		}
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
