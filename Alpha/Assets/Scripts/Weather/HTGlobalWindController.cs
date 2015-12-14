using UnityEngine;
using System.Collections;

public class HTGlobalWindController : MonoBehaviour {
	private HTWindStrength.WindStrength windStrength;
	public HTWindStrength.WindStrength WindStrength {
		get { return this.windStrength; }
		set { this.windStrength = value; }
	}
	
	private HTWindableController[] grassControllers;	
	// Use this for initialization
	void Start () {
		GameObject[] tempGrassObjects = GameObject.FindGameObjectsWithTag(HTSceneConstants.GrassTag);
		this.grassControllers = new HTWindableController[tempGrassObjects.Length];
		for (int i = 0; i < tempGrassObjects.Length; i++) {
			this.grassControllers[i] = tempGrassObjects[i].GetComponent<HTWindableController>();
		}
	}
	
	void Update() {

	}
	
	public void SetSyncWindState(int state) {
		foreach (HTWindableController grassController in this.grassControllers) {
			grassController.SetWindState(state);
		}
	}
	
	public void SetAsynchWindState(int state) {
		foreach (HTWindableController grassController in this.grassControllers) {
			grassController.SetWindStateAsynch(state, Random.Range(0, 10) * .1f);
		}
	}
}
