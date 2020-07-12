using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Creature : MonoBehaviour
{
    public CreatureData creatureData;

    // Creature Data
    private int health;

    private float armor;
    private float strength;
    private float speed;
    private float jump;

    private Element elementalDefense;
    private string[] moveset;

    private Dictionary<Trait, Coroutine> statModifiers;
    private Dictionary<Trait, GameObject> statParticles;

    // Physical Data
    private Rigidbody2D rb;
    private EnemySpawner spawn;
    private TMP_Text commandBox;

    private bool onGround;
    private bool invulnerable;
    public bool dying;

    private AnimatorOverrideController aoc;
    private Animator animator;
    private string cdBaseSprite;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();

        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        animator.runtimeAnimatorController = aoc;
    }

    public void AssignCreatureType(string cdn, EnemySpawner _spawn)
    {
        rb = transform.GetComponent<Rigidbody2D>();
        commandBox = transform.GetChild(2).GetComponent<TMP_Text>();

        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        animator.runtimeAnimatorController = aoc;

        statModifiers = new Dictionary<Trait, Coroutine>();
        statParticles = new Dictionary<Trait, GameObject>();

        creatureData = Resources.Load<CreatureData>("Data/Creatures/" + cdn);
        cdBaseSprite = creatureData.spriteKind;

        spawn = _spawn;

        health = creatureData.health;
        armor = creatureData.armor;
        strength = creatureData.strength;
        speed = creatureData.speed;
        jump = creatureData.jump;
        elementalDefense = creatureData.elementalDefense;
        moveset = creatureData.moveset;

        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Constants.elementColors[elementalDefense];

        if (creatureData.movementPattern.Contains(MovementPattern.PlayerControlled))
        {
            StartCoroutine(PlayerMovement());
        }
        else
        {
            if (creatureData.movementPattern.Contains(MovementPattern.Patrol))
            {
                StartCoroutine(Patrol());
            }

            StartCoroutine(AutoType());
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

            if (actionData.ContainsKey(ActionInfo.Synonym))
            {
                actionData = Actions.actions[actionData[ActionInfo.Synonym].ToString()];
            }

            if ((Target)actionData[ActionInfo.Target] == Target.Self)
            {
                PerformActionOnSelf(actionData, strength);
            }
            else if ((Target)actionData[ActionInfo.Target] == Target.Projectile)
            {
                GameObject projectile = Instantiate(Resources.Load<GameObject>("ActionEmbodiments/" + actionData[ActionInfo.EmbodiShape]),
                    new Vector3(transform.position.x + (transform.GetChild(0).localScale.x * (float)actionData[ActionInfo.SpawnX]), transform.position.y + (float)actionData[ActionInfo.SpawnY], 1f),
                    Quaternion.identity);
                projectile.GetComponent<ActionEmbodiment>().SetAction(actionData);
                projectile.GetComponent<ActionEmbodiment>().SetCaster(gameObject);
                projectile.GetComponent<ActionEmbodiment>().StartCoroutine(projectile.GetComponent<ActionEmbodiment>().Spawn());
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

    public void PerformActionOnSelf(Dictionary<ActionInfo, object> actionData, float attackerStrength)
    {
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Heal)
        {
            StartCoroutine(PassiveEffect(actionData));
            StartCoroutine(HealthChangeIcon(actionData, (int)(float)actionData[ActionInfo.Potency], 0));

            health = Mathf.Min(creatureData.health, health + (int)(float)actionData[ActionInfo.Potency]);
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Damage && !dying && !invulnerable)
        {
            float multiplier = Constants.typeEffectivenesses[(int)actionData[ActionInfo.Element]][(int)elementalDefense];
            int damage = (int)((float)actionData[ActionInfo.Potency] * multiplier * (attackerStrength / armor));
            StartCoroutine(HealthChangeIcon(actionData, damage, multiplier));

            health = Mathf.Max(0, health - damage);
            if (health <= 0)
            {
                if (creatureData.movementPattern.Contains(MovementPattern.PlayerControlled))
                {
                    PlayerManager.current.StartCoroutine(PlayerManager.current.Respawn());
                }
                else
                {
                    StartCoroutine(Die());
                }
            }
            else
            {
                StartCoroutine(InvincibilityFrames());

            }
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Strength ||
            (Trait)actionData[ActionInfo.Trait] == Trait.Armor ||
            (Trait)actionData[ActionInfo.Trait] == Trait.Speed ||
            (Trait)actionData[ActionInfo.Trait] == Trait.Jump)
        {
            Coroutine cor = StartCoroutine(TempStatMod(actionData));
            statModifiers[(Trait)actionData[ActionInfo.Trait]] = cor;
        }
    }

    private IEnumerator HealthChangeIcon (Dictionary<ActionInfo, object> actionData, int total, float multiplier)
    {
        GameObject indicator = Instantiate(Resources.Load<GameObject>("Prefabs/HealthChangeIndicator"), transform.position, Quaternion.identity);
        //int total = (int)(((Trait)actionData[ActionInfo.Trait] == Trait.Heal) ? actionData[ActionInfo.Potency] : (float)actionData[ActionInfo.Potency] * strength / armor);
        
        indicator.GetComponent<TMP_Text>().text = total.ToString();

        if (multiplier == 1f)
        {
            indicator.GetComponent<TMP_Text>().color = new Color(0.8f, 0.6f, 0.2f);
            PlayerManager.current.StartCoroutine(PlayerManager.current.ShakeCamera(0.25f, 0.15f));
        }
        else if (multiplier == 0f)
        {
            indicator.GetComponent<TMP_Text>().color = new Color(0.4f, 1f, 0.2f);
        }
        else if (multiplier == 2f)
        {
            indicator.GetComponent<TMP_Text>().color = new Color(0.8f, 0f, 0f);
            PlayerManager.current.StartCoroutine(PlayerManager.current.ShakeCamera(0.5f, 0.25f));
        }
        else if (multiplier == 0.5f)
        {
            indicator.GetComponent<TMP_Text>().color = new Color(0.5f, 0.4f, 0.3f);
            PlayerManager.current.StartCoroutine(PlayerManager.current.ShakeCamera(0.1f, 0.05f));
        }

        indicator.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);
        yield return new WaitForSeconds(0.75f);
        GameObject.Destroy(indicator);

        yield return null;
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
                    transform.GetChild(0).localScale = new Vector3(1, 1, 1);
                    animator.SetBool("moving", true);
                }
                else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    rb.velocity = new Vector2(-10, rb.velocity.y);
                    transform.GetChild(0).localScale = new Vector3(-1, 1, 1);
                    animator.SetBool("moving", true);
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    animator.SetBool("moving", false);
                }

                onGround = Physics2D.OverlapCircle(groundChecker.transform.position, 0.02f, LayerMask.GetMask("Ground"));
                animator.SetBool("grounded", onGround);

                if (onGround && Input.GetKeyDown(KeyCode.UpArrow))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 20);
                }

                animator.SetFloat("yVel", rb.velocity.y);
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

        Constants.CompletelyDestroy(particleSystemObject);

        yield return null;
    }

    public int GetHealth ()
    {
        return health;
    }

    public void SetHealth (int _health)
    {
        health = _health;
    }

    public float GetStrength()
    {
        return strength;
    }

    private IEnumerator InvincibilityFrames()
    {
        invulnerable = true;
        yield return new WaitForSeconds(0.5f);
        invulnerable = false;
        yield return null;
    }

    private IEnumerator Die()
    {
        dying = true;

        if (!creatureData.movementPattern.Contains(MovementPattern.PlayerControlled))
        {
            float t = Time.time;
            while (Time.time - t < 0.8f)
            {
                if (transform.localScale.x == 1)
                {
                    transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
                }
                yield return null;
            }

            for (int i = 0; i < creatureData.lootCount; i++)
            {
                int r = UnityEngine.Random.Range(0, creatureData.letterDrops.Length);
                Letter letter = (Letter)Enum.Parse(typeof(Letter), creatureData.letterDrops.ToUpper().Substring(r, 1));

                GameObject drop = Instantiate(Resources.Load<GameObject>("Prefabs/LetterDrop"), new Vector3(transform.position.x, transform.position.y + 0.24f, transform.position.z), Quaternion.identity);
                drop.transform.GetChild(0).GetComponent<TMP_Text>().text = letter.ToString();
                drop.name = letter.ToString();

                drop.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-6, 6), UnityEngine.Random.Range(0, 6));
            }

            if (spawn != null)
            {
                spawn.RemoveEnemy();
            }
            Constants.CompletelyDestroy(gameObject);
        }

        yield return null;
    }

    private IEnumerator Patrol()
    {
        bool goingRight = PlayerManager.current.transform.position.x - transform.position.x > 0;
        float t = Time.time;

        transform.GetChild(0).localScale = new Vector3(goingRight ? 1 : -1, 1, 1);

        animator.SetBool("moving", true);

        while (true)
        {
            if (Time.time - t > 0.5f)
            {
                if (goingRight)
                {
                    if (!Physics2D.OverlapCircle(transform.GetChild(1).GetChild(2).position, 0.04f, LayerMask.GetMask("Ground")) ||
                        Physics2D.OverlapCircle(transform.GetChild(1).GetChild(4).position, 0.04f, LayerMask.GetMask("Ground")))
                    {

                        print("turning left");
                        print(!Physics2D.OverlapCircle(transform.GetChild(1).GetChild(2).position, 0.04f, LayerMask.GetMask("Ground")));
                        print(Physics2D.OverlapCircle(transform.GetChild(1).GetChild(4).position, 0.04f, LayerMask.GetMask("Ground")));

                        transform.GetChild(0).localScale = new Vector3(-1, 1, 1);

                        goingRight = false;
                        t = Time.time;
                    }
                }
                else
                {
                    if (!Physics2D.OverlapCircle(transform.GetChild(1).GetChild(1).position, 0.04f, LayerMask.GetMask("Ground")) ||
                        Physics2D.OverlapCircle(transform.GetChild(1).GetChild(3).position, 0.04f, LayerMask.GetMask("Ground")))
                    {
                        print("turning right");
                        print(!Physics2D.OverlapCircle(transform.GetChild(1).GetChild(1).position, 0.04f, LayerMask.GetMask("Ground")));
                        print(Physics2D.OverlapCircle(transform.GetChild(1).GetChild(3).position, 0.04f, LayerMask.GetMask("Ground")));

                        transform.GetChild(0).localScale = new Vector3(1, 1, 1);

                        goingRight = true;
                        t = Time.time;
                    }
                }
            }

            if (!dying)
            {
                rb.velocity = new Vector2(goingRight ? 3 : -3, rb.velocity.y);
            }

            yield return null;
        }
    }

    private IEnumerator AutoType()
    {
        while (true)
        {
            int spellIndex = (int)UnityEngine.Random.Range(0, creatureData.moveset.Length);
            for (int i = 0; i < creatureData.moveset[spellIndex].Length; i++)
            {
                commandBox.text = commandBox.text + creatureData.moveset[spellIndex].Substring(i, 1);
                yield return new WaitForSeconds(creatureData.typingSpeed);
            }

            commandBox.text = "";
            CheckAction(creatureData.moveset[spellIndex]);

            yield return new WaitForSeconds(creatureData.spaceBetweenWords);
        }
    }

    private IEnumerator TempStatMod(Dictionary<ActionInfo, object> actionData)
    {
        if (statModifiers.ContainsKey((Trait)actionData[ActionInfo.Trait])) { 
            StopCoroutine(statModifiers[(Trait)actionData[ActionInfo.Trait]]);
            Constants.CompletelyDestroy(statParticles[(Trait)actionData[ActionInfo.Trait]]);
        }

        statModifiers.Remove((Trait)actionData[ActionInfo.Trait]);
        statParticles.Remove((Trait)actionData[ActionInfo.Trait]);

        if ((Trait)actionData[ActionInfo.Trait] == Trait.Strength)
        {
            strength = creatureData.strength;
            strength *= (float)actionData[ActionInfo.Potency];
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Armor)
        {
            armor = creatureData.armor;
            armor *= (float)actionData[ActionInfo.Potency];
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Speed)
        {
            speed = creatureData.speed;
            speed *= (float)actionData[ActionInfo.Potency];
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Jump)
        {
            jump = creatureData.jump;
            jump *= (float)actionData[ActionInfo.Potency];
        }
        
        GameObject particleSystemObject = Instantiate(Resources.Load<GameObject>("Particles/Behaviors/" + actionData[ActionInfo.AnimBehavior]), transform);
        ParticleSystem ps = particleSystemObject.GetComponent<ParticleSystem>();
        ps.textureSheetAnimation.AddSprite(Resources.Load<Sprite>("Particles/Sprites/" + actionData[ActionInfo.AnimSprite]));

        statParticles.Add((Trait)actionData[ActionInfo.Trait], particleSystemObject);

        var main = ps.main;
        main.startColor = (Color)actionData[ActionInfo.AnimColor];

        print("started");
        yield return new WaitForSeconds((float)actionData[ActionInfo.Duration]);
        print("ended");

        if ((Trait)actionData[ActionInfo.Trait] == Trait.Strength)
        {
            strength = creatureData.strength;
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Armor)
        {
            armor = creatureData.armor;
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Speed)
        {
            speed = creatureData.speed;
        }
        if ((Trait)actionData[ActionInfo.Trait] == Trait.Jump)
        {
            jump = creatureData.jump;
        }

        Constants.CompletelyDestroy(particleSystemObject);
        statModifiers.Remove((Trait)actionData[ActionInfo.Trait]);
        statParticles.Remove((Trait)actionData[ActionInfo.Trait]);

        yield return null;
    }
}