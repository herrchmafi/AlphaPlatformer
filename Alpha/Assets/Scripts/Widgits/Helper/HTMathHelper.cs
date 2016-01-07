using UnityEngine;
using System.Collections;

public class HTMathHelper {
	public static float DegreesToRadians(float degrees) {
		return HTMathConstants.radiansPerDegree * degrees;
	}

	public static Vector2 NormalizedVectFromRadians(float radians) {
		float x = 1;
		float y = Mathf.Tan(radians) * x;
		return new Vector2(x, y).normalized;
	}
}
