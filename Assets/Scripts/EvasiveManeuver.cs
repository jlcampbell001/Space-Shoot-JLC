using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuver : MonoBehaviour {

	public Vector2 startWait;
	public Vector2 manueverTime;
	public Vector2 manueverWait;
	public Boundary boundary;

	public float dodge;
	public float smoothing;
	public float tilt;
    public float targetManeuver;

    private float currentSpeed;
	private Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody> ();
		currentSpeed = rigidbody.velocity.z;
		StartCoroutine (Evade ());
	}

	IEnumerator Evade() {
		yield return new WaitForSeconds (Random.Range(startWait.x, startWait.y));

		while (true) {
			// this will cause it to dodge inwards to keep it away from the side
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign(transform.position.x);
			yield return new WaitForSeconds (Random.Range(manueverTime.x, manueverTime.y));
			targetManeuver = 0;
			yield return new WaitForSeconds (Random.Range(manueverWait.x, manueverWait.y));
		}
	}
	
	public void FixedUpdate () {
        makeManeuver(targetManeuver);
	}

    public void makeManeuver(float targetValue)
    {
        float newManeuver = Mathf.MoveTowards(rigidbody.velocity.x, targetValue, Time.deltaTime * smoothing);
        rigidbody.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);

        float x = Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax);
        float z = Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax);
        rigidbody.position = new Vector3(x, 0.0f, z);

        rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
    }
}
