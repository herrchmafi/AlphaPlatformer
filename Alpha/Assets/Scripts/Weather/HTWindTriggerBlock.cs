using UnityEngine;
using System.Collections;

public class HTWindTriggerBlock : MonoBehaviour {
	private HTWindPath[] windPaths;
	private int windIndex;
	private HTTimer timer;
	
	public float fadeoutTime;
	
	// Use this for initializations
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer != null && this.timer.IsTiming) {
			this.timer.Update();
			HTWindPath path = this.windPaths[this.windIndex];
			if (this.timer.Seconds >= path.Seconds) {
				this.timer.Reset();
				this.windIndex++;
				if (this.windIndex >= this.windPaths.Length) {
					this.timer.Stop();
					return;
				}
				path = this.windPaths[this.windIndex];
			}
			transform.Translate(path.Translate(Time.deltaTime, this.timer.Seconds));
			transform.eulerAngles = path.EulerAngulate(this.timer.Seconds);
		}
	}
	
	public void Init (HTWindPath[] windPaths, float heightScale) {
		this.windPaths = windPaths;
		if (this.windPaths.Length == 0) { return; }
		this.windPaths = windPaths;
		transform.localScale = new Vector2(transform.localScale.x, heightScale);
		this.timer = new HTTimer();
		this.timer.Start();
	}
	
}
