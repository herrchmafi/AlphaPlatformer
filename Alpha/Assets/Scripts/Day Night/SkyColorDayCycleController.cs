﻿using UnityEngine;
using System.Collections;

//Name is kinda weird
public class SkyColorDayCycleController : MonoBehaviour {
	public Color[] colors;
	private Color currentColor;
	private Color targetColor;

	private Camera camera;
	
	private DayCycleController dayCycleController; 
	
	// Use this for initialization
	void Start () {
		this.camera = gameObject.GetComponent<Camera>();
		this.dayCycleController = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<DayCycleController>();
		this.currentColor = this.colors[this.dayCycleController.currentHour];
		this.targetColor = this.colors[this.dayCycleController.NextHour];
	}
	
	// Update is called once per frame
	void Update () {
		this.currentColor = this.colors[this.dayCycleController.currentHour];
		this.targetColor = this.colors[this.dayCycleController.NextHour];
		this.camera.backgroundColor = Color.Lerp(this.currentColor, this.targetColor, this.dayCycleController.HourTimeDec);
	}
}
