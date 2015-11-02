using UnityEngine;
using System.Collections;

public class HTSunLightController : MonoBehaviour {

	//Note: Intensity size MUST be the same size as the amount of hours
	//Set in inspector and make sure it lines up with day sequences to make sense
	public float[] intensities;

	private Light sunLight;
	
	private HTDayCycleController dayCycleController;
	// Use this for initialization
	void Start () {
		this.sunLight = transform.GetComponent<Light>();
		this.dayCycleController = GameObject.FindGameObjectWithTag("DayNightManager").GetComponent<HTDayCycleController>();;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (intensities.Length > 0) {
			float currentIntensity = this.intensities[this.dayCycleController.currentHour];
			float targetIntensity = this.intensities[this.dayCycleController.NextHour];
			this.sunLight.intensity = Mathf.Lerp(currentIntensity, targetIntensity, this.dayCycleController.HourTimeDec);
		}
	}
}
