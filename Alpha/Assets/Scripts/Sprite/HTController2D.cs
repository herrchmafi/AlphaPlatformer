using UnityEngine;
using System.Collections;

public class HTController2D : HTRaycastController {
	
	public float maxClimbAngle;
	public float maxDescendAngle;
	
	private CollisionInfo collisionInfo;
	public CollisionInfo CollInfo {
		get { return this.collisionInfo; }
	}
	
	private Vector2 playerInput;
	public Vector2 PlayerInput {
		get { return this.playerInput; }
	}
	
	public struct CollisionInfo {
		public bool isAbove, isBelow;
		public bool isLeft, isRight;
		
		public bool climbingSlope;
		public bool descendingSlope;
		public bool isFallingThroughPlatform;
		public float slopeAngle, slopeAngleOld;
		//Left is -1, right is 1
		public int faceDir;
		public Vector3 prevDist;
		
		public void Reset() {
			isAbove = isBelow = false;
			isLeft = isRight = false;
			climbingSlope = false;
			descendingSlope = false;
			
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
	
	private void ResetIsFallingThroughPlatform() {
		this.collisionInfo.isFallingThroughPlatform = false;
	}
	
	public override void Start() {
		base.Start ();
		this.collisionInfo.faceDir = 1;
	}
	
	public void Move(Vector3 distVect, bool standingOnPlatform = false) {
		this.Move(distVect, Vector2.zero, standingOnPlatform);
	}
	
	public void Move(Vector3 distVect, Vector2 input, bool standingOnPlatform = false) {
		this.UpdateRaycastOrigins ();
		this.collisionInfo.Reset ();
		this.collisionInfo.prevDist = distVect;
		
		this.playerInput = input;
		
		if (distVect.x != 0) {
			this.collisionInfo.faceDir = (int)Mathf.Sign(distVect.x);
		}
		
		if (distVect.y < 0) {
			this.DescendSlope(ref distVect);
		}
		
		this.HorizontalCollisions (ref distVect);
		
		if (distVect.y != 0) {
			this.VerticalCollisions (ref distVect);
		}
		
		transform.Translate (distVect);
		if (standingOnPlatform) {
			this.collisionInfo.isBelow = true;
		}
	}
	
	private void HorizontalCollisions(ref Vector3 distVect) {
		float dirX = this.collisionInfo.faceDir;
		float rayLength = Mathf.Abs (distVect.x) + skinWidth;
		
		if (Mathf.Abs(distVect.x) < skinWidth) {
			rayLength = 2 * skinWidth;
		}
		
		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (dirX == -1) ? this.raycastOrigins.bottomLeft : this.raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.right * dirX * rayLength,Color.red);
			
			if (hit) {
				if (hit.distance == 0) {
					continue;
				}
				
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				//For ascending slopes
				if (i == 0 && slopeAngle <= maxClimbAngle) {
					if (this.collisionInfo.descendingSlope) {
						this.collisionInfo.descendingSlope = false;
						distVect = this.collisionInfo.prevDist;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != this.collisionInfo.slopeAngleOld) {
						distanceToSlopeStart = hit.distance - skinWidth;
						distVect.x -= distanceToSlopeStart * dirX;
					}
					ClimbSlope(ref distVect, slopeAngle);
					distVect.x += distanceToSlopeStart * dirX;
				}
				//If slope angle is too big
				if (!this.collisionInfo.climbingSlope || slopeAngle > maxClimbAngle) {
					distVect.x = (hit.distance - skinWidth) * dirX;
					rayLength = hit.distance;
					//Move up slope
					if (this.collisionInfo.climbingSlope) {
						distVect.y = Mathf.Tan(this.collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(distVect.x);
					}
					
					this.collisionInfo.isLeft = dirX == -1;
					this.collisionInfo.isRight = dirX == 1;
				}
			}
		}
	}
	
	void VerticalCollisions(ref Vector3 distVect) {
		float directionY = Mathf.Sign (distVect.y);
		float rayLength = Mathf.Abs (distVect.y) + skinWidth;
		
		for (int i = 0; i < this.verticalRayCount; i ++) {
			
			Vector2 rayOrigin = (directionY == -1) ? this.raycastOrigins.bottomLeft : this.raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (this.verticalRaySpacing * i + distVect.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);
			
			if (hit) {
				if (hit.collider.tag == "PassThrough") {
					if (directionY == 1 || hit.distance == 0) {
						continue;
					}
					if (this.collisionInfo.isFallingThroughPlatform) {
						continue;
					}
					//Allows for player to pass through from top of moving platform
					if (this.playerInput.y == -1) {
						this.collisionInfo.isFallingThroughPlatform = true;
						//Weirdness for helping allow for moving through moving platform
						//Downside is that you should not have moving platforms that you can move through have large y dimensions
						Invoke("ResetIsFallingThroughPlatform", .5f);
						continue;
					}
				}
				distVect.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;
				//Handles climbing slope displacement
				if (this.collisionInfo.climbingSlope) {
					distVect.x = distVect.y / Mathf.Tan(this.collisionInfo.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(distVect.x);
				}
				
				this.collisionInfo.isBelow = directionY == -1;
				this.collisionInfo.isAbove = directionY == 1;
			}
		}
		
		if (this.collisionInfo.climbingSlope) {
			float dirX = Mathf.Sign(distVect.x);
			rayLength = Mathf.Abs(distVect.x) + skinWidth;
			Vector2 rayOrigin = ((dirX == -1) ? this.raycastOrigins.bottomLeft : this.raycastOrigins.bottomRight) + Vector2.up * distVect.y;
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * dirX, rayLength, collisionMask);
			
			if (hit) {
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				if (slopeAngle != this.collisionInfo.slopeAngle) {
					distVect.x = (hit.distance - skinWidth) * dirX;
					this.collisionInfo.slopeAngle = slopeAngle;
				}
			}
		}
	}
	
	void ClimbSlope(ref Vector3 distVect, float slopeAngle) {
		float moveDistance = Mathf.Abs (distVect.x);
		float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
		
		if (distVect.y <= climbVelocityY) {
			distVect.y = climbVelocityY;
			distVect.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (distVect.x);
			this.collisionInfo.isBelow = true;
			this.collisionInfo.climbingSlope = true;
			this.collisionInfo.slopeAngle = slopeAngle;
		}
	}
	
	void DescendSlope(ref Vector3 distVect) {
		float dirX = Mathf.Sign (distVect.x);
		Vector2 rayOrigin = (dirX == -1) ? this.raycastOrigins.bottomRight : this.raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
		
		if (hit) {
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= this.maxDescendAngle) {
				//Indicates heading in downward direction
				if (Mathf.Sign(hit.normal.x) == dirX) {
					if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(distVect.x)) {
						float moveDistance = Mathf.Abs(distVect.x);
						float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
						distVect.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (distVect.x);
						distVect.y -= descendVelocityY;
						
						this.collisionInfo.slopeAngle = slopeAngle;
						this.collisionInfo.descendingSlope = true;
						this.collisionInfo.isBelow = true;
					}
				}
			}
		}
	}
	
}