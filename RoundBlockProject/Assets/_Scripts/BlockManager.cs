using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {
	List<Block> _blockList;
	int blockCount;
	RacketCtrl _racketCtrl;

	float edgeTop, edgeRight, edgeBottom, edgeLeft;
	float MARGINE = 5f;


	// Use this for initialization
	void Start () {
		setEdges();
		_blockList = new List<Block>();
		blockCount = this.transform.childCount;
		if (blockCount > 0) {
			for (int i = 0; i < blockCount; i++)
			{
				Block block = transform.GetChild(i).GetComponent<Block>();
				_blockList.Add(block);
			}
		}
	}

	void setEdges() {
		_racketCtrl = GameObject.Find("Racket").GetComponent<RacketCtrl>();
		edgeTop	   	= _racketCtrl.otherRackets[0].transform.position.z - MARGINE;
		edgeLeft	= _racketCtrl.otherRackets[1].transform.position.x + MARGINE;
		edgeRight 	= _racketCtrl.otherRackets[2].transform.position.x - MARGINE;
		edgeBottom 	= _racketCtrl.gameObject.transform.position.x + MARGINE;
}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void removeBlock(Block pBlock) {
		Vector3 v = new Vector3((float)Random.Range(edgeLeft, edgeRight), 0, (float)Random.Range(edgeBottom, edgeTop));
		pBlock.transform.position = v;

		//blockCount--;
		//if (blockCount == 0) {
		//	reset();
		//	blockCount = _blockList.Count;
		//}
	}


	public void reset() {
		for (int i = 0; i < _blockList.Count; i++) {
			_blockList[i].setDefaultPosition();
		}
	}
}
