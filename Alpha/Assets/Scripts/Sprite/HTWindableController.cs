using UnityEngine;
using System.Collections;

public class HTWindableController : MonoBehaviour {
	private Animator animator;
	public LayerMask windLayer;
	
	//Holds information for staggered start
	public struct AsynchStartState {
		private int newState;
		public int NewState {
			get { return this.newState; }
			set { this.newState = value; }
		}
		
		private HTTimer timer;
		public HTTimer Timer {
			get {
				if (this.timer == null) {
					this.timer = new HTTimer();
				} 
				return this.timer; 
			}
			set { this.timer = value; }
		}
		
		private float staggerSeconds;
		public float StaggerSeconds {
			get { return this.staggerSeconds; }
			set { this.staggerSeconds = value; }
		}
		
		public void Init (int newState, float staggerSeconds) {
			this.staggerSeconds = staggerSeconds;
			this.newState = newState;
			this.Timer.Start();
		}
	}
	private AsynchStartState asynchStartState;
	// Use this for initialization
	void Start () {
		this.animator = transform.GetComponent<Animator>();
	}
	
	void Update() {
		if (this.asynchStartState.Timer.IsTiming) {
			this.asynchStartState.Timer.Update();
			if (this.asynchStartState.Timer.Seconds >= this.asynchStartState.StaggerSeconds) {
				this.SetWindState(this.asynchStartState.NewState);
			}
		}
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		//Override collision handling if still in stagger state
		if (this.asynchStartState.Timer.IsTiming) {
			return;
		}
		if (HTUnityComponentsHelper.IsInLayerMask(collider.gameObject, this.windLayer)) {
			Vector2 distanceVect = collider.transform.position - transform.position;
			HTWindStrength windStrength = collider.gameObject.GetComponent<HTWindStrength>();
			switch (windStrength.strength) {
				case HTWindStrength.WindStrength.SLIGHT:
					if (distanceVect.x >= 0) {
						this.animator.SetTrigger(HTAnimatorParamsConstants.WindSingleSlightForward);
					} else {
						this.animator.SetTrigger(HTAnimatorParamsConstants.WindSingleSlightBackward);
					} 
					break;
				case HTWindStrength.WindStrength.STRONG:
					break;
				case HTWindStrength.WindStrength.BLUSTERY:
					break;
				default:
					break;
			}
		}
	}
	
	public void SetWindState(int state) {
		this.animator.SetInteger(HTAnimatorParamsConstants.WindStateParam, state);
	}
	
	public void SetWindStateAsynch(int state, float staggerSeconds) {
		this.asynchStartState.Init(state, staggerSeconds);
	}
	
	public void SetWindTrigger(string param) {
		this.animator.SetTrigger(param);
	}
}
