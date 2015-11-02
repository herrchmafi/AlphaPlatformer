using UnityEngine;
using System.Collections;

public class HTWindController : MonoBehaviour {
	public enum State {
		IDLE = 0,
		SLIGHT = 1,
		STRONG = 2,
		BLUSTERY = 3
	}
	private State currentState;
	public State CurrentState {
		get { return this.currentState; }
		set { this.currentState = value; }
	}	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
}
