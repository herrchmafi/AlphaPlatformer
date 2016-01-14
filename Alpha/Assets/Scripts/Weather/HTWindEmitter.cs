using UnityEngine;
using System.Collections;

public class HTWindEmitter : MonoBehaviour {
	public Transform windBlockFab;
	
	public float windSpeed;
	public float windBlockDuration;
	
	public int dir;
	
	private float secondsToNextWindBlock;
	
	private HTTimer timer;
	
	private float emitWaitSecondsMin, emitWaitSecondsMax;
	private bool isEmmisionsRandomized;
	// Use this for initialization
	void Start () {
		this.timer = new HTTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			this.StartEmissionsWithRandomWait(3.0f, 10.0f);
		} else if (Input.GetKeyDown("2")) {
			this.Stop();
		} else if (Input.GetKeyDown("3")) {
			this.Stop();
		}
		if (this.timer.IsTiming) {
			this.timer.Update();
			//If seconds elapsed passes wait seconds, emit
			if (this.timer.Seconds >= this.secondsToNextWindBlock) {
				this.EmitWindBlock();
				//If randomized, generate next wait time
				if (this.isEmmisionsRandomized) {
					this.secondsToNextWindBlock = Random.Range(this.emitWaitSecondsMin, this.emitWaitSecondsMax);
				}
				this.timer.Reset();
			}
		} 
	}
	
	//Constant wind emissions
	public void StartEmisssionsPerMinute(int count) {
		this.EmitWindBlock();
		this.secondsToNextWindBlock = HTTimer.secondsPerMinute / count;
		this.timer.Start();
	}
	
	//Used for randomized wind emissions between range
	public void StartEmissionsWithRandomWait(float minSeconds, float maxSeconds) {
		this.EmitWindBlock();
		if (minSeconds >= maxSeconds) {
			throw new System.ArgumentException("Invalid parameters", "minSeconds is greater or equal to maxSeconds");
		}
		this.emitWaitSecondsMin = minSeconds;
		this.emitWaitSecondsMax = maxSeconds;
		this.secondsToNextWindBlock = Random.Range(this.emitWaitSecondsMin, this.emitWaitSecondsMax);
		this.isEmmisionsRandomized = true;
		this.timer.Start();
	}

//	//For straight paths
//	public HTWindPath(int dir, float speed, float seconds, Vector2 angle)
//
//	//For sine paths
//	public HTWindPath(int dir, float speed, float seconds, float amplitude, float frequency)
//	
//	//For circular paths
//	public HTWindPath(int dir, float speed, float seconds, float changeDegrees) 

	
	private void EmitWindBlock() {
		Transform windBlockTransform = (Transform)Instantiate(this.windBlockFab, transform.position, transform.rotation);
		HTWindPath[] windPaths = new HTWindPath[] {
			new HTWindPath(this.dir, this.windSpeed, 5.0f, 100.0f, 20.0f),
			new HTWindPath(this.dir, this.windSpeed, 10.0f, 100.0f),
		};
		windBlockTransform.gameObject.GetComponent<HTWindPathController>().Init(windPaths, transform.localScale.y);
	}
	
	public void Stop() {
		this.timer.Stop();
		this.isEmmisionsRandomized = false;
		this.emitWaitSecondsMin = .0f;
		this.emitWaitSecondsMax = .0f;	
	}
}
