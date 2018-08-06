using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Credits : NetworkBehaviour
{
    [SerializeField] const int startCredits = 0;
    [SerializeField] int creditIncrement = 10;

    public bool bCreditCounterEnabled = false;

    [SyncVar(hook = "OnChangeCredits")]
    public int credits = 0;

    private float timeUntilNextCredits = 1;

    public void OnChangeCredits(int newCredits)
    {
        if (!isLocalPlayer) return;

        credits = newCredits;
        GameObject HUD = GameObject.FindWithTag("HUD");
        HUD.GetComponent<HUDManager>().UpdateCredits(credits);
    }

    private void Update()
    {
        if (bCreditCounterEnabled)
        {
            UpdateCredits();
        }
    }

    private void UpdateCredits()
    {
        if (!isServer) return;

        timeUntilNextCredits = Mathf.Clamp(timeUntilNextCredits -= Time.deltaTime, 0, 1);
        if (timeUntilNextCredits <= 0)
        {
            timeUntilNextCredits = 1;
            credits += creditIncrement;
        }
    }

    public void SpendCredits(int amount)
    {
        credits -= amount;
    }
}
