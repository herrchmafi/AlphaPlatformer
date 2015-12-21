using UnityEngine;
using System.Collections;

public class HTMotionHelper {
	//isBottom set to true will start at bottom of motion and move upwards vice versa
	public static Vector2 CircularMotion(bool isBottom, float pathDec, float radius, Vector2 pivotPoint) {
		float offset = (isBottom) ? HTMathConstants.radiansPerRevolution * .75f : HTMathConstants.radian * .25f;
		float thetaRadians = pathDec * HTMathConstants.radiansPerRevolution + offset;
		float x = pivotPoint.x + radius * Mathf.Cos(thetaRadians);
		float y = pivotPoint.y + radius * Mathf.Sin(thetaRadians);
		return new Vector2(x, y);
	}
}
