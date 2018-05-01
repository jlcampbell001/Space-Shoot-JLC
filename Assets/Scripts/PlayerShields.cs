using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShields : MonoBehaviour {

    public int shieldPower;
    public int maxShieldPower;
    public GameObject shield;
    public Text shieldText;

    private void Start()
    {
        SetActiveShields();
        SetShieldText();
    }

    public void AdjustShieldPower(int adjustment)
    {
        if (shieldPower + adjustment <= maxShieldPower)
        {
            shieldPower = Mathf.Max(0, shieldPower + adjustment);
            SetActiveShields();
            SetShieldText();
        }
    }

    void SetActiveShields()
    {
        //Enable shields if they have power.
        if (shieldPower > 0)
        {
            shield.SetActive(true);
        }
        else
        //Disable shields if they are out of power.
        {
            shield.SetActive(false);
        }
    }

    void SetShieldText()
    {
        shieldText.text = "Shield Power: " + shieldPower;
    }
}
