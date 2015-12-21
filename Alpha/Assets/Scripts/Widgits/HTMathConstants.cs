using UnityEngine;
using System.Collections;
using System;

public class HTMathConstants {

	public const float degreesPerRevolution = 360.0f;
	public const float radiansPerRevolution = 2 * (float)Math.PI;
	public const float radian = 180.0f / (float)Math.PI;
	
	public static readonly Vector2 nullPoint = new Vector2(float.MinValue, float.MinValue);
}
