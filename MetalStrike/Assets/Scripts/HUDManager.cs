using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public GameObject spawnTimer;
    private Text spawnTimerText;

    public GameObject creditDisplay;
    private Text creditDisplayText;

    void Start()
    {
        spawnTimerText = spawnTimer.GetComponent<Text>();
        creditDisplayText = creditDisplay.GetComponent<Text>();
    }

    public void UpdateSpawnTimer(int value)
    {
        spawnTimerText.text = value.ToString();
    }

    public void UpdateCredits(int credits)
    {
        creditDisplayText.text = credits.ToString();
    }
}
