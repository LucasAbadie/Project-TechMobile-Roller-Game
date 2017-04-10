using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBoxHandler : MonoBehaviour {

    private string TAG = "WinBoxHandler";

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            // Victory
            LevelManager.Instance.OnWin();
        }
    }
}
