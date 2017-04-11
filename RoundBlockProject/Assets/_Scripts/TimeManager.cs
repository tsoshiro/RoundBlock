using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {
	public Text _timeLabel;

	float TIME = 15f;

	public bool isTimerOn = false;
	public float timer;

	public float timePassed = 0;

	// Use this for initialization
	void Start () {
		// 初期化
		timer = TIME;
		timePassed = 0;
		isTimerOn = false;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTimer();
	}

	void UpdateTimer() {
		if (!isTimerOn) {
			return;
		}

		timer -= Time.deltaTime;
		timePassed += Time.deltaTime;
		if (timer <= 0) {
			timer = 0;
			timePassed = 0;
			stopTimer();
		}

		updateTimeLabel();
	}

	public void startTimer() {
		if (isTimerOn) {
			return;
		}
		isTimerOn = true;
	}

	public void stopTimer() {
		if (!isTimerOn) {
			return;
		}
		isTimerOn = false;
	}

	public void addTime(float pTime) {
		timer += pTime;
	}

	public void resetTime() {
		timer = TIME;
	}

	public void updateTimeLabel() {
		_timeLabel.text = "TIME : " + timer.ToString("F1");
	}
}
