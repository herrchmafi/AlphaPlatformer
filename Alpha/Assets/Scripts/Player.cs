using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float jumpHeight;
	public float timeToJumpApex;
	public float doubleJumpHeight;
	public float timeToDoubleJumpApex;
	//Lower results in reaching terminal velocity quicker
	public float accelTimeAirbourne;
	public float accelTimeGrounded;
	public float walkSpeed;
	public float sprintSpeed;

	private float gravity;
	private float jumpVelocity;
	private float doubleJumpVelocity;
	
	[SerializeField]
	private float targetSpeed;
	
	private Vector3 velocityVect;
	private float velocityXSmoothing;
	
	public enum ActionState {
		IDLE, WALKING, SPRINTING, JUMPING
	}
	[SerializeField]
	private ActionState aState;
	public ActionState AState {
		get { return this.aState; }
	}
	
	private bool hasDoubleJump;
	
	private InputHelper inputHelper;

	private Controller2D controller;

	void Start() {
		this.inputHelper = new InputHelper();
		this.controller = GetComponent<Controller2D> ();
		this.gravity = PhysicsHelperMethods.ObjectGravity(this.jumpHeight, this.timeToJumpApex);
		this.jumpVelocity = PhysicsHelperMethods.JumpVelocity(this.gravity, this.timeToJumpApex);
		this.doubleJumpVelocity = PhysicsHelperMethods.JumpVelocity(this.gravity, this.timeToDoubleJumpApex);
		this.targetSpeed = this.walkSpeed;
	}

	void Update() {
	
		this.inputHelper.Update();
	
		//Reset velocity whenever top/bottom collision
		if (this.controller.CollInfo.isAbove || this.controller.CollInfo.isBelow) {
			this.velocityVect.y = 0;
			this.hasDoubleJump = true;
		}
		//Handles jumps and double jumps
		if (Input.GetButtonDown ("Jump")) {
			if (this.controller.CollInfo.isBelow) {
				this.velocityVect.y = this.jumpVelocity;

			} else if (!this.controller.CollInfo.isBelow && this.hasDoubleJump) {
				this.velocityVect.y = 0;
				this.velocityVect.y = this.doubleJumpVelocity;
			}
		}
		
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		
		//Allows for more gradual changes in velocity
		//Add more for additional types of surfaces
		float targetAccelTime;
		if (this.controller.CollInfo.isBelow) {
			targetAccelTime = this.accelTimeGrounded;
		} else {
			targetAccelTime = this.accelTimeAirbourne;
		}
		//Handles different changing ground speeds
		if (this.controller.CollInfo.isBelow) {
			if (Input.GetButton("Sprint")) {
				this.targetSpeed = this.sprintSpeed;
			} else {
				this.targetSpeed = this.walkSpeed;
			}
		}
		float targetVelocityX = input.x * this.targetSpeed;
		velocityVect.x = Mathf.SmoothDamp (velocityVect.x, targetVelocityX, ref velocityXSmoothing, targetAccelTime);
		velocityVect.y += this.gravity * Time.deltaTime;
		this.controller.Move (velocityVect * Time.deltaTime);
	}
}
