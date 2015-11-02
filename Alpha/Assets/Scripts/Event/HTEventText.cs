using UnityEngine;
using System.Collections;

public abstract class HTEventText : MonoBehaviour {
	public Rect textArea = new Rect(0, 0, 300, 100);
	
	public Vector3 textOffset = Vector3.zero;
	
	public abstract bool IsTexting { get; set; }
	
	public abstract string[] TextComponents { set; }
	public abstract float SecondsBetweenLetters { get; set; }

}
