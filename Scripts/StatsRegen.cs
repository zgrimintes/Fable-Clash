using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsRegen : PlayerManager
{
    [SerializeField] private float timeToGainMana = 15f;
    private float lastManaGain;

    void Update()
    {
        mana = GetComponent<PlayerManager>().mana;

        if (mana == 4) return;

        if (Time.time - lastManaGain > timeToGainMana)
        {
            lastManaGain = Time.time;
            updateText(mana + 1);
            fighterManager.mana_Gain(gameObject);
        }
    }
}
