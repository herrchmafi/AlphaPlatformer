using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTDayCycleController : MonoBehaviour {
	//This order is always preserved
	public enum State {
		MORNING, AFTERNOON, EVENING, NIGHT
	}
	public State currentState;
	
	public int morningStart;
	public int afternoonStart;
	public int eveningStart;
	public int nightStart;
	
	public int hoursCount;
	
	public int currentHour;
	public int NextHour {
		get { return (this.currentHour + 1) % this.hoursCount; }
	}
	//Hour time factor will be used to translate an actual hour into an ingame hour
	public float hourTimeFactor;
	
	private float hourTimeDec;
	public float HourTimeDec {
		get { return this.hourTimeDec; }
	}
	
	private Dictionary<int, State> hourStates;
	private Dictionary<State, int> stateHours;
	
	private HTPercipitationController percipitationController;
	private bool didChangePercipitationState;
	
	private HTTimer timer;
	
	
	// Use this for initialization
	void Start () {
		this.hourStates = new Dictionary<int, State>();
		this.hourStates.Add(this.morningStart, State.MORNING);
		this.hourStates.Add(this.afternoonStart, State.AFTERNOON);
		this.hourStates.Add(this.eveningStart, State.EVENING);
		this.hourStates.Add(this.nightStart, State.NIGHT);
		
		//I need bijective dictionary, not sure if cleaner/more space efficient way
		this.stateHours = new Dictionary<State, int>();
		foreach(KeyValuePair<int, State> hourState in this.hourStates) {
			this.stateHours.Add(hourState.Value, hourState.Key);
		}
		
		this.timer = new HTTimer();
		
		this.percipitationController = GameObject.FindGameObjectWithTag("WeatherManager").transform.GetChild(0).GetComponent<HTPercipitationController>();
		
		this.timer.Start();
		
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		this.hourTimeDec = Mathf.Clamp01(this.timer.Hours * this.hourTimeFactor);
		//Reset and update after each control point is met
		if (this.hourTimeDec >= 1.0f)  {
			this.hourTimeDec = 0.0f;
			this.currentHour++;
			this.timer.Reset();
		}
		this.currentHour %= this.hoursCount;
		//Set/Reset all necessary day-related things here
		if (this.currentHour == 0 && !this.didChangePercipitationState) {
			int bucket = Random.Range(0 ,this.percipitationController.RNGBuckets.Length);
			this.percipitationController.SetMagnitudeFromBucket(bucket);
			this.didChangePercipitationState = true;
			print("Did Change with choice: " + this.percipitationController.magnitude);
		} else if (this.currentHour != 0 && this.didChangePercipitationState){
			this.didChangePercipitationState = false;
			print ("Stable");
		}
		State tempState;
		//Update current state
		if (this.hourStates.TryGetValue(this.currentHour, out tempState)) {
			this.currentState = tempState;
		}
	}
	
	//Sets state, will reset hours to starting hour 
	public void SetState(State targetState) {
		this.currentState = targetState;
		this.currentHour = this.stateHours[this.currentState];
	}
	
	//Sets hour and adjusts state
	public void SetHour(int targetHour) {
		this.currentHour = targetHour;
		State targetState;
		if (this.hourStates.TryGetValue(this.currentHour, out targetState)) {
			this.currentState = targetState;
		} else {
			for (int i = this.currentHour - 1; i >= 0; i--) {
				if(this.hourStates.TryGetValue(i, out targetState)) {
					this.currentState = targetState;
					break;
				}
			}
		}
	 } 
}
