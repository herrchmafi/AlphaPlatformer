using UnityEngine;
using System.Collections;

public class HTWindEmitter : MonoBehaviour {
	public Transform windBlockFab;
	
	public float windSpeed;
	public float windBlockDuration;
	
	public Vector2 dirVect;
	
	private float secondsToNextWindBlock;
	
	private HTTimer timer;
	// Use this for initialization
	void Start () {
		this.timer = new HTTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			this.StartEmisssionsPerMinute(60);
		} else if (Input.GetKeyDown("2")) {
			this.StartEmisssionsPerMinute(30);
		} else if (Input.GetKeyDown("3")) {
			this.Stop();
		}
		if (this.timer.IsTiming) {
			this.timer.Update();
			if (this.timer.Seconds >= this.secondsToNextWindBlock) {
				this.EmitWindBlock();
				this.timer.Reset();
			}
		} 
	}
	
	public void StartEmisssionsPerMinute(int count) {
		Transform windBlockTransform = (Transform)Instantiate(this.windBlockFab, transform.position, transform.rotation);
		windBlockTransform.gameObject.GetComponent<HTWindTriggerBlock>().Init(this.windSpeed, this.dirVect, this.windBlockDuration, transform.localScale.y);
		this.secondsToNextWindBlock = HTTimer.secondsPerMinute / count;
		this.timer.Start();
	}
	
	private void EmitWindBlock() {
		Transform windBlockTransform = (Transform)Instantiate(this.windBlockFab, transform.position, transform.rotation);
		windBlockTransform.gameObject.GetComponent<HTWindTriggerBlock>().Init(this.windSpeed, this.dirVect, this.windBlockDuration, transform.localScale.y);
	}
	
	public void Stop() {
		this.timer.Stop();	
	}
}
