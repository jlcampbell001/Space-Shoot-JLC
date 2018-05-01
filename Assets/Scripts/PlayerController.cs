using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public GameObject[] shotSpawns;

    public float fireRate = 0.5f;

    private float nextFire = 0.0f;

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            for (int i = 0; i < shotSpawns.Length; i++)
            {
                if (shotSpawns[i].activeSelf)
                {
                    // Reset x and z so tilt will not cause shot to go out of bounds towards and away from the screen.
                    Quaternion shotRotation = shotSpawns[i].transform.rotation;
                    shotRotation.x = 0.0f;
                    shotRotation.z = 0.0f;
                    //Debug.Log(shotRotation.x + ", " + shotRotation.y + ", " + shotRotation.z);

                    Instantiate(shot, shotSpawns[i].transform.position, shotRotation);
                    GetComponent<AudioSource>().Play();
                }
            }

        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Rigidbody rigidbody = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rigidbody.velocity = movement * speed;

        float x = Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax);
        float z = Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax);
        rigidbody.position = new Vector3(x, 0.0f, z);

        rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
    }
}
