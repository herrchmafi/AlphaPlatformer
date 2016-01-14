using UnityEngine;
using System.Collections;

public class HTMathHelper {
	public static float DegreesToRadians(float degrees) {
		return HTMathConstants.radiansPerDegree * degrees;
	}
	//Use to get point on unit circle based on radians
	public static Vector2 NormalizedVectFromRadians(float radians) {
		float x = Mathf.Cos(radians);
		float y = Mathf.Sin(radians);
		return new Vector2(x, y) / Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
	}
	
	public static Vector2 NonNormalizedVectFromRadians(float radians) {
		return new Vector2(1, Mathf.Tan (radians));
	}
}
