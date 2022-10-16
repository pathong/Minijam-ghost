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

    [SerializeField] protected float attackRange;
    [SerializeField] private float attackTime = 1f;
    [SerializeField] private AudioClip walkSound;

    // setting
    [Header("Setting")]
    [SerializeField] private float soundDetectRange = 10f;    // Will go into State.Find if idle if sound is in this radius
    [SerializeField] private float visionRange = 5f;    // Will go into State.Stalk if see player in this range
    [SerializeField] private float farRange = 20f;
    [SerializeField] private Vector2 detectionOffset;
    //[SerializeField] private LayerMask visionMask;

    // lightning setting
    [Header("Lightning Setting")]
    [SerializeField] private Vector2 brightnessLerp = new Vector2(0, 1);
    [SerializeField] private Vector2 alphaLerp = new Vector2(0, 1);

    // ai
    private Vector2 interestPos;
    [SerializeField] private bool canSeePlayer = false;
    [Header("Behavior Setting")]
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderTimer;

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
        StopAllCoroutines();
        StartCoroutine(IdleStateCR());
        
        AnimatorMove(false);
        SyncLightning(0);
    }

    // ----------------------Update-------------------------
    void Update() {
        // detect player in view
        VisionUpdate();

        // if lightning, change from stalk or charge to wander

        // Debug
        if (Input.GetKeyDown(KeyCode.K)) nextState = State.Wander;
    }
    
    void VisionUpdate() {
        canSeePlayer = false;
        if (player == null) return;

        // check if player is out of range
        Vector2 _pos = (Vector2)transform.position + detectionOffset; // add offset
        Vector2 deltaPos = (Vector2)player.position - _pos;
        if (deltaPos.sqrMagnitude > Mathf.Pow(visionRange, 2)) return;
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
        canSeePlayer = true;
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
        }
    }

    public void TakeDamage() {
        
    }

    // IEnumerator IdleStateCR() {

    // }

    // ----------------------State Machine-------------------------
    IEnumerator IdleStateCR() {
        currentState = State.Idle;
        AnimatorMove(false, 1.25f);

        yield return new WaitForSeconds(Random.Range(0.4f, 2f));

        ModifyNextState();
        StartNextState();
    }

    IEnumerator WanderStateCR() {
        currentState = State.Wander;
        AnimatorMove(true);

        Vector3 newPos = RandomNavCircle(transform.position, wanderRadius, -1);
        agent.SetDestination(newPos);
        Debug.Log(newPos);
        yield return new WaitForSeconds(Random.Range(5f, 6f));

        ModifyNextState();
        StartNextState();
    }

    IEnumerator SearchStateCR() {
        currentState = State.Search;
        AnimatorMove(true, 1.25f);
        
        yield return new WaitForSeconds(Random.Range(5f, 5f));
        // go to interestPos
        // wait
        // wander
        
        ModifyNextState();
        StartNextState();
    }

    IEnumerator StalkStateCR() {
        currentState = State.Stalk;
        AnimatorMove(true);

        yield return new WaitForSeconds(Random.Range(0.4f, 2f));
        // keep distance but follow

        ModifyNextState();
        StartNextState();
    }

    IEnumerator ChargeStateCR() {
        currentState = State.Charge;
        AnimatorMove(true, 2);

        yield return new WaitForSeconds(Random.Range(2f, 2f));
        // tatakaeeeeeeeeeeee

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
                if (rand < 0.2f) nextState = State.Charge;
                break;
            case State.Charge:
                nextState = State.Stalk;
                break;
            } 
            Debug.Log("BossState: " + currentState + " -> " + nextState);
        }
        // else, something already assign the next state
    }

    void StartNextState() {
        StopAllCoroutines();
        Debug.Log("BossState: >" + nextState);
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
    }
    

    // ----------------------Enable and Disable-------------------------
    void OnEnable() => instances.Add(this);
    void OnDisable() => instances.Remove(this);
}
