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
	
	private Vector3 angle;
	public Vector3 Angle {
		get { return this.angle; }
	}

	private int dir;		
	private float speed;
	private float secondsDuration;
	public float Seconds {
		get { return this.secondsDuration; }
	}
	
	//Used for sine motion
	private float amplitude;
	private float frequency;
	
	//Used for loop motion
	
	private Vector2 initialPoint = HTMathConstants.nullPoint;
	public Vector2 InitialPoint {
		get { return this.initialPoint; }
		set { this.initialPoint = value; }
	}
	private float radius;
	private bool isBottom;
	//This will be calculated based off current position
	
	public Vector2 Translate(float deltaSeconds, float seconds) {
		switch (this.path) {
			case HTWindPath.WindPath.STRAIGHT:
				return (HTWindPath.forward * this.dir * this.speed * deltaSeconds);
			case HTWindPath.WindPath.SINE:
				return new Vector2(this.dir * this.speed * deltaSeconds, 
			                   this.amplitude * Mathf.Sin(this.frequency * HTMathConstants.radian * seconds) * deltaSeconds);
			case HTWindPath.WindPath.LOOP:
				if (this.initialPoint == HTMathConstants.nullPoint) { throw new System.InvalidOperationException("Did not set initial point for loop path"); }
				return HTMotionHelper.CircularMotion(this.isBottom, seconds / this.secondsDuration, this.radius, this.initialPoint);
		}
		return Vector3.zero;
	}
	
	//WTF Naming
	public Vector3 EulerAngulate(float seconds) {
		switch (this.path) {
		case HTWindPath.WindPath.STRAIGHT:
				return this.angle;
			case HTWindPath.WindPath.SINE:
				return Vector2.zero;
			case HTWindPath.WindPath.LOOP:
				return Vector2.zero;
		}
		return Vector3.zero;
	}
	
	//For straight paths
	public HTWindPath(int dir, float speed, float seconds, Vector3 angle) {
		this.path = WindPath.STRAIGHT;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.angle = angle;
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
	public HTWindPath(int dir, float seconds, float radius, bool isBottom) {
		this.path = WindPath.LOOP;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.radius = radius;
		this.isBottom = isBottom;
	}
	
	public HTWindPath(int dir, float seconds, float radius, bool isBottom, Vector2 initialPoint) {
		this.path = WindPath.LOOP;
		this.dir = dir;
		this.speed = speed;
		this.secondsDuration = seconds;
		this.radius = radius;
		this.isBottom = isBottom;
		this.initialPoint = initialPoint;
	}
}
