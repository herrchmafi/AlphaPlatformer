using UnityEngine;
using System.Collections;

public class HTWindPathController : MonoBehaviour {
	private HTWindPath[] windPaths;
	private int windIndex;
	private HTTimer timer;
	
	public float fadeoutTime;
	
	private Vector2 auxEulerVect;
	
	// Use this for initializations
	void Start () {
		this.auxEulerVect = transform.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.timer != null && this.timer.IsTiming) {
			this.timer.Update();
			HTWindPath path = this.windPaths[this.windIndex];
			if (this.timer.Seconds >= path.SecondsDuration) {
				this.timer.Reset();
				this.windIndex++;
				if (this.windIndex >= this.windPaths.Length) {
					this.timer.Stop();
					//TODO: Use coroutine
					Destroy(gameObject);
					return;
				}
				path = this.windPaths[this.windIndex];
				path.InitialEulerAngle = transform.eulerAngles;
			}
			this.auxEulerVect = path.EulerAngulate(Time.deltaTime);
			transform.Translate(path.Translate(transform.position, Time.deltaTime, this.auxEulerVect));
		}
	}

	public void Init (HTWindPath[] windPaths, float heightScale) {
		this.windPaths = windPaths;
		if (this.windPaths.Length == 0) {
			return;
		}
		this.windPaths = windPaths;
		this.windPaths[0].InitialEulerAngle = transform.eulerAngles;
		transform.localScale = new Vector2(transform.localScale.x, heightScale);
		this.timer = new HTTimer();
		this.timer.Start();
	}
	
}
