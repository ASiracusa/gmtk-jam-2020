using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEmbodiment : MonoBehaviour
{
    private Dictionary<ActionInfo, object> action;
    private GameObject caster;
    private Rigidbody2D rb;

    void Start()
    {

    }

    public void SetAction (Dictionary<ActionInfo, object> _action)
    {
        action = _action;
    }

    public void SetCaster (GameObject _caster)
    {
        caster = _caster;
    }

    public IEnumerator Spawn()
    {
        rb = GetComponent<Rigidbody2D>();

        ParticleSystem ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        ps.textureSheetAnimation.AddSprite(Resources.Load<Sprite>("Particles/Sprites/" + action[ActionInfo.AnimSprite]));

        var main = ps.main;
        main.startColor = (Color)action[ActionInfo.AnimColor];

        StartCoroutine(Move());
        yield return null;
    }

    private IEnumerator Move()
    {
        if ((ProjectileType)action[ActionInfo.ProjType] == ProjectileType.Straight)
        {
            float directionMod = caster.transform.GetChild(0).localScale.x;
            while (true)
            {
                rb.velocity = new Vector2((float)action[ActionInfo.ProjVel] * directionMod, 0);
                yield return true;
            }
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hit " + collision.gameObject.name);
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
        {
            print("hit a wall/ground");
            Destroy(gameObject);
        }
        else if (collision.gameObject != caster && !collision.gameObject.layer.Equals(LayerMask.NameToLayer("Pickup")))
        {
            print(collision.gameObject.name);
            collision.gameObject.GetComponent<Creature>().PerformActionOnSelf(action, caster.gameObject.GetComponent<Creature>().GetStrength());
            if ((bool)action[ActionInfo.ProjWeak])
            {
                print("hit an enemy and fizzled");
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator End()
    {
        ParticleSystem ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        float startTime = Time.time;
        var emission = ps.emission;
        emission.rateOverTime = 0;
        var main = ps.main;
        while (Time.time - startTime < main.startLifetime.constantMax)
        {
            yield return null;
        }

        yield return null;
    }
}
