using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {


	public float moveDistance;
	public float time;
	private float timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if(timer >= time)
		{
			timer = 0;
			transform.position = new Vector3 (transform.position.x - moveDistance, transform.position.y, transform.position.z);
		}

		//transform.position = new Vector3 (transform.position.x - 1, transform.position.y, transform.position.z);
	}
}
