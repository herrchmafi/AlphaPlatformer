using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlatformController : RaycastController {

	public LayerMask passengerMask;

	public Vector3[] localWaypoints;
	Vector3[] globalWaypoints;

	public float speed;
	public bool isCyclic;
	public float waitTime;
	[Range(0,2)]
	public float easeAmount;

	private int fromWaypointIndex;
	private float decBetweenWaypoints;
	private float nextMoveTime;

	private List<PassengerMovement> passengerMovement;
	private Dictionary<Transform,Controller2D> passengerDictionary = new Dictionary<Transform, Controller2D>();
	
	struct PassengerMovement {
		public Transform transform;
		public Vector3 velocity;
		public bool isStandingOnPlatform;
		public bool isMoveBeforePlatform;
		
		public PassengerMovement(Transform _transform, Vector3 _velocity, bool _isStandingOnPlatform, bool _isMoveBeforePlatform) {
			transform = _transform;
			velocity = _velocity;
			isStandingOnPlatform = _isStandingOnPlatform;
			isMoveBeforePlatform = _isMoveBeforePlatform;
		}
	}
	
	public override void Start () {
		base.Start ();

		this.globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
		}
	}

	void Update () {

		UpdateRaycastOrigins ();

		Vector3 velocity = CalculatePlatformMovement();

		CalculatePassengerMovement(velocity);

		this.MovePassengers (true);
		transform.Translate (velocity);
		this.MovePassengers (false);
	}

	private float EaseInOut(float x) {
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
	}
	
	private Vector3 CalculatePlatformMovement() {

		if (Time.time < this.nextMoveTime) {
			return Vector3.zero;
		}
		//Calculate interpolation between points
		this.fromWaypointIndex %= this.globalWaypoints.Length;
		int toWaypointIndex = (this.fromWaypointIndex + 1) % this.globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance (this.globalWaypoints[this.fromWaypointIndex], this.globalWaypoints[toWaypointIndex]);
		this.decBetweenWaypoints += Time.deltaTime * this.speed / distanceBetweenWaypoints;
		this.decBetweenWaypoints = Mathf.Clamp01 (this.decBetweenWaypoints);
		float easedDecBetweenWaypoints = this.EaseInOut (this.decBetweenWaypoints);

		Vector3 newPos = Vector3.Lerp (this.globalWaypoints[this.fromWaypointIndex], this.globalWaypoints[toWaypointIndex], easedDecBetweenWaypoints);

		if (this.decBetweenWaypoints >= 1) {
			this.decBetweenWaypoints = 0;
			this.fromWaypointIndex++;

			if (!this.isCyclic) {
				if (this.fromWaypointIndex >= this.globalWaypoints.Length - 1) {
					this.fromWaypointIndex = 0;
					System.Array.Reverse(this.globalWaypoints);
				}
			}
			this.nextMoveTime = Time.time + this.waitTime;
		}

		return newPos - transform.position;
	}

	//Moves Passengers. Guarantees that each passenger is moved only once regardless of how many ray hits
	private void MovePassengers(bool isBeforeMovePlatform) {
		foreach (PassengerMovement passenger in this.passengerMovement) {
			if (!this.passengerDictionary.ContainsKey(passenger.transform)) {
				this.passengerDictionary.Add(passenger.transform, passenger.transform.GetComponent<Controller2D>());
			}
			if (passenger.isMoveBeforePlatform == isBeforeMovePlatform) {
				this.passengerDictionary[passenger.transform].Move(passenger.velocity, passenger.isStandingOnPlatform);
			}
		}
	}

	private void CalculatePassengerMovement(Vector3 velocity) {
		HashSet<Transform> movedPassengers = new HashSet<Transform> ();
		this.passengerMovement = new List<PassengerMovement> ();

		float dirX = Mathf.Sign (velocity.x);
		float dirY = Mathf.Sign (velocity.y);

		// Vertically moving platform
		if (velocity.y != 0) {
			float rayLength = Mathf.Abs (velocity.y) + skinWidth;
			
			for (int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = (dirY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (this.verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * dirY, rayLength, this.passengerMask);

				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = (dirY == 1) ? velocity.x : 0;
						float pushY = velocity.y - (hit.distance - skinWidth) * dirY;

						this.passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY), dirY == 1, true));
					}
				}
			}
		}

		// Horizontally moving platform
		if (velocity.x != 0) {
			float rayLength = Mathf.Abs (velocity.x) + skinWidth;
			
			for (int i = 0; i < horizontalRayCount; i++) {
				Vector2 rayOrigin = (dirX == -1) ? this.raycastOrigins.bottomLeft : this.raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (this.horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, this.passengerMask);

				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x - (hit.distance - skinWidth) * dirX;
						float pushY = -skinWidth;
						
						passengerMovement.Add(new PassengerMovement(hit.transform, new Vector3(pushX,pushY), false, true));
					}
				}
			}
		}

		// Passenger on top of a horizontally or downward moving platform
		if (dirY == -1 || velocity.y == 0 && velocity.x != 0) {
			float rayLength = skinWidth * 2;
			
			for (int i = 0; i < verticalRayCount; i ++) {
				Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);
				
				if (hit) {
					if (!movedPassengers.Contains(hit.transform)) {
						movedPassengers.Add(hit.transform);
						float pushX = velocity.x;
						float pushY = velocity.y;
						
						passengerMovement.Add(new PassengerMovement(hit.transform,new Vector3(pushX,pushY), true, false));
					}
				}
			}
		}
	}


	void OnDrawGizmos() {
		if (localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = .3f;

			for (int i =0; i < localWaypoints.Length; i ++) {
				Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
			}
		}
	}
	
}
