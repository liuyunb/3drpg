using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyStatas { GUARD , PATROL ,CHASE ,DEAD}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterData))]
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private NavMeshAgent agent;
    private Animator anim;
    protected CharacterData characterData;

    public EnemyStatas enemyStata;

    public float sightRadius;
    public float patrolRadius;
    public float lookAtTime;

    private Vector3 guardPos;
    private Quaternion guardRotation;
    protected GameObject attackTarget;
    private float speed;
    private Vector3 wayPoint;
    private float remainLookAtTime;
    private float lastAttackTime;
    private bool playerDead;

    public bool isGuard;

    private bool isWalk;
    private bool isChase;
    private bool isFollow;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterData = GetComponent<CharacterData>();
        speed = agent.speed;
        guardPos = transform.position;
        remainLookAtTime = lookAtTime;
        guardRotation = transform.rotation;
    }

    private void Start()
    {
        if (isGuard)
            enemyStata = EnemyStatas.GUARD;
        else
            enemyStata = EnemyStatas.PATROL;
        GetNewPoint();
        GameManager.Instance.RegisterObserver(this);
    }

    private void Update()
    {
        if(!playerDead)
        {
            SwitchStata();
            SetAnim();
        }
    }

    private void OnDisable()
    {
        if (!GameManager.isInitialized) return;
        GameManager.Instance.RemoveObserver(this);
        if(GetComponent<LootSpawner>() && characterData.isDead)
        {
            GetComponent<LootSpawner>().SpawnLoot();
        }
        if (QuestManager.isInitialized && characterData.isDead)
            QuestManager.Instance.UpdateQuestProcess(this.name,1);
    }

    public void SetAnim()
    {
        anim.SetBool("walk", isWalk);
        anim.SetBool("chase", isChase);
        anim.SetBool("follow", isFollow);
        anim.SetBool("critical", characterData.isCritical);
        anim.SetBool("dead", characterData.isDead);
    }

    public void SwitchStata()
    {
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;


        if(characterData.isDead)
        {
            enemyStata = EnemyStatas.DEAD;
        }
        else if(FoundPlayer())
        {
            enemyStata = EnemyStatas.CHASE;
            isWalk = false;
            isChase = true;
        }


        switch(enemyStata)
        {
            case EnemyStatas.GUARD:
                Guard();
                break;
            case EnemyStatas.PATROL:
                Patrol();
                break;
            case EnemyStatas.CHASE:
                Chase();
                break;
            case EnemyStatas.DEAD:
                Dead();
                break;
        }
    }

    public void Guard()
    {
        isChase = false;
        if(transform.position != guardPos)
        {
            agent.destination = guardPos;
            if(Vector3.SqrMagnitude(transform.position - guardPos) <= agent.stoppingDistance)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, guardRotation, 0.01f);
            }
        }
    }
    public void Patrol()
    {
        isChase = false;
        isWalk = true;
        agent.speed = speed * 0.5f;
        if(Vector3.Distance(wayPoint,transform.position) <= agent.stoppingDistance)
        {
            isWalk = false;
            if (remainLookAtTime > 0)
                remainLookAtTime -= Time.deltaTime;
            else
                GetNewPoint();
        }
        else
            agent.destination = wayPoint;
    }
    public void Chase()
    {

        agent.isStopped = false;

        if(!FoundPlayer())
        {
            //拉脱战
            if (remainLookAtTime > 0)
                remainLookAtTime -= Time.deltaTime;
            else if (isGuard)
                enemyStata = EnemyStatas.GUARD;
            else if (!isGuard)
                enemyStata = EnemyStatas.PATROL;
            agent.destination = transform.position;
            isChase = true;
            isFollow = false;
        }
        else
        {
            isFollow = true;
            agent.speed = speed;
            agent.destination = attackTarget.transform.position;

            if (TakeInAttack() || TakeInSkill())
            {
                isFollow = false;
                agent.isStopped = true;

                if (lastAttackTime <= 0)
                {
                    lastAttackTime = characterData.cd;
                    transform.LookAt(attackTarget.transform.position);
                    characterData.isCritical = Random.value <= characterData.criticalChance;


                    if (TakeInAttack())
                    {
                        //普通攻击
                        anim.SetTrigger("attack");
                    }
                    if (TakeInSkill())
                    {
                        //技能攻击
                        anim.SetTrigger("skill");
                    }

                }
            }
        }



    }
    public void Dead()
    {
        agent.radius = 0;
        GetComponent<Collider>().enabled = false;
        GameManager.Instance.RemoveObserver(this);
        Destroy(gameObject,2f);
    }

    public bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);

        foreach (var target in colliders)
        {
            if(target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                remainLookAtTime = lookAtTime;
                return true;
            }
        }
        attackTarget = null;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guardPos, patrolRadius);
    }

    public void GetNewPoint()
    {
        remainLookAtTime = lookAtTime;
        float randomX = Random.Range(-patrolRadius, patrolRadius);
        float randomZ = Random.Range(-patrolRadius, patrolRadius);

        Vector3 randomPos = new Vector3(guardPos.x + randomX, transform.position.y, guardPos.z + randomZ);

        NavMeshHit hit;

        wayPoint = NavMesh.SamplePosition(randomPos, out hit, patrolRadius, 1) ? hit.position : transform.position;
    }

    public bool TakeInAttack()
    {
        if (Vector3.Distance(attackTarget.transform.position, transform.position) < characterData.attackRange)
            return true;
        return false;
    }

    public bool TakeInSkill()
    {
        if (Vector3.Distance(attackTarget.transform.position, transform.position) < characterData.skillRange)
            return true;
        return false;
    }

    public void Hit()
    {
        if(attackTarget != null)
        {
            var target = attackTarget.GetComponent<CharacterData>();
            characterData.TakeDamage(characterData, target);
        }
    }

    public void EndNotify()
    {
        //实现动画
        isChase = false;
        isWalk = false;
        playerDead = true;
        anim.SetBool("Win", true);
        //停止移动
        agent.isStopped = true;
    }
}
