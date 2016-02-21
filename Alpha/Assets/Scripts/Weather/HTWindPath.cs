using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTWindPath {
	public static Vector2 forward = Vector2.right;

	private HTWindPathHelper.WindPath path;
	public HTWindPathHelper.WindPath WindPath {
		get { return this.path; }
	}
	
	private Vector2 targetAngle;

	private int dir;	
	public int Dir {
		get { return this.dir; }
	}	
	private float speed;
	public float Speed {
		get { return this.speed; }
	}
	private float seconds = .0f;
	public float Seconds {
		get { return this.seconds; }
	}

	private float secondsDuration;
	public float SecondsDuration {
		get { return this.secondsDuration; }
	}
	private Vector2 currentEulerAngle;
	public Vector2 CurrentEulerAngle {
		get { return this.currentEulerAngle; }
	}
	
	//Used for sine motion
	private float amplitude;
	
	private float frequency;
	public float Frequency {
		get { return this.frequency; }
	}
	
	//Used for loop motion
	private float changeDegrees;
	public float ChangeDegrees {
		get { return this.changeDegrees; }
	}

	private Vector2 initialEulerAngle = Vector2.zero;
	public Vector2 InitialEulerAngle {
		get { return this.initialEulerAngle; }
		set { this.initialEulerAngle = value; }
	}
	
	private Vector2 initialPos = Vector2.zero;
	public Vector2 InitialPos {
		get { return this.initialPos; }
	}

	public void Setup(Vector2 initialPos) {
		this.initialEulerAngle = initialEulerAngle;
		this.currentEulerAngle = this.initialEulerAngle;
		this.initialPos = initialPos;
		switch (this.path) {
		case HTWindPathHelper.WindPath.LOOP:
			float normalizedInitialDegrees = HTMathHelper.NormalizeAngle(this.initialEulerAngle.y);
			//We want something that resembles a circular motion no matter what the angle it's coming from
			float lowerAngleBound = HTMathHelper.NormalizeAngle(-30.0f);
			float totalRevolutionDegrees = (normalizedInitialDegrees >= lowerAngleBound) ?
				normalizedInitialDegrees - lowerAngleBound + HTMathConstants.degreesPerRevolution 
					: lowerAngleBound - normalizedInitialDegrees;
			this.secondsDuration = totalRevolutionDegrees / this.changeDegrees;
			break;
		}
	}
	
	//Will give final translation based off of current angle and movement
	//Sign is dependent on what the angle is
	public Vector3 Translate(Vector3 currentPos, float deltaSeconds, Vector2 eulerAngles) {
		Vector2 tempMovement = HTMathHelper.NormalizedVectFromRadians(HTMathHelper.DegreesToRadians(eulerAngles.y)) * this.speed * deltaSeconds;
		switch (this.path) {
		case HTWindPathHelper.WindPath.STRAIGHT:
			break;
		case HTWindPathHelper.WindPath.WAVE:
			Vector2 initialAngleMovement = HTMathHelper.NormalizedVectFromRadians(HTMathHelper.DegreesToRadians(this.initialEulerAngle.y)) * this.speed * deltaSeconds;
			tempMovement = initialAngleMovement + new Vector2(tempMovement.x, this.amplitude * tempMovement.y);
			break;
		case HTWindPathHelper.WindPath.LOOP:
			break;
		}
		return currentPos + new Vector3(this.dir * tempMovement.x, tempMovement.y, .0f);
	}
	
	//WTF Naming
	public Vector2 EulerAngulate(float deltaSeconds) {
		Vector2 angle = HTMathConstants.nullPoint;
		this.seconds += deltaSeconds;
		switch (this.path) {
		case HTWindPathHelper.WindPath.STRAIGHT:
			angle = this.targetAngle;
			break;
		case HTWindPathHelper.WindPath.WAVE:
			angle = new Vector2(.0f, Mathf.Cos(HTMathConstants.radiansPerRevolution * this.frequency * this.seconds));
			break;
		case HTWindPathHelper.WindPath.LOOP:
			angle = this.currentEulerAngle + new Vector2(.0f, this.changeDegrees * deltaSeconds);
			break;
		}
		this.currentEulerAngle = angle;
		return angle;
	}
	
	//For straight paths
	public HTWindPath(int dir, float speed, float seconds, Vector2 angle) {
		this.path = HTWindPathHelper.WindPath.STRAIGHT;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.targetAngle = angle;
	}
	
	//For wave paths
	//Amplitude is the height of the wave. Frequency is the number of periods completed per second
	public HTWindPath(int dir, float speed, float seconds, Vector2 angle, float amplitude, float frequency) {
		this.path = HTWindPathHelper.WindPath.WAVE;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.amplitude = amplitude;
		this.frequency = frequency;
		this.initialEulerAngle = angle;
	}
	
	//For circular paths
	public HTWindPath(int dir, float speed, float radius) {
		this.path = HTWindPathHelper.WindPath.LOOP;
		this.dir = dir;
		this.speed = speed;
		this.changeDegrees = HTMathConstants.degreesPerRevolution / radius;
	}
}
