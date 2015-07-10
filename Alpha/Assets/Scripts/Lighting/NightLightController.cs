using UnityEngine;
using System.Collections;

public class NightLightController : MonoBehaviour {
	public float maxRange = 3.0f;
	public float minRange = 0.0f;
	
	private float prevRange;
	private float targetRange;
	
	public float transitionTime = 3.0f;
	
	private Timer transitionTimer;
	
	private float transitionDec;
	
	private Animator animator;
	private Light light;
	
	// Use this for initialization
	void Start () {
		this.animator = transform.GetComponent<Animator>();
		this.light = transform.GetChild(0).GetComponent<Light>();
		this.transitionTimer = new Timer();
	}
	
	void Update() {
		if (Input.GetKeyDown("1")) {
			this.TurnOn();
		} else if (Input.GetKeyDown("2")) {
			this.TurnOff();
		}
		this.transitionTimer.Update();
		if (this.transitionTimer.IsTiming) {
			if (this.transitionTimer.Seconds < this.transitionTime) {
				this.transitionDec = this.transitionTimer.Seconds / this.transitionTime;
				this.light.range = Mathf.Lerp(this.prevRange, this.targetRange, this.transitionDec);
			} else {
				this.light.range = this.targetRange;
				this.transitionTimer.Stop();
			}
		}
	}
	
	public void TurnOn() {
		if (!this.transitionTimer.IsTiming && this.targetRange != this.maxRange) {
			this.transitionTimer.Start();
			this.targetRange = this.maxRange;
			this.prevRange = this.minRange;
		}
		this.animator.SetInteger("Switch", 1);
	}
	
	public void TurnOff() {
		if (!this.transitionTimer.IsTiming && this.targetRange != this.minRange) {
			this.transitionTimer.Start();
			this.targetRange = this.minRange;
			this.prevRange = this.maxRange;
		}
		this.animator.SetInteger("Switch", 0);
	}
}
