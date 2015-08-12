using UnityEngine;
using System.Collections;

public class MindBullet : MonoBehaviour {
	public float speed;

	private int dir;
	public int Dir {
		set { this.dir = value; }
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector2(dir * speed * Time.deltaTime, .0f));	
	}
	
	void OnCollisionEnter2D (Collision2D coll) {
	
		if (!coll.gameObject.tag.Equals("Player")) {
			DestroyObject(gameObject);
		}
	}	
}
