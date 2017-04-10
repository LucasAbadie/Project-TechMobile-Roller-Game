using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectHandler : MonoBehaviour {

    private static string TAG = "SwitchObjectHandler";
    public GameObject burstEffect;

    public Transform target;

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null)
        {
            Instantiate(burstEffect, target.transform.position, Quaternion.AngleAxis(90, new Vector3(0, 1, 0)));
            Destroy(target.gameObject);
        }
    }
}
