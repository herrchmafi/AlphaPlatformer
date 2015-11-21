using UnityEngine;
using System.Collections;

public class HTUnityComponentsHelper {
	public static bool IsInLayerMask(GameObject obj, LayerMask layerMask) {
		int objLayerMask = (1 << obj.layer);
		if ((layerMask.value & objLayerMask) > 0)  // Extra round brackets required!
			return true;
		else
			return false; 
	}
	
}
