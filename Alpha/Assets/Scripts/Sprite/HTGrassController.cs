using UnityEngine;
using System.Collections;

public class HTGrassController : MonoBehaviour {
	private Animator animator;
	// Use this for initialization
	void Start () {
		this.animator = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("1")) {
			this.animator.SetInteger("Wind State", 0);
		} else if (Input.GetKeyDown("2")) {
			this.animator.SetInteger("Wind State", 1);
		} else if (Input.GetKeyDown("3")) {
			this.animator.SetInteger("Wind State", 2);
		} else if (Input.GetKeyDown("4")) {
			this.animator.SetInteger("Wind State", 3);
		}
	}
}
