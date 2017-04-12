using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using System.Collections.Generic;

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
		DataCtrl dataCtrl = new DataCtrl ();

		dataCtrl.InitItemMasterData ();

		float timeNow = 11.4f;
		List<float> fl = dataCtrl.getRateList (timeNow);

		Assert.AreEqual (1, fl [0]);
		Assert.AreEqual (3, fl [1]);
		Assert.AreEqual (1, fl [2]);
		Assert.AreEqual (1, fl [3]);
		Assert.AreEqual (1, fl [4]);
		Assert.AreEqual (0, fl [5]);
		Assert.AreEqual (1, fl [6]);
		Assert.AreEqual (8, fl [7]);
//		11,1,3,1,1,1,0,1,8
	}

	[Test]
	public void LanguageLoadTest() {
		LanguageCtrl lc = new GameObject().AddComponent<LanguageCtrl>();
		lc.initLocalization ();

		string testMsg = lc.getMessageFromCode ("answer_01_y");
		Assert.AreEqual ("はい！", testMsg);
	}
}
