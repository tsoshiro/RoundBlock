using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	Vector3 defaultPosition;
	BlockManager _blockManager;

	// Use this for initialization
	void Start () {
		defaultPosition = transform.position;
		_blockManager = transform.parent.GetComponent<BlockManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag == "Ball") {
			//this.transform.position = this.transform.position + Vector3.up * 100;
			_blockManager.removeBlock(this);
		}
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
