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
    AnimDuration,

    ProjType,
    ProjVel,
    ProjWeak,

    Element,

    SpawnX,
    SpawnY,

    EmbodiShape
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

public enum ProjectileType
{
    Straight
}

public class Actions
{
    public static readonly Dictionary<string, Dictionary<ActionInfo, object>> actions = new Dictionary<string, Dictionary<ActionInfo, object>>()
    {
        {"FIRE", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Damage },
            { ActionInfo.Potency, 25f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Projectile },
            { ActionInfo.AnimSprite, "particleDot" },
            { ActionInfo.AnimColor, new Color(1f, 0.4f, 0f) },
            { ActionInfo.ProjType, ProjectileType.Straight },
            { ActionInfo.ProjVel, 8f },
            { ActionInfo.ProjWeak, false },
            { ActionInfo.Element, Element.Fire },
            { ActionInfo.SpawnX, 0.24f },
            { ActionInfo.SpawnY, 0f },
            { ActionInfo.EmbodiShape, "MessySpiral" }
        } },
        {"FROST", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Damage },
            { ActionInfo.Potency, 25f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Projectile },
            { ActionInfo.AnimSprite, "particleSnowflake" },
            { ActionInfo.AnimColor, new Color(0.2f, 1f, 1f) },
            { ActionInfo.ProjType, ProjectileType.Straight },
            { ActionInfo.ProjVel, 8f },
            { ActionInfo.ProjWeak, false },
            { ActionInfo.Element, Element.Ice },
            { ActionInfo.SpawnX, 0.24f },
            { ActionInfo.SpawnY, 0f },
            { ActionInfo.EmbodiShape, "MessySpiral" }
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
        } },
        {"HURT", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Damage },
            { ActionInfo.Potency, 40f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.Element, Element.Normal }
        }}
    };
}
