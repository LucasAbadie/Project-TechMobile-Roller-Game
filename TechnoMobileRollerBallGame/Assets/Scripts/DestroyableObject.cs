using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {

    public float forceRequired = 15.0f;
    public GameObject burstEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.impulse.magnitude > forceRequired)
        {
            Instantiate(burstEffect, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
