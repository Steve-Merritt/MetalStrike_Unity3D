using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public int maxHealth = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = 0;

    public RectTransform healthbar;

    private void OnChangeHealth(int health)
    {
        healthbar.sizeDelta = new Vector2((float)health / maxHealth * 100.0f, healthbar.sizeDelta.y);
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}
