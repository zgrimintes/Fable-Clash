using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsRegen : MonoBehaviour
{
    [SerializeField] private float timeToGainMana = 15f;
    [SerializeField] private FighterManager fighterManager;
    private float lastManaGain;
    private int mana;


    void Update()
    {
        mana = GetComponent<CharacterManager>().mana;

        if (mana == 4) return;

        if (Time.time - lastManaGain > timeToGainMana)
        {
            lastManaGain = Time.time;
            fighterManager.mana_Gain(gameObject);
            GetComponent<CharacterManager>().updateText();
        }
    }
}
