using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class EnemyBoss : MonoBehaviour, IDamangable
{
    // Singleton but multiple, just to be used by SendSoundLocation to all object
    public static List<EnemyBoss> instances = new List<EnemyBoss>();

    public enum State { Idle = 10, Wander = 20, Search = 30, Stalk = 40, Charge = 50}
    private State currentState = State.Idle;
    private State nextState = State.Idle;

    // setting
    [Header("Setting")]
    [SerializeField] private float soundDetectRange = 10f;    // Will go into State.Find if idle if sound is in this radius
    [SerializeField] private float visionRange = 5f;    // Will go into State.Stalk if see player in this range
    [SerializeField] private float farRange = 15f;
    [SerializeField] private Vector2 detectionOffset;
    //[SerializeField] private LayerMask visionMask;

    // lightning setting
    [Header("Lightning Setting")]
    [SerializeField] private Vector2 brightnessLerp = new Vector2(0, 1);
    [SerializeField] private Vector2 alphaLerp = new Vector2(0, 1);

    // ai
    private Vector2 interestPos;
    [Header("Behavior Setting")]
    [SerializeField] private float wanderRadius;
    [Tooltip("If player has not come in the farRange within this time, next wandering will be close to the player")]
    [SerializeField] private float farRangePatienceTime = 12;
    [SerializeField] private float playerTimeInFarRange = 0;
    [SerializeField] private float stalkDistance = 15;

    private float initialAgentSpeed;
    [Header("State move speed stting, based on agent's speed")]
    [SerializeField] private float wanderSpeedFactor = 1;
    [SerializeField] private float searchSpeedFactor = 1.3f;
    [SerializeField] private float stalkInSpeedFactor = 0.7f;
    [SerializeField] private float stalkOutSpeedFactor = 1.4f;
    [SerializeField] private float chargeSpeedFactor = 2.1f;

    private float playerSqrDist = 0;

    [Header("Attack Setting")]
    [SerializeField] private float attackRange = 3.2f;
    [SerializeField] private float attackTime = 0.8f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Damage til stun")]
    [SerializeField] private int stunHealthMax = 2;
    [SerializeField] private float timeTilRegen = 20;
    [SerializeField] private int stunHealth = 0;
    private float timeSinceStunned = 0;

    [Header("Sound")]
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField] private AudioClip[] crySounds;
    [SerializeField] private AudioClip[] hurtSounds;
    private float walkSoundFrequencyFactor = 1;
    private bool walkSoundEnable = false;

    // references
    private NavMeshAgent agent;
    private Animator animator;
    [Header("References")]
    [SerializeField] Animator decoyAnimator;
    private Transform player;

    // ----------------------Start-------------------------
    void Awake()
    {   
        currentState = State.Idle;
        nextState = State.Idle;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent = GetComponent<NavMeshAgent> ();
        animator = GetComponent<Animator>();
        player = Extension.GetPlayer();
    }

    void Start() {
        initialAgentSpeed = agent.speed;
        stunHealth = stunHealthMax;

        RestartCR();
        
        AnimatorMove(false);
        SyncLightning(0);
    }

    void RestartCR() {
        StopAllCoroutines();
        StartCoroutine(PeriodicAttackCR());
        StartCoroutine(WalkSoundCR());
        StartNextState();
    }

    // ----------------------Update-------------------------
    void Update() {
        // update player sqr distance
        PlayerSqrDistUpdate();

        // detect player in view
        VisionUpdate();

        // update far range time
        FarRangeUpdate();

        // update flip sprite
        FlipSpriteUpdate();

        // update stun health
        StunHealthUpdate();

        // if lightning, change from stalk or charge to wander
        // in sync lightning
    }

    void PlayerSqrDistUpdate() {
        if (player == null) return;
        
        Vector2 _pos = (Vector2)transform.position + detectionOffset; // add offset
        Vector2 deltaPos = (Vector2)player.position - _pos;
        playerSqrDist = deltaPos.sqrMagnitude;
    }
    
    void VisionUpdate() {
        if (player == null) return;

        // check if player is out of range
        if (playerSqrDist > Mathf.Pow(visionRange, 2)) return;
        //Debug.Log("c1: player in range");

        /*
        // check if anything obstruct the view 
        RaycastHit2D hit = Physics2D.Raycast(_pos, deltaPos.normalized, visionRange, visionMask);
        Debug.DrawRay(_pos, deltaPos, Color.red, 0.2f);
        if (!hit) {Debug.Log("c2 fail: can't find any collision"); return;}
        if (hit.collider.GetComponent(typeof(PlayerMovement)) == null) {
            Debug.Log("c2 fail: collide with: " + hit.collider.name + " at " + hit.point.x + "," + hit.point.y);
            return;
        }
        Debug.Log("c2: player is unobstruct" + " at " + hit.point.x + "," + hit.point.y);
        */

        // if it can see player
        if ((int)currentState < 35) nextState = State.Stalk;
    }

    void FarRangeUpdate() {
        if (playerSqrDist > Mathf.Pow(farRange, 2)) {
            playerTimeInFarRange += Time.deltaTime;
        } else {
            playerTimeInFarRange = 0;
        }
    }

    void FlipSpriteUpdate() {
        if (Mathf.Sign(agent.velocity.x) == Mathf.Sign(transform.localScale.x)) {
            Vector3 _scale = transform.localScale;
            _scale.x *= -1;
            transform.localScale = _scale;
        }
    }

    void StunHealthUpdate() {
        timeSinceStunned += Time.deltaTime;
        if (timeSinceStunned > timeTilRegen) {
            stunHealth = stunHealthMax;
        }

        if (stunHealth <= 0) {
            nextState = State.Stalk;
            stunHealth = stunHealthMax;
            RestartCR();
        }
    }

    // ----------------------Static-------------------------
    public static void SendTargetSoundLocation(Vector3 target) {
        foreach (EnemyBoss b in instances) {
            //foreach instance, check if sound are in the detectable range
            Vector2 deltaPos = ((Vector2)b.transform.position + b.detectionOffset) - (Vector2)target;
            if (deltaPos.sqrMagnitude < Mathf.Pow(b.soundDetectRange, 2)) {
                b.SetTargetLocation(target);
            }
        }
    }

    private void SetTargetLocation(Vector2 target) {
        // set target and change state
        interestPos = target;
        // Debug.Log("Boss detect sound");

        // Idle -> Search
        if ((int)currentState < 25) nextState = State.Search;
    }

    public static void SyncLightning(float factor) {
        foreach (EnemyBoss b in instances) {
            SpriteRenderer s = b.GetComponent<SpriteRenderer>();
            float bright = Mathf.Lerp(b.brightnessLerp.x, b.brightnessLerp.y, factor);
            float alpha = Mathf.Lerp(b.alphaLerp.x, b.alphaLerp.y, factor);
            s.color = new Color(bright, bright, bright, alpha);

            /*
            // if lightning, change from stalk or charge to wander
            if (factor >= 1 && (int)b.currentState > 25) {
                // Debug.Log("set lightning state");
                b.nextState = State.Wander;
                b.RestartCR();
            }
            */
        }
    }

    // ----------------------Damage-------------------------
    private void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);
        foreach (var col in hits)
        {
            // Debug.Log(col.name);
            IDamangable damagable = col.GetComponent<IDamangable>();
            if(damagable != null)
            {
                damagable.TakeDamage();
            }
        }
    }

    public void TakeDamage() {
        stunHealth -= 1;

        PlaySound(hurtSounds);
    }

    IEnumerator PeriodicAttackCR() {
        while (true) {
            Attack();
            yield return new WaitForSeconds(attackTime);
        }
    }

    // ----------------------State Machine-------------------------
    IEnumerator IdleStateCR() {
        currentState = State.Idle;
        AnimatorMove(false, 1.25f);

        yield return new WaitForSeconds(Random.Range(0.3f, 1.6f));

        ModifyNextState();
        StartNextState();
    }

    IEnumerator WanderStateCR() {
        currentState = State.Wander;
        agent.speed = initialAgentSpeed * wanderSpeedFactor;
        AnimatorMove(true, wanderSpeedFactor);

        // random point to go to, unless player is far for too long, then it wander close to player
        Vector3 origin = transform.position;
        if (playerTimeInFarRange > farRangePatienceTime && player != null) {
            // if player has been far away for too long
            // set random destination based on player position instead
            origin = player.position;
        }
        Vector3 newPos = RandomNavCircle(origin, wanderRadius, -1);
        agent.SetDestination(newPos);
        yield return new WaitForSeconds(Random.Range(1.5f, 2.7f));

        ModifyNextState();
        StartNextState();
    }

    IEnumerator SearchStateCR() {
        currentState = State.Search;
        agent.speed = initialAgentSpeed * searchSpeedFactor;
        AnimatorMove(true, searchSpeedFactor);
        
        // go to interestPos
        // wait
        // wander
        agent.SetDestination(interestPos);
        yield return new WaitForSeconds(Random.Range(1.4f, 2f));
        
        ModifyNextState();
        StartNextState();
    }

    IEnumerator StalkStateCR() {
        currentState = State.Stalk;

        // check if player is null
        if (player == null) nextState = State.Wander;

        // timer
        float timer = Random.Range(2f, 4f);

        // keep distance but follow
        // if player is in soundDetectRange move away;
        // else move closer
        while (true && player != null) {
            float loopTime = Random.Range(.4f, .7f);
            if (timer < 0) break;
            if (playerSqrDist < Mathf.Pow(stalkDistance, 2)) {
                // move away
                agent.speed = initialAgentSpeed * stalkOutSpeedFactor;
                AnimatorMove(true, stalkOutSpeedFactor);

                Vector3 newPos = transform.position - player.position;
                newPos = newPos.normalized;
                newPos *= stalkDistance * 1.25f;
                agent.SetDestination(newPos);
            } else {
                // move closer
                agent.speed = initialAgentSpeed * stalkInSpeedFactor;
                AnimatorMove(true, stalkInSpeedFactor);

                Vector3 newPos = RandomNavCircle(player.position, soundDetectRange, -1);
                agent.SetDestination(newPos);
            }
            yield return new WaitForSeconds(loopTime);
            timer -= loopTime;
        }

        ModifyNextState();
        StartNextState();
    }

    IEnumerator ChargeStateCR() {
        currentState = State.Charge;
        agent.speed = initialAgentSpeed * chargeSpeedFactor;
        AnimatorMove(true, chargeSpeedFactor);

        // tatakaeeeeeeeeeeee
        PlaySound(crySounds);
        if (player != null) {
            // get newPos close to player
            Vector3 origin = player.position;
            Vector3 newPos = RandomNavCircle(origin, 2, -1);

            // extend that so it run pass the player
            Vector3 delta = (newPos - transform.position);
            delta *= 1.7f;
            newPos = transform.position + delta;

            agent.SetDestination(newPos);
            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
        // if player is null skip

        ModifyNextState();
        StartNextState();
    }

    void ModifyNextState() {
        // if the state are repeat, have chance to modify it
        if (currentState == nextState) {
            float rand = Random.value;
            switch(currentState) {
            case State.Idle:
                if (rand < 0.5f) nextState = State.Wander;
                break;
            case State.Wander:
                if (rand < 0.8f) nextState = State.Idle;
                break;
            case State.Search:
                nextState = State.Wander;
                break;
            case State.Stalk:
                if (rand < 0.35f) nextState = State.Charge;
                break;
            case State.Charge:
                nextState = State.Stalk;
                break;
            } 
            // Debug.Log("BossState: " + currentState + " -> " + nextState);
        }
        // else, something already assign the next state
    }

    void StartNextState() {
        // Debug.Log("BossState: >" + nextState);
        switch(nextState) {
            case State.Idle:
                StartCoroutine(IdleStateCR());
                break;
            case State.Wander:
                StartCoroutine(WanderStateCR());
                break;
            case State.Search:
                StartCoroutine(SearchStateCR());
                break;
            case State.Stalk:
                StartCoroutine(StalkStateCR());
                break;
            case State.Charge:
                StartCoroutine(ChargeStateCR());
                break;
        } 
    }

    // ----------------------Util-------------------------
    //https://forum.unity.com/threads/solved-random-wander-ai-using-navmesh.327950/
    Vector3 RandomNavCircle(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitCircle * dist;

            randDirection += origin;

            NavMeshHit navHit;

            NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);

            return navHit.position;
    }

    // ----------------------Animator-------------------------
    void AnimatorMove(bool isMoving) {
        AnimatorMove(isMoving, 1);
    }

    void AnimatorMove(bool isMoving, float speed) {
        animator.SetBool("isMoving", isMoving);
        animator.speed = speed;
        decoyAnimator.SetBool("isMoving", isMoving);
        decoyAnimator.speed = speed;
        walkSoundEnable = isMoving;
        walkSoundFrequencyFactor = speed;
    }

    // ----------------------Gizmos-------------------------
    void OnDrawGizmos() {
        Vector3 pos = transform.position;
        pos.x += detectionOffset.x;
        pos.y += detectionOffset.y;
        Gizmos.color = new Color(255, 165, 0);
        Gizmos.DrawWireSphere(pos, soundDetectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, visionRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(pos, farRange);
    }

    // ----------------------Sound-------------------------
    void PlaySound(AudioClip[] sounds) {
        if (sounds == null || sounds.Length <= 0) return;
        SoundManager.PlaySound(SoundUtil.RandSound(sounds), transform.position);
    }

    IEnumerator WalkSoundCR() {
        while(true) {
            if (walkSoundEnable) PlaySound(walkSounds);
            if(SoundGraphManager.soundGraphManager != null) SoundGraphManager.TriggerSoundGraph(transform.position);
            yield return new WaitForSeconds(0.7f * (1 / walkSoundFrequencyFactor));
        }
    }

    // ----------------------Enable and Disable-------------------------
    void OnEnable() => instances.Add(this);
    void OnDisable() => instances.Remove(this);
}
