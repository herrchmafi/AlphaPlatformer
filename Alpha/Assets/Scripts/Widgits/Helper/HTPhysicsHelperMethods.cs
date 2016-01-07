using UnityEngine;
using System.Collections;

public class HTPhysicsHelperMethods {

	public static float ObjectGravity(float jumpHeight, float timeToJumpApex) {
		return -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
	}
	
	public static float JumpVelocity(float gravity, float timeToJumpApex) {
		return Mathf.Abs(gravity) * timeToJumpApex;
	}
}
