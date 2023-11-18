using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeightClass", menuName = "Weight Class ")]
public class WeightClass : ScriptableObject
{
    public string Class_Type;
    public float speed;
    public int _NA_dmg;
    public int _HA_dmg;
    public int _RA_dmg;
    public int _MA_dmg;
    public int _S_dmg;

}
