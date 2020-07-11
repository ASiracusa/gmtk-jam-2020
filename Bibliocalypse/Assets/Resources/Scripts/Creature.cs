using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    private float health;
    private float armor;
    private float strength;
    private Element elementalDefense;

    private string[] moveset;

    public bool CheckAction(string actionName)
    {
        if (Actions.actions.ContainsKey(actionName))
        {
            Dictionary<ActionInfo, object> actionData = Actions.actions[actionName];
            print("it will happen");

            if ((Target)actionData[ActionInfo.Target] == Target.Self)
            {
                PerformActionOnSelf(actionData);
            }

            return true;
        }
        else
        {
            print("it won't");
            return false;
        }
    }

    public void PerformActionOnSelf(Dictionary<ActionInfo, object> actionData)
    {
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Heal)
        {
            StartCoroutine(PassiveEffect(actionData));
        }
    }

    private IEnumerator PassiveEffect (Dictionary<ActionInfo, object> actionData)
    {
        float startTime = Time.time;

        GameObject particleSystemObject = Instantiate(Resources.Load<GameObject>("Particles/Behaviors/" + actionData[ActionInfo.AnimBehavior]), transform);
        ParticleSystem ps = particleSystemObject.GetComponent<ParticleSystem>();
        ps.textureSheetAnimation.AddSprite(Resources.Load<Sprite>("Particles/Sprites/" + actionData[ActionInfo.AnimSprite]));

        var main = ps.main;
        main.startColor = (Color)actionData[ActionInfo.AnimColor];

        while (Time.time - startTime < (float)actionData[ActionInfo.AnimDuration])
        {
            yield return null;
        }

        startTime = Time.time;
        var emission = ps.emission;
        emission.rateOverTime = 0;
        while (Time.time - startTime < ps.startLifetime)
        {
            yield return null;
        }

        GameObject.Destroy(particleSystemObject);

        yield return null;
    }
}

public class CreatureData
{
    private float health;
    private float armor;
    private float strength;
    private Element elementalDefense;

    private string[] moveset;

    private MovementPattern movementPattern;
}