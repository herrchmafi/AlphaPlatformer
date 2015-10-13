using UnityEngine;
using System.Collections;

public class PercipitationController : MonoBehaviour {

	public enum Magnitude { NONE, LIGHT, MEDIUM, HEAVY }
	//[SerializeField]
	public Magnitude magnitude;
	
	#region RNG bucket percentages
	public int bucketsNone;
	public int bucketsLight;
	public int bucketsMedium;
	public int bucketsHeavy;
	
	private Magnitude[] rngBuckets;
	public Magnitude[] RNGBuckets {
		get { return this.rngBuckets; }
	}
	private void setRNGBuckets() {
		this.rngBuckets = new Magnitude[this.bucketsNone + this.bucketsLight
								+ this.bucketsMedium + this.bucketsHeavy];
		int relativeIndex = 0;
		for (int i = relativeIndex; i < this.bucketsNone + relativeIndex; i++) {
			this.rngBuckets[i] = Magnitude.NONE;
		}
		relativeIndex += this.bucketsNone;
		for (int i = relativeIndex; i < this.bucketsLight + relativeIndex; i++) {
			this.rngBuckets[i] = Magnitude.LIGHT;
		}
		relativeIndex += this.bucketsLight;
		for (int i = relativeIndex; i < this.bucketsMedium + relativeIndex; i++) {
			this.rngBuckets[i] = Magnitude.MEDIUM;
		}
		relativeIndex += this.bucketsMedium;
		for (int i = relativeIndex; i < this.bucketsHeavy + relativeIndex; i++) {
			this.rngBuckets[i] = Magnitude.HEAVY;
		}
	}
	
	#endregion
	
	#region Light Vars
	//Precipitation System
	public float startSpeedLight;
	public float startSizeLight;
	public int maxParticlesLight;
	
	//Emission
	public float emissionRateLight;
	
	#endregion
	
	#region Medium Vars
	//Precipitation System
	public float startSpeedMedium;
	public float startSizeMedium;
	public int maxParticlesMedium;
	
	//Emission
	public float emissionRateMedium;
	
	#endregion
	
	#region Light Vars
	//Precipitation System
	public float startSpeedHeavy;
	public float startSizeHeavy;
	public int maxParticlesHeavy;
	
	//Emission
	public float emissionRateHeavy;
	
	#endregion
	
	#region Shared
	//Renderer
	public Material material;
	
	#endregion
	
	private ParticleSystem particleSystem;
	
	void Start () {
		this.particleSystem = gameObject.GetComponent<ParticleSystem>();
		this.SetNone();
		this.setRNGBuckets();
	}
	private void SetNone () {
		this.particleSystem.enableEmission = false;
		this.magnitude = Magnitude.NONE;
	}
	
	private void SetLight () {
		this.magnitude = Magnitude.LIGHT;
		this.particleSystem.enableEmission = true;
		this.particleSystem.startSpeed = this.startSpeedLight;
		this.particleSystem.startSize = this.startSizeLight;
		this.particleSystem.maxParticles = this.maxParticlesLight;
		this.particleSystem.emissionRate = this.emissionRateLight;
	
	}
	
	private void SetMedium () {
		this.particleSystem.enableEmission = true;
		this.magnitude = Magnitude.MEDIUM;
		this.particleSystem.startSpeed = this.startSpeedMedium;
		this.particleSystem.startSize = this.startSizeMedium;
		this.particleSystem.maxParticles = this.maxParticlesMedium;
		this.particleSystem.emissionRate = this.emissionRateMedium;
	}
	
	private void SetHeavy () {
		this.particleSystem.enableEmission = true;
		this.magnitude = Magnitude.HEAVY;
		this.particleSystem.startSpeed = this.startSpeedHeavy;
		this.particleSystem.startSize = this.startSizeHeavy;
		this.particleSystem.maxParticles = this.maxParticlesHeavy;
		this.particleSystem.emissionRate = this.emissionRateHeavy;
	}
	
	public void SetMagnitudeFromBucket(int bucket) {
		switch (this.rngBuckets[bucket]) {
			case Magnitude.NONE:
				this.SetNone();
				break;
			case Magnitude.LIGHT:
				this.SetLight();
				break;
			case Magnitude.MEDIUM:
				this.SetMedium();
				break;
			case Magnitude.HEAVY:
				this.SetHeavy();
				break;
		}
	}
}
