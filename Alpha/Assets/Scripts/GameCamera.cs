using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	private Camera cam;
	
	public float dampTime;
	private Vector3 velocVect;
	
	private Transform target;
	private Vector3 offset;
	// Use this for initialization
	void Start () {
		this.cam = transform.GetComponent<Camera>();
	}
	
	void LateUpdate() {
		if (this.target) {
			Vector3 point = this.cam.WorldToViewportPoint(this.target.position);
			Vector3 delta = target.position - this.cam.ViewportToWorldPoint(new Vector3(.5f, .5f, point.z));
			Vector3 dest = transform.position + this.offset + delta;
			transform.position = Vector3.SmoothDamp(transform.position, dest, ref velocVect, this.dampTime);
		}
	}
	
	public void SetTarget (Transform targetTransform, Vector3 offset) {
		this.target = targetTransform;
		this.offset = offset;
	}
}
