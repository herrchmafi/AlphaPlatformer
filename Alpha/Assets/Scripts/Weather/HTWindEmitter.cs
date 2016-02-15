using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTWindEmitter : MonoBehaviour {
	public Transform windBlockFab;

	public int dir;

	//Shared
	public float minWindSpeed, maxWindSpeed;
	public float windBlockColliderYScale;
	public float windBlockScale;

	//Straight
	public float absoluteMaxAngle;

	//For wavy path 
	public float amplitude;
	public float frequency;

	//Straight and wavy
	public float minDurationSeconds, maxDurationSeconds;

	//For circular path
	public float radius;

	private HTTimer timer;
	
	public float emitWaitSecondsMin, emitWaitSecondsMax;
	private bool isEmmisionsRandomized;
	
	private float secondsToNextWindBlock;

	public HTWindPathHelper.WindPathTemplate template;

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
//	//For wavy paths
//	public HTWindPath(int dir, float speed, float seconds, float amplitude, float frequency)
//	
//	//For circular paths
//	public HTWindPath(int dir, float speed, float radius)

	
	private void EmitWindBlock() {
		Transform windBlockTransform = (Transform)Instantiate(this.windBlockFab, transform.position, transform.rotation);
		List<HTWindPath> windPaths = new List<HTWindPath>();
		if (this.minWindSpeed > this.maxWindSpeed) {
			throw new System.ArgumentException("Invalid parameters", "minWindSpeed is greater than maxWindSpeed");
		}
		float windSpeed = Random.Range(this.minWindSpeed, this.maxWindSpeed);

		if (this.minDurationSeconds > this.maxDurationSeconds) {
			throw new System.ArgumentException("Invalid parameters", "minDurationSeconds is greater than maxDurationSeconds");
		}
		List<HTWindPathHelper.WindPath> windTrack = new List<HTWindPathHelper.WindPath>();
		switch (this.template) {
		case HTWindPathHelper.WindPathTemplate.STRAIGHT:
			windPaths.Add(new HTWindPath(this.dir, windSpeed, 
			Random.Range(this.minDurationSeconds, this.maxDurationSeconds), new Vector2(0, Random.Range(-this.absoluteMaxAngle, this.absoluteMaxAngle))));
			windTrack = HTWindPathHelper.straight;
			break;
		case HTWindPathHelper.WindPathTemplate.WAVEY:
			windPaths.Add(new HTWindPath(this.dir, windSpeed, 
				Random.Range(this.minDurationSeconds, this.maxDurationSeconds), this.amplitude, this.frequency));
			windTrack = HTWindPathHelper.wavey;
			break;
		case HTWindPathHelper.WindPathTemplate.STRAIGHTWAVEY:
			windPaths.Add(new HTWindPath(this.dir, windSpeed, 
			Random.Range(this.minDurationSeconds, this.maxDurationSeconds), new Vector2(0, Random.Range(-this.absoluteMaxAngle, this.absoluteMaxAngle))));
			windTrack = HTWindPathHelper.straightWavey;
			break;
		case HTWindPathHelper.WindPathTemplate.WAVEYLOOP:
			windPaths.Add(new HTWindPath(this.dir, windSpeed, 
			Random.Range(this.minDurationSeconds, this.maxDurationSeconds), this.amplitude, this.frequency));
			windPaths.Add(new HTWindPath(this.dir, windSpeed, this.radius));
			windTrack = HTWindPathHelper.waveyLoop;
			break;
		default:
			//fuck you
			break;
		}
		windBlockTransform.gameObject.GetComponent<HTWindPathController>().Init(windTrack, windPaths, new Vector2(transform.position.x, 
			transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2)), this.windBlockScale, this.windBlockColliderYScale);
	}
	
	
	public void Stop() {
		this.timer.Stop();
		this.isEmmisionsRandomized = false;
		this.emitWaitSecondsMin = .0f;
		this.emitWaitSecondsMax = .0f;	
	}
}
