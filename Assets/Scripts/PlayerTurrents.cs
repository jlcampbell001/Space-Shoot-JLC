using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurrents : MonoBehaviour {

    public int turrentLevel;
    public int maxTurrentLevel;
    public GameObject bowTurrent;
    public GameObject portBow;
    public GameObject starboardBow;
    public GameObject portBowAngle;
    public GameObject starboardBowAngle
        ;

    // Use this for initialization
    void Start () {
        SetActiveTurrents();
	}
	
	public void AdjustTurrentLevel(int adjustment)
    {
        if (turrentLevel + adjustment <= maxTurrentLevel)
        {
            turrentLevel = Mathf.Max(1, turrentLevel + adjustment);
            SetActiveTurrents();
        }
    }

    void SetActiveTurrents()
    {
        // One active turrent.
        if (turrentLevel < 2 || turrentLevel > maxTurrentLevel)
        {
            bowTurrent.SetActive(true);
            portBow.SetActive(false);
            starboardBow.SetActive(false);
            portBowAngle.SetActive(false);
            starboardBowAngle.SetActive(false);
        }

        // Two active turrents.
        if (turrentLevel == 2)
        {
            bowTurrent.SetActive(false);
            portBow.SetActive(true);
            starboardBow.SetActive(true);
            portBowAngle.SetActive(false);
            starboardBowAngle.SetActive(false);
        }

        // Three active turrents.
        if (turrentLevel == 3)
        {
            bowTurrent.SetActive(true);
            portBow.SetActive(false);
            starboardBow.SetActive(false);
            portBowAngle.SetActive(true);
            starboardBowAngle.SetActive(true);
        }

    }
}
