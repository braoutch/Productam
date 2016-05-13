using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Engine : MonoBehaviour {

	//Temps de départ de chaque état
	float initTimeStart = 0;
	float initTimeStop = 0;

	//Temps totaux de chaque états
	float totalTimeStart = 0;
	float totalTimeStop = 0;

	//Temps de pause
	float initPause = 0;
	float pausedTime = 0;

	//Heure de départ
	float startingTime = 0;


	int status = -1;
	int oldStatus = -1;

	public Text ratio;
	public Text bandeauBas;
	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 15;
	}
	
	// Update is called once per frame
	void Update () {
		float ratioToDisplay = 0;

		if (status == 0)
			ratioToDisplay = (totalTimeStart / (totalTimeStart + totalTimeStop + (Time.time - initTimeStop)));
				
		if (status == 1)
			ratioToDisplay = ((totalTimeStart + (Time.time - initTimeStart)) / (totalTimeStart + totalTimeStop + (Time.time - initTimeStart)));	

		if(status != -2 && !float.IsNaN(ratioToDisplay*100))
		ratio.text = Math.Round (ratioToDisplay*100, 2).ToString("F2") + " %";

		if(status != -2)
		bandeauBas.text = CalculTotal();
	}

	String CalculTotal()
	{
		int heures, minutes, secondes, temps;
		temps = (int)(Time.time - startingTime - pausedTime);
		minutes = temps / 60;
		secondes = temps - 60 * minutes;
		heures = minutes / 60;
		minutes = minutes - heures * 60;
		if(status != -1)
			return ("Temps passé total : " + heures.ToString () + " heures, " + minutes.ToString () + " minutes, " + secondes.ToString () + " secondes, et sans les pauses.");
		else
			return("En attente...");
	}

	public void StartButton()
	{
		if (status == 0 || status == -1) {
			initTimeStart = Time.time;
			totalTimeStop += Mathf.Abs (Time.time - initTimeStop);
			if (status == -1) {
				startingTime = Time.time;
				totalTimeStop = 0;
			}
			status = 1;
		}
	}

	public void StopButton()
	{
		if (status == 1) {
			initTimeStop = Time.time;
			totalTimeStart += Mathf.Abs (Time.time - initTimeStart);
			status = 0;
		}
	}

	public void RestartButton()
	{
		totalTimeStart = 0;
		totalTimeStop = 0;
		initTimeStart = 0;
		initTimeStop = 0;
		status = -1;
	}

	public void Pause()
	{
		if (status == 0 || status == 1) {
			oldStatus = status;
			status = -2;
			initPause = Time.time;


		} 
		else {
			status = oldStatus;
			pausedTime += Time.time - initPause;
			if (status == 0)
				initTimeStop += (Time.time - initPause);
			if (status == 1)
				initTimeStart += (Time.time - initPause);
			initPause = 0;
		}

	}


}
