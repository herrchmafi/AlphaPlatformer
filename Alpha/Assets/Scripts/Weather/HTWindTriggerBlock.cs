using UnityEngine;
using System.Collections;

public class HTWindTriggerBlock : MonoBehaviour {
	public float speed;
	public Vector2 dirVect;
	public float secondsDuration;
	
	private HTTimer timer;
	// Use this for initialization
	void Start () {
		this.timer = new HTTimer();
		this.timer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		this.timer.Update();
		if (this.timer.Seconds >= this.secondsDuration) {
			Destroy(gameObject);
		}
		transform.Translate(this.dirVect * this.speed * Time.deltaTime);
	}
	
}
