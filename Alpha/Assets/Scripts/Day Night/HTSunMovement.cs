using UnityEngine;
using System.Collections;

public class HTSunMovement : MonoBehaviour {
	public Vector3 pivotPoint;
	
	[Range(0, 100)]
	public int radius;
	
	[Range(0, 24)]
	public int hourOffset;
	
	private Vector3 initialPos;
	
	private HTDayCycleController dayCycleController;
	// Use this for initialization
	void Start () {
		this.dayCycleController = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<HTDayCycleController>();
		transform.position = this.pivotPoint - new Vector3(-this.radius, .0f, .0f);
	}
	
	// Update is called once per frame
	void Update () {
		float targetHour = (this.dayCycleController.currentHour + this.hourOffset) % this.dayCycleController.hoursCount;
		float thetaRadians = (targetHour + this.dayCycleController.HourTimeDec) / this.dayCycleController.hoursCount * HTMathConstants.radiansPerRevolution;
		float x = this.pivotPoint.x + -this.radius * Mathf.Cos(thetaRadians);
		float y = this.pivotPoint.y + this.radius * Mathf.Sin(thetaRadians);
		transform.position = new Vector2(x, y);
	}
	

}
