using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour {

    private float duration = 1.5f;
    private float startTime;

	// Use this for initialization
	private void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	private void Update () {
        if (Time.time - startTime > duration)
            Destroy(this.gameObject);
	}
}
