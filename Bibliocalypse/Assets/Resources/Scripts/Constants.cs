using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Letter
{
    Q,
    W,
    E,
    R,
    T,
    Y,
    U,
    I,
    O,
    P,
    A,
    S,
    D,
    F,
    G,
    H,
    J,
    K,
    L,
    Z,
    X,
    C,
    V,
    B,
    N,
    M
}

public enum Element
{
    Normal,
    Fire,
    Ice,
    Water
}

public enum MovementPattern
{
    Patrol,
    LedgeConscious,
    RunAway,
    PlayerControlled
}

public enum CreatureTraits
{
    Armor,
    Strength,
    Speed
}

public class Constants
{
    public static readonly Dictionary<Element, Color> elementColors = new Dictionary<Element, Color>()
    {
        { Element.Normal, new Color(1f, 1f, 1f) },
        { Element.Fire, new Color(1f, 0.2f, 0f) },
        { Element.Ice, new Color(0.5f, 1f, 1f) },
        { Element.Water, new Color(0.3f, 0.2f, 1f) }
    };

    public static readonly float[][] typeEffectivenesses = new float[][]
    {
        new float[] {1f, 1f, 1f, 1f },
        new float[] {1f, 1f, 2f, 0.5f },
        new float[] {1f, 0.5f, 1f, 2f },
        new float[] {1f, 2f, 0.5f, 1f }
    };
}