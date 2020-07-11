using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionInfo
{
    Synonym,
    Trait,
    Potency,
    Duration,
    Target,
    AnimBehavior,
    AnimSprite,
    AnimColor,
    AnimDuration
}

public enum Target
{
    Self,
    Projectile
}

public enum Trait
{
    Heal,
    Damage,
    Speed
}

public class Actions
{
    public static readonly Dictionary<string, Dictionary<ActionInfo, object>> actions = new Dictionary<string, Dictionary<ActionInfo, object>>()
    {
        {"FIRE", new Dictionary<ActionInfo, object>() {

        } },
        {"HEAL", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Heal },
            { ActionInfo.Potency, 25f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleHeart" },
            { ActionInfo.AnimColor, new Color(1f, 1f, 0.5f) },
            { ActionInfo.AnimDuration, 1f },
        } }
    };
}
