using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTWindPathController : MonoBehaviour {
	private List<HTWindPathHelper.WindPath> windTrack;
	private List<HTWindPath> windPaths;
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
			if (this.timer.Seconds >= path.SecondsDuration) {
				this.timer.Reset();
				this.windIndex++;
				if (this.windIndex >= this.windTrack.Count) {
					this.timer.Stop();
					//TODO: Use coroutine
					Destroy(gameObject);
					return;
				}
				switch(this.windTrack[this.windIndex]) {
				//	//For straight paths
//	public HTWindPath(int dir, float speed, float seconds, Vector2 angle)
//
//	//For wavy paths
//	public HTWindPath(int dir, float speed, float seconds, Vector2 angle, float amplitude, float frequency)
//	
//	//For circular paths
//	public HTWindPath(int dir, float speed, float radius)
				case HTWindPathHelper.WindPath.STRAIGHT:
					this.windPaths.Add(new HTWindPath(path.Dir, path.Speed, path.Seconds, path.CurrentEulerAngle));
					break;
				case HTWindPathHelper.WindPath.WAVE:
					this.windPaths.Add(new HTWindPath(path.Dir, path.Speed, path.Seconds, path.CurrentEulerAngle, 10.0f, 3.0f));
					break;
				case HTWindPathHelper.WindPath.LOOP:
//					this.windPa
					break;
				default:
					break;
				}
				path = this.windPaths[this.windIndex];
				path.Setup(transform.position);
			}
			transform.eulerAngles = path.EulerAngulate(Time.deltaTime);
			transform.position = path.Translate(transform.position, Time.deltaTime, transform.eulerAngles);
		}
	}
	//Use if durations are known
	public void Init (List<HTWindPathHelper.WindPath> windTrack, List<HTWindPath> windPaths, Vector3 position, float scale, float colliderYScale) {
		this.windPaths = windPaths;
		if (this.windPaths.Count == 0) {
			return;
		}
		this.windTrack = windTrack;
		this.windPaths = windPaths;
		this.windPaths[0].Setup(position);
		TrailRenderer trailRenderer = GetComponent<TrailRenderer>();
		trailRenderer.startWidth = trailRenderer.startWidth * scale;
		transform.localScale = scale * new Vector2(transform.localScale.x, transform.localScale.y);
		BoxCollider2D childCollider2D = transform.GetChild(0).GetComponent<BoxCollider2D>();
		childCollider2D.size = new Vector2(childCollider2D.size.x, childCollider2D.size.y * colliderYScale);
		this.timer = new HTTimer();
		this.timer.Start();
	}
}
