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
			this.transform.position = this.transform.position + Vector3.up * 100;
			_blockManager.removeBlock();
		}
	}

	public void setDefaultPosition() {
		this.transform.position = defaultPosition;
	}
}
