using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {
	public RacketCtrl _racketCtrl;
	public Ball _ball;
	public BlockManager _blockManager;
	public GameObject _debugModePanel;

	public Text _labelSpeed;
	public Text _labelMargin;

	bool isDebugMode = false;

	void Start() {
		// ACTIVEになってたらOFFにする。
		if (this.gameObject.activeSelf ||
		    isDebugMode) {
			_debugModePanel.SetActive(false);
			isDebugMode = false;
		}
	}


	public void SwitchDebugMode() {
		bool pFlg = isDebugMode;
		_debugModePanel.SetActive(!pFlg);
		isDebugMode = !pFlg;

		if (isDebugMode) {
			this.GetComponent<GameManager>().pauseGame();
		} else {
			this.GetComponent<GameManager>().startGame();
		}
	}

	bool pIsVelocityFlg = false;
	public void setIsVelocity() {
		_racketCtrl.setIsVelocity(pIsVelocityFlg);
		pIsVelocityFlg = !pIsVelocityFlg;
	}

	// <summary>
	// Ball Speed
	// </summary>
	public void setBallSpeedFaster() {
		_ball.addMagnitudeValue(0.5f);
		setLabel(_labelSpeed, _ball.magnitude.ToString("F1"));
	}

	public void setBallSpeedSlower() {
		_ball.addMagnitudeValue(-0.5f);
		setLabel(_labelSpeed, _ball.magnitude.ToString("F1"));
	}


	void setLabel(Text pText, string str) {
		pText.text = str;
	}

	// Block Margine
	public void addMargin() {
		_blockManager.addMargineValue(0.2f);
		setLabel(_labelMargin, _blockManager.getMargineValue().ToString("F1"));
	}

	public void subMargin() {
		_blockManager.addMargineValue(-0.2f);
		setLabel(_labelMargin, _blockManager.getMargineValue().ToString("F1"));
	}

}
