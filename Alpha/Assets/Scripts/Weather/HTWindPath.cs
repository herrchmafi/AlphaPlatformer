using UnityEngine;
using System.Collections;

public class HTWindPath {
	public static Vector2 forward = Vector2.right;
	public const float maxAngle = 30.0f;
	
	public enum WindPath {
		STRAIGHT, LOOP, SINE
	}
	private WindPath path;
	public WindPath Path {
		get { return this.path; }
	}
	
	private Vector2 targetAngle;

	private int dir;		
	private float speed;

	private float secondsDuration;
	public float SecondsDuration {
		get { return this.secondsDuration; }
	}

	private float seconds = 0.0f;
	private Vector2 currentEulerAngle;
	
	//Used for sine motion
	private float amplitude;
	private float frequency;
	
	//Used for loop motion
	private float changeDegrees;
	public float ChangeDegrees {
		get { return this.changeDegrees; }
	}

	private Vector2 initialEulerAngle = HTMathConstants.nullPoint;
	public Vector2 InitialEulerAngle {
		set { 
			this.initialEulerAngle = value;
			this.currentEulerAngle = this.initialEulerAngle; 
		}
	}

	public Vector3 Translate(Vector2 currentPos, float deltaSeconds, Vector2 eulerAngles) {
		Vector2 deltaMovement = this.dir * HTMathHelper.NormalizedVectFromRadians(HTMathHelper.DegreesToRadians(eulerAngles.y)) * this.speed * deltaSeconds;
		return deltaMovement;
	}
	
	//WTF Naming
	public Vector2 EulerAngulate(float deltaSeconds) {
		Vector2 angle = HTMathConstants.nullPoint;
		switch (this.path) {
			case HTWindPath.WindPath.STRAIGHT:
				angle = this.targetAngle;
				break;
			case HTWindPath.WindPath.SINE:
				angle = this.initialEulerAngle + new Vector2(.0f, this.amplitude * Mathf.Cos(this.frequency * HTMathConstants.radian * this.seconds));
				break;
			case HTWindPath.WindPath.LOOP:
				angle = this.currentEulerAngle + new Vector2(.0f, this.changeDegrees * deltaSeconds);
				break;
		}
		this.currentEulerAngle = angle;
		this.seconds += deltaSeconds;
		return angle;
	}
	
	//For straight paths
	public HTWindPath(int dir, float speed, float seconds, Vector2 angle) {
		this.path = WindPath.STRAIGHT;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.targetAngle = angle;
	}
	
	//For sine paths
	public HTWindPath(int dir, float speed, float seconds, float amplitude, float frequency) {
		this.path = WindPath.SINE;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.amplitude = amplitude;
		this.frequency = frequency;
	}
	
	//For circular paths
		public HTWindPath(int dir, float speed, float seconds, float changeDegrees) {
		this.path = WindPath.LOOP;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.changeDegrees = changeDegrees;
	}
}
