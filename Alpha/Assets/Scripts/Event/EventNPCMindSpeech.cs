using UnityEngine;
using System.Collections;

public class EventNPCMindSpeech : EventText {
	
	private Transform target;
	
	private string[] textComponents;
	public override string[] TextComponents {
		set { this.textComponents = value; }
	}
	
	private int speechIndex;
	private string currentSpeech = "";
	
	private int letterIndex;
	
	private bool isSpeaking;
	public override bool IsTexting {
		get { return this.isSpeaking; }
		set { this.isSpeaking = value; }
	}
	
	private Timer speechTimer;
	private float secondsBetweenLetters = .1f;
	public override float SecondsBetweenLetters {
		get { return this.secondsBetweenLetters; }
		set { this.secondsBetweenLetters = value; }
	}
	
	void OnGUI(){
		if (this.currentSpeech.Length > 0) {
			Vector3 point = (Vector3)Camera.main.WorldToScreenPoint(this.target.position + this.textOffset);
			this.textArea.x = point.x;
			this.textArea.y = Screen.height - point.y - this.textArea.height; 
			GUI.Label(this.textArea, this.currentSpeech);
		} 
	}
	
	void Start () {
		this.target = transform.parent;
		this.speechTimer = new Timer();
	}
	
	void Update() {
		if (this.isSpeaking) {
			this.speechTimer.Update();
			//Initialize speech, if speaking, then check if letter index needs to be incremented
			if (!this.speechTimer.IsTiming) {
				this.speechTimer.Start();
				this.textComponents = new string[3] { 
					"This is a speech component, hear me roar!", 
					"I have massive diarrhea", 
					"This is the last of me, I promise!" 
				};
				
			} 
			if (this.speechTimer.Seconds >= this.secondsBetweenLetters) {
				this.speechTimer.Reset();
				this.letterIndex++;
			}
			//Checks if letter index exceeds the current speech
			//If so, update in speech contents, o.e. update speech index 
			if (this.letterIndex <= this.textComponents[this.speechIndex].Length) {
				this.currentSpeech = this.textComponents[this.speechIndex].Substring(0, this.letterIndex); 
			} else if (this.speechIndex > this.textComponents[this.speechIndex].Length) {
				this.Next();
			} 
		}
	}
	
	private void Next() {
		//If speechIndex is greater or equal than speech components length, stop talking
		//o.e. stop talking
		if (this.speechIndex < this.textComponents.Length) {
			this.letterIndex = 0;
			this.speechIndex++;
			this.speechTimer.Reset ();
		}
		if (this.speechIndex >= this.textComponents.Length) {
			this.letterIndex = 0;
			this.speechIndex = 0;
			this.currentSpeech = "";
			this.speechTimer.Stop();
			this.isSpeaking = false;
		}
	}
	
	
	
	
}
