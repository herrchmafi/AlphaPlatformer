using UnityEngine;
using System.Collections;

public class EventNPCSpeech : MonoBehaviour {

	public Rect textArea = new Rect(0, 0, 300, 100);
	
	public Vector3 textOffset = Vector3.zero;
	
	private Transform target;
	
	private string[] speechComponents;
	public string[] SpeechComponents {
		set { this.speechComponents = value; }
	}
	
	private int speechIndex;
	private string currentSpeech = "";
	
	private int letterIndex;
	
	private bool isSpeaking;
	public bool IsSpeaking {
		get { return this.isSpeaking; }
		set { this.isSpeaking = value; }
	}
	
	private bool isInRange;
	
	private Timer speechTimer;
	private float secondsBetweenLetters = .1f;
	public float SecondsBetweenLetters {
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
		this.speechTimer.Update();
		
		if (this.isSpeaking) {
			//Initialize speech, if speaking, then check if letter index needs to be incremented
			if (!this.speechTimer.IsTiming) {
				this.speechTimer.Start();
				this.speechComponents = new string[3] { "This is a speech component, hear me roar!", "I have massive diarrhea", "This is the last of me, I promise!" };
				
			} else if (this.speechTimer.Seconds >= this.secondsBetweenLetters) {
				this.speechTimer.Reset();
				this.letterIndex++;
			}
			//Checks if letter index exceeds the current speech
			//If so, update in speech contents, o.e. update speech index 
			if (this.letterIndex < this.speechComponents[this.speechIndex].Length) {
				this.currentSpeech = this.speechComponents[this.speechIndex].Substring(0, this.letterIndex); 
			} else {
				this.letterIndex = 0;
				this.speechIndex++;
			}
			//If speechIndex is greater or equal than speech components length, stop talking
			//o.e. stop talking
			if (this.speechIndex >= this.speechComponents.Length) {
				this.letterIndex = 0;
				this.speechIndex = 0;
				this.speechTimer.Stop ();
			}
			
		}
	}

	
	
	
	
}
