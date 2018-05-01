using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed;
    public float randomSpeed;
    public float maxSpeed = 7.0f;
    public float minSpeed = 4.0f;

    private void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();

        if (randomSpeed != 0.0f)
        {
            speed = Random.Range(minSpeed, maxSpeed) * randomSpeed;
        }

        rigidbody.velocity = transform.forward * speed;
    }
}
