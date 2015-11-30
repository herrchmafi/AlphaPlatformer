using UnityEngine;
using System.Collections;

public class HTGlobalWindController : MonoBehaviour {
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
	
	private HTGrassController[] grassControllers;	
	// Use this for initialization
	void Start () {
		GameObject[] tempGrassObjects = GameObject.FindGameObjectsWithTag(HTSceneConstants.GrassTag);
		this.grassControllers = new HTGrassController[tempGrassObjects.Length];
		for (int i = 0; i < tempGrassObjects.Length; i++) {
			this.grassControllers[i] = tempGrassObjects[i].GetComponent<HTGrassController>();
		}
	}
	
	void Update() {

	}
	
	public void SetSyncWindState(int state) {
		foreach (HTGrassController grassController in this.grassControllers) {
			grassController.SetWindState(state);
		}
	}
	
	public void SetAsynchWindState(int state) {
		foreach (HTGrassController grassController in this.grassControllers) {
			grassController.SetWindStateAsynch(state, Random.Range(0, 10) * .1f);
		}
	}
}
