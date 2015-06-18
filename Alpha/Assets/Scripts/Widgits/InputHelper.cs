using UnityEngine;
using System.Collections;

//This class is responsible for detecting how long keys have been pressed for or thing such as double taps
public class InputHelper {
	public Timer jumpTimer;
	
	public enum KeyPressDuration {
		NONE, SHORT, MEDIUM, LONG
	}
	public KeyPressDuration jumpDuration;
	private bool isJumpPressed;
	
	public void Update() {
		this.jumpTimer.Update();
		if (this.isJumpPressed) {
			if (this.jumpTimer.Seconds >= .1f) {
				this.jumpDuration = KeyPressDuration.LONG;
			}
		} 
		
	}
	
	public void JumpPressed() {
		this.jumpTimer.Start();
		this.isJumpPressed = true;
	}
	
	public void JumpReleased() {
		if (this.jumpTimer.Seconds < .1f) {
			this.jumpDuration = KeyPressDuration.SHORT;
		} else {
			this.jumpDuration = KeyPressDuration.NONE;
		}
		this.jumpTimer.Stop();
		this.isJumpPressed = false;
	}
	
	public InputHelper () {
		this.jumpTimer = new Timer();
	}
}
