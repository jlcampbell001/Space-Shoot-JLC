using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        /*
        DestroyByContact dbc = other.GetComponent<DestroyByContact>();
        if (dbc != null && dbc.hps == 1)
        {
            Debug.Log(other.transform.position);
        }
        */

        Destroy(other.gameObject);
    }
}
