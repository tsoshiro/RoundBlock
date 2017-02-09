using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {
	List<Block> _blockList;
	int blockCount;

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
		
	}

	public void removeBlock() {
		blockCount--;
		if (blockCount == 0) {
			reset();
			blockCount = _blockList.Count;
		}
	}


	public void reset() {
		for (int i = 0; i < _blockList.Count; i++) {
			_blockList[i].setDefaultPosition();
		}
	}
}
