using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour {
	public RacketCtrl _racketCtrl;
	public GameObject _debugModePanel;

	bool isDebugMode = false;

	public void SwitchDebugMode() {
		bool pFlg = isDebugMode;
		_debugModePanel.SetActive(!pFlg);
		isDebugMode = !pFlg;
	}

	bool pIsVelocityFlg = false;
	public void setIsVelocity() {
		_racketCtrl.setIsVelocity(pIsVelocityFlg);
		pIsVelocityFlg = !pIsVelocityFlg;
	}
}
