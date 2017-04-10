using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RateMethodTest {

	[Test]
	public void EditorTest() {
		//Arrange
		var gameObject = new GameObject();

		//Act
		//Try to rename the GameObject
		var newGameObjectName = "My game object";
		gameObject.name = newGameObjectName;

		//Assert
		//The object has a new name
		Assert.AreEqual(newGameObjectName, gameObject.name);
	}

	[Test]
	public void BlockRateTest() {
//		BlockManager _blockManager = new GameObject ("BlockManager", typeof(BlockManager)).GetComponent<BlockManager> ();
//
//		// サンプル入力
//		_blockManager.RATE_LIST.Add (10);
//		_blockManager.RATE_LIST.Add (20);
//		_blockManager.RATE_LIST.Add (30);
//		_blockManager.RATE_LIST.Add (40);
//		_blockManager.RATE_LIST.Add (50);
//		_blockManager.RATE_LIST.Add (60);


	}
}
