using UnityEngine;
using System.Collections;

public class HTWindStrength : MonoBehaviour {
	public enum WindStrength {
		IDLE = 0,
		SLIGHT = 1,
		STRONG = 2,
		BLUSTERY = 3
	}
	public WindStrength strength;
}
