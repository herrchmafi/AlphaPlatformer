using UnityEngine;
using System.Collections;

public class HTWindTriggerBlock : MonoBehaviour {
	private float speed;
	private Vector2 dirVect;
	private float secondsDuration;
	
	private HTTimer timer;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer.IsTiming) {
			this.timer.Update();
			if (this.timer.Seconds >= this.secondsDuration) {
				Destroy(gameObject);
			}
			transform.Translate(this.dirVect * this.speed * Time.deltaTime);
		}
	}
	
	public void Init (float speed, Vector2 dirVect, float secondsDuration, float heightScale) {
		this.speed = speed;
		this.dirVect = dirVect;
		this.secondsDuration = secondsDuration;
		transform.localScale = new Vector2(transform.localScale.x, heightScale);
		this.timer = new HTTimer();
		this.timer.Start();
	}
	
}
