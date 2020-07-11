using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public CreatureData creatureData;

    // Creature Data
    private float health;
    private float armor;
    private float strength;
    private Element elementalDefense;
    private string[] moveset;

    // Physical Data
    private Rigidbody2D rb;

    private bool onGround;

    private AnimatorOverrideController aoc;
    private Animator animator;
    private string cdBaseSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        cdBaseSprite = "nothing";

        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        animator.runtimeAnimatorController = aoc;
    }

    public void AssignCreatureType(string cdn)
    {
        CreatureData cd = Resources.Load<CreatureData>("Data/Creatures/" + cdn);
        cdBaseSprite = cdn;

        health = cd.health;
        armor = cd.armor;
        strength = cd.strength;
        elementalDefense = cd.elementalDefense;
        moveset = cd.moveset;

        if (cd.movementPattern.Contains(MovementPattern.PlayerControlled))
        {
            StartCoroutine(PlayerMovement());
        }
        else
        {

        }
    }

    void Update()
    {
        if (Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Idle") != null)
            aoc["idle"] = Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Idle");
        if (Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Walk") != null)
            aoc["walk"] = Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Walk");
        if (Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Cast") != null)
            aoc["cast"] = Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Cast");
        if (Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Rise") != null)
            aoc["rise"] = Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Rise");
        if (Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Apex") != null)
            aoc["apex"] = Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Apex");
        if (Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Fall") != null)
            aoc["fall"] = Resources.Load<AnimationClip>("Animations/" + cdBaseSprite + "/" + cdBaseSprite + "Fall");


    }

    public bool CheckAction(string actionName)
    {
        if (Actions.actions.ContainsKey(actionName))
        {
            StartCoroutine(CastAnimation());

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

    private IEnumerator CastAnimation()
    {
        animator.SetBool("casting", true);
        yield return new WaitForSeconds(0.35f);
        animator.SetBool("casting", false);
        yield return null;
    }

    public void PerformActionOnSelf(Dictionary<ActionInfo, object> actionData)
    {
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Heal)
        {
            StartCoroutine(PassiveEffect(actionData));
        }
    }

    private IEnumerator PlayerMovement()
    {
        GameObject groundChecker = transform.GetChild(1).GetChild(0).gameObject;
        while (true)
        {
            if (!PlayerManager.current.cutscene)
            {
                if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
                {
                    rb.velocity = new Vector2(10, rb.velocity.y);
                    transform.localScale = new Vector3(8, 8, 1);
                    animator.SetBool("moving", true);
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    rb.velocity = new Vector2(-10, rb.velocity.y);
                    transform.localScale = new Vector3(-8, 8, 1);
                    animator.SetBool("moving", true);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.SetBool("moving", false);
                }

                onGround = Physics2D.OverlapCircle(groundChecker.transform.position, 0.02f, LayerMask.GetMask("Ground"));
                print(onGround + " " + groundChecker.transform.position);

                if (onGround && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 20);
                }
            }
            yield return null;
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