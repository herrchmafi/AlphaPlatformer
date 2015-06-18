using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
		GameObject camObject = GameObject.FindGameObjectWithTag("MainCamera");
		camObject.GetComponent<GameCamera>().SetTarget(playerObject.transform, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
