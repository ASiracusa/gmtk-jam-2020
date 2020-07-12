using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreatureData")]
public class CreatureData : ScriptableObject
{
    public int health;
    public float armor;
    public float strength;
    public Element elementalDefense;

    public string[] moveset;
    public float typingSpeed;
    public float spaceBetweenWords;

    public string spriteKind;

    public string letterDrops;
    public int lootCount;

    public List<MovementPattern> movementPattern;
}