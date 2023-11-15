using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "FighterManager", menuName ="Figher Manager")]
public class FighterManager : ScriptableObject
{
    public int HP;
    public ScriptableObject wClass;
    public string characterName;
}
