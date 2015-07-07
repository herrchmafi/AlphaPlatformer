using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	public Controller2D target;
	public Vector2 focusDimensions;
	private FocusArea focusArea;
	
	public float verticalOffset;
	public float lookAheadDistX;
	public float lookSmoothTimeX;
	public float verticalSmoothTime;
	
	private float currentLookAheadX;
	private float targetLookAheadX;
	private float lookAheadDirX;
	private float smoothLookVelocityX;
	private float smoothVelocityY;
	
	private bool isLookAheadStopped;
	
	void Start() {
		this.focusArea = new FocusArea(this.target.boxCollider.bounds, this.focusDimensions);
	}
	
	void LateUpdate() {
		this.focusArea.Update(this.target.boxCollider.bounds);
		
		Vector2 focusPosition = this.focusArea.center + Vector2.up * this.verticalOffset;
		
		if (this.focusArea.velocity.x != 0) {
			this.lookAheadDirX = Mathf.Sign(this.focusArea.velocity.x);
			if (Mathf.Sign(this.target.PlayerInput.x) == Mathf.Sign(this.focusArea.velocity.x) && this.target.PlayerInput.x != 0) {
				this.isLookAheadStopped = false;
				this.targetLookAheadX = this.lookAheadDirX * this.lookAheadDistX;
			} else {
				if (!this.isLookAheadStopped) {
					this.isLookAheadStopped = true;
					this.targetLookAheadX = this.currentLookAheadX + (this.lookAheadDirX * this.lookAheadDistX - this.currentLookAheadX) / 4.0f;
				}
			}
			
		}
		
		//this.targetLookAheadX = this.lookAheadDirX * this.lookAheadDistX;
		this.currentLookAheadX = Mathf.SmoothDamp(this.currentLookAheadX, this.targetLookAheadX, ref this.smoothLookVelocityX, this.lookSmoothTimeX);
		
		focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref this.smoothVelocityY, this.verticalSmoothTime);
		focusPosition += Vector2.right * this.currentLookAheadX;
		//Mostly arbitrary number. Negative to make camera in front of stage
		transform.position = (Vector3) focusPosition + Vector3.forward * -10;

	}
	
	void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, .5f);
		Gizmos.DrawCube(this.focusArea.center, this.focusDimensions);
	}
	
	struct FocusArea {
		public Vector2 center;
		public Vector2 velocity;
		float left, right;
		float top, bottom;
		
		public FocusArea(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bottom = targetBounds.min.y;
			top = targetBounds.min.y + size.y;
			
			velocity = Vector2.zero;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
		}
		
		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			//If out of bounds, then shift accordingly
			if (targetBounds.min.x < left) {
				shiftX = targetBounds.min.x - left;
			} else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;
			
			float shiftY = 0;
			//If out of bounds, then shift accordingly
			if (targetBounds.min.y < bottom) {
				shiftY = targetBounds.min.y - bottom;
			} else if (targetBounds.max.y > top) {
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bottom += shiftY;
			center = new Vector2((left + right) / 2, (top + bottom) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}
