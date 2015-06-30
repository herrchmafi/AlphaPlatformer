using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
	//Change in inspector, values set just as a reference for what I think works well
	public float maxJumpHeight = 4.0f;
	public float minJumpHeight = 1.0f;
	public float timeToJumpApex = .4f;
	public float timeToDoubleJumpApex = .3f;
	//Lower results in reaching terminal velocity quicker
	public float accelTimeAirbourne = .2f;
	public float accelTimeGrounded = .1f;
	public float walkSpeed = 6.0f;
	public float sprintSpeed = 12.0f;
	
	public float wallSlideSpeed = 3.0f;
	public float wallStickTime = .25f;
	private Timer wallStickTimer;
	
	public Vector2 wallOff;
	public Vector2 wallJump;
	
	
	private float gravity;
	private float maxJumpVelocity;
	private float minJumpVelocity;
	private float doubleJumpVelocity;
	
	[SerializeField]
	private float targetSpeed;
	
	private Vector3 velocityVect;
	private float velocityXSmoothing;
	
	public enum ActionState {
		IDLE, WALKING, SPRINTING,  
		WALLSLIDING, WALLJUMPING, 
		JUMPING, FALLING
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
		this.gravity = PhysicsHelperMethods.ObjectGravity(this.maxJumpHeight, this.timeToJumpApex);
		this.maxJumpVelocity = PhysicsHelperMethods.JumpVelocity(this.gravity, this.timeToJumpApex);
		this.minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(this.gravity) * this.minJumpHeight);
		this.doubleJumpVelocity = PhysicsHelperMethods.JumpVelocity(this.gravity, this.timeToDoubleJumpApex);
		this.targetSpeed = this.walkSpeed;
		this.wallStickTimer = new Timer();
	}
	
	void Update() {
		this.inputHelper.Update();
		this.wallStickTimer.Update();
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		
		//Handles Ground Speeds
		if (this.controller.CollInfo.isBelow) {
			if (Input.GetButton("Sprint")) {
				this.targetSpeed = this.sprintSpeed;
				this.aState = ActionState.SPRINTING;
			} else if (input.x != 0) {
				this.targetSpeed = this.walkSpeed;
				this.aState = ActionState.WALKING;
			} else {
				//If player is idle and jumps, allows them to have some horizontal mobility
				this.targetSpeed = this.walkSpeed;
				this.aState = ActionState.IDLE;
			}
		}
		//Allows for more gradual changes in velocity
		//Add more for additional types of surfaces
		float targetAccelTime;
		if (this.controller.CollInfo.isBelow) {
			targetAccelTime = this.accelTimeGrounded;
		} else {
			targetAccelTime = this.accelTimeAirbourne;
		}
		
		float targetVelocityX = input.x * this.targetSpeed;
		velocityVect.x = Mathf.SmoothDamp (velocityVect.x, targetVelocityX, ref velocityXSmoothing, targetAccelTime);
		
		bool isWallSliding = false;
		int wallDirX = (this.controller.CollInfo.isLeft) ? -1 : 1;
		
		//If there is a left or right collision and you are not grounded and moving upwards (wall sliding)
		if ((this.controller.CollInfo.isLeft || this.controller.CollInfo.isRight) && !this.controller.CollInfo.isBelow && this.velocityVect.y < .0f) {
			isWallSliding = true;
			this.aState = ActionState.WALLSLIDING;
			if (this.velocityVect.y < -this.wallSlideSpeed) {
				this.velocityVect.y = -this.wallSlideSpeed;
			}
			//Gives player buffer when performing wall leaps
			if (this.wallStickTimer.Seconds < this.wallStickTime) {
				this.velocityXSmoothing = .0f;
				this.velocityVect.x = .0f;
				//If direction input is opposite of wall, player is potentially wall leaping
				if (input.x != wallDirX && input.x != 0) {
					this.wallStickTimer.Start();
				} else {
					this.wallStickTimer.Stop();
				}
			} else {
				this.wallStickTimer.Stop();
			}
		}
		
		//Handles jumps and double jumps
		if (Input.GetButtonDown ("Jump")) {
			if (this.controller.CollInfo.isBelow) {
				this.aState = ActionState.JUMPING;
				this.velocityVect.y = this.maxJumpVelocity;
				
			} else if (isWallSliding) {
				this.hasDoubleJump = false;
				//If wall is opposite direction of input, do a wall leap
				//Otherwise, GTFO
				if (wallDirX == -input.x) {
					this.aState = ActionState.WALLJUMPING;
					this.hasDoubleJump = true;
					this.velocityVect.x = -wallDirX * this.wallJump.x;
					this.velocityVect.y = wallJump.y;
				} else {
					this.velocityVect.x = -wallDirX * this.wallOff.x;
					this.velocityVect.y = wallOff.y;
				}
			} else if (!this.controller.CollInfo.isBelow && this.hasDoubleJump) {
				this.aState = ActionState.JUMPING;
				this.velocityVect.y = 0;
				this.velocityVect.y = this.doubleJumpVelocity;
				this.hasDoubleJump = false;
			}
		}
		if (Input.GetButtonUp("Jump")) {
			//Handles if player releases jump button early
			//Will shortent jump
			if (this.velocityVect.y > this.minJumpVelocity && this.hasDoubleJump) {
				this.velocityVect.y = this.minJumpVelocity;
			}
		}
		
		if (!this.controller.CollInfo.isBelow && Mathf.Sign(this.velocityVect.y) == -1) {
			this.aState = ActionState.FALLING;
		}
		
		this.velocityVect.y += this.gravity * Time.deltaTime;
		this.controller.Move (velocityVect * Time.deltaTime, input);
		
		//Reset velocity whenever top/bottom collision
		if (this.controller.CollInfo.isAbove || this.controller.CollInfo.isBelow) {
			this.velocityVect.y = 0;
			this.hasDoubleJump = this.controller.CollInfo.isBelow;
		}
	}
}
