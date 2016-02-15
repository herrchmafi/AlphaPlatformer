using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HTWindPathHelper {
	public enum WindPath {
		STRAIGHT, LOOP, WAVE
	}

	public enum WindPathTemplate {
		STRAIGHT, WAVEY, STRAIGHTWAVEY, WAVEYLOOP 
	}

	public static List<WindPath> straight = new List<WindPath>() {
		WindPath.STRAIGHT
	};
	public static List<WindPath> wavey = new List<WindPath>() {
		WindPath.WAVE
	};
	public static List<WindPath> straightWavey = new List<WindPath>() {
		WindPath.STRAIGHT,
		WindPath.WAVE
	};
	public static List<WindPath> waveyLoop = new List<WindPath>() {
		WindPath.WAVE,
		WindPath.LOOP
	};
}
