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
    Strength,
    Armor,
    Speed,
    Jump
}

public enum ProjectileType
{
    Straight
}

public enum StatModifier
{
    strength,
    armor,
    speed,
    jumppower
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
        {"FLOW", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Damage },
            { ActionInfo.Potency, 25f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Projectile },
            { ActionInfo.AnimSprite, "particleDrop" },
            { ActionInfo.AnimColor, new Color(0.2f, 1f, 1f) },
            { ActionInfo.ProjType, ProjectileType.Straight },
            { ActionInfo.ProjVel, 8f },
            { ActionInfo.ProjWeak, false },
            { ActionInfo.Element, Element.Water },
            { ActionInfo.SpawnX, 0.24f },
            { ActionInfo.SpawnY, 0f },
            { ActionInfo.EmbodiShape, "MessySpiral" }
        } },

        {"HEAL", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Heal },
            { ActionInfo.Potency, 15f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleHeart" },
            { ActionInfo.AnimColor, new Color(1f, 1f, 0.5f) },
            { ActionInfo.AnimDuration, 1f },
        } },
        {"MEND", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Heal },
            { ActionInfo.Potency, 25f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleHeart" },
            { ActionInfo.AnimColor, new Color(1f, 1f, 0.5f) },
            { ActionInfo.AnimDuration, 1f },
        } },
        {"CURE", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Synonym, "MEND" },
        } },
        {"RESTORE", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Synonym, "MEND" },
        } },
        {"REMEDY", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Heal },
            { ActionInfo.Potency, 35f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleHeart" },
            { ActionInfo.AnimColor, new Color(1f, 1f, 0.5f) },
            { ActionInfo.AnimDuration, 1f },
        } },
        {"PANACEA", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Synonym, "REMEDY" },
        } },

        {"BUFF", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Strength },
            { ActionInfo.Potency, 1.5f },
            { ActionInfo.Duration, 30f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleSpark" },
            { ActionInfo.AnimColor, new Color(0.7f, 0.1f, 0f) },
            { ActionInfo.AnimDuration, 1f },
        } },
        {"FLEX", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Strength },
            { ActionInfo.Potency, 2f },
            { ActionInfo.Duration, 30f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleSpark" },
            { ActionInfo.AnimColor, new Color(0.7f, 0.1f, 0f) },
            { ActionInfo.AnimDuration, 1f },
        } },

        {"DEBUFF", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Strength },
            { ActionInfo.Potency, 0.75f },
            { ActionInfo.Duration, 10f },
            { ActionInfo.Target, Target.Projectile },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleSpark" },
            { ActionInfo.AnimColor, new Color(0.8f, 0f, 0.8f) },
            { ActionInfo.ProjType, ProjectileType.Straight },
            { ActionInfo.ProjVel, 8f },
            { ActionInfo.ProjWeak, false },
            { ActionInfo.Element, Element.Normal },
            { ActionInfo.SpawnX, 0.24f },
            { ActionInfo.SpawnY, 0f },
            { ActionInfo.EmbodiShape, "MessySpiral" },
        } },
        {"WEAKEN", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Strength },
            { ActionInfo.Potency, 0.6f },
            { ActionInfo.Duration, 10f },
            { ActionInfo.Target, Target.Projectile },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleSpark" },
            { ActionInfo.AnimColor, new Color(0.8f, 0f, 0.8f) },
            { ActionInfo.ProjType, ProjectileType.Straight },
            { ActionInfo.ProjVel, 8f },
            { ActionInfo.ProjWeak, false },
            { ActionInfo.Element, Element.Normal },
            { ActionInfo.SpawnX, 0.24f },
            { ActionInfo.SpawnY, 0f },
            { ActionInfo.EmbodiShape, "MessySpiral" },
        } },

        {"IRON", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Armor },
            { ActionInfo.Potency, 1.25f },
            { ActionInfo.Duration, 10f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleSpark" },
            { ActionInfo.AnimColor, new Color(0.7f, 0.7f, 0.7f) },
            { ActionInfo.AnimDuration, 1f },
        } },
        {"STEEL", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Synonym, "IRON" },
        } },
        {"ARMOR", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Armor },
            { ActionInfo.Potency, 1.5f },
            { ActionInfo.Duration, 10f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.AnimBehavior, "Bubbling" },
            { ActionInfo.AnimSprite, "particleSpark" },
            { ActionInfo.AnimColor, new Color(0.5f, 0.5f, 0.5f) },
            { ActionInfo.AnimDuration, 1f },
        } },

        {"HURT", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Damage },
            { ActionInfo.Potency, 40f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.Element, Element.Normal }
        } },
        {"DIE", new Dictionary<ActionInfo, object>() {
            { ActionInfo.Trait, Trait.Damage },
            { ActionInfo.Potency, 1000f },
            { ActionInfo.Duration, 0f },
            { ActionInfo.Target, Target.Self },
            { ActionInfo.Element, Element.Normal }
        } }
    };
}
