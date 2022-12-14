using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : MonoBehaviour, IDamangable
{
    public enum EnemyState { Idle, Chasing, Attacking, Dead}
    public EnemyState currentState;


    [SerializeField] protected float attackRange;
    [SerializeField] private float attackTime = 1f;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private ParticleSystem smokePar;
    public int MaxHealth;
    private int currentHealth;

    private NavMeshAgent agent;

    public Transform player;

    private bool isAttacking;
    

    // Start is called before the first frame update
    void Awake()
    {
        currentState = EnemyState.Idle;
        currentHealth = MaxHealth;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;


        player = Extension.GetPlayer();

    }

    private void Start()
    {
        Instantiate(smokePar, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
    }


    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                if(player!= null) { currentState = EnemyState.Chasing; StartCoroutine(nameof(WalkSound)); }
                break;
            case EnemyState.Chasing:
                agent.SetDestination(player.position);
                
                if(Vector2.Distance(player.position, this.transform.position) <= attackRange)
                {
                    currentState = EnemyState.Attacking;
                    agent.ResetPath();
                }
                break;
            case EnemyState.Attacking:
                if (isAttacking) { return; }
                StartCoroutine(nameof(AttackDelayTest));
                break;
            case EnemyState.Dead:
                agent.ResetPath();
                break;
        }
    }
    protected virtual void Attack()
    {
        // attack (start animation)
        
    }

    public void OnAttackAnimationFinished()
    {
        isAttacking = false;
        // Check next state after attack animation is finished.
        if(Vector2.Distance(player.position, this.transform.position) > attackRange)
        {
            currentState = EnemyState.Chasing;
            StartCoroutine(nameof(WalkSound));
        }
        else
        {
            currentState = EnemyState.Attacking;
        }
    }

    IEnumerator AttackDelayTest()
    {
        isAttacking = true;
        
        Attack();
        yield return new WaitForSeconds(attackTime);
        OnAttackAnimationFinished();
    }

    public void TakeDamage()
    {
        currentHealth -= 1;
        if(currentHealth <= 0) {
            currentState = EnemyState.Dead;
            Instantiate(smokePar, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
            Destroy(gameObject); 
        } //TODO : maybe do something else
    }

    IEnumerator WalkSound()
    {
        while(currentState == EnemyState.Chasing)
        {
            float wait = 1f;
            SoundManager.PlaySound(walkSound, transform.position, .5f);
            SoundGraphManager.TriggerSoundGraph(transform.position);
            yield return new WaitForSeconds(wait);
            
        }
    }
}

