using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreatureData")]
public class CreatureData : ScriptableObject
{
    public float health;
    public float armor;
    public float strength;
    public Element elementalDefense;

    public string[] moveset;
    public float typingSpeed;
    public float spaceBetweenWords;

    public string spriteKind;

    public List<MovementPattern> movementPattern;
}