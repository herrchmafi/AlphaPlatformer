using UnityEngine;
using System.Collections;

public class HTMotionHelper {
	//isBottom set to true will start at bottom of motion and move upwards vice versa
	public static Vector2 CircularMotion(bool isBottom, float pathDec, float radius, Vector2 initialPoint) {
		float offsetRadians, dir;
		Vector2 pivotPoint;
		if (isBottom) {
			offsetRadians = HTMathConstants.radiansPerRevolution * .75f;
			dir = 1.0f;
			pivotPoint = new Vector2(initialPoint.x, initialPoint.y + radius);
		} else {
			offsetRadians = HTMathConstants.radiansPerRevolution * .25f;
			dir = -1.0f;
			pivotPoint = new Vector2(initialPoint.x, initialPoint.y - radius);
		}
		float thetaRadians = pathDec * HTMathConstants.radiansPerRevolution + offsetRadians;  
		float x = pivotPoint.x + dir * radius * Mathf.Cos(thetaRadians);
		float y = pivotPoint.y + radius * Mathf.Sin(thetaRadians);
		return new Vector2(x, y);
	}
}
