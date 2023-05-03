using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private CharacterData characterData;

    private GameObject attackTarget;


    private float lastAttackTime;
    private float attackCD;
    [SerializeField]
    private bool canDash;
    public float dashSpeed = 10f;
    private float oriSpeed;
    private float dashStartTime;
    public float dashTime = 0.5f;
    private float dashingTime = 0f;

    public Image dashMask;
    

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterData = GetComponent<CharacterData>();
        attackCD = characterData.cd;
        oriSpeed = agent.speed;
    }

    private void OnEnable()
    {
        MouseManager.Instance.OnMouseClick += MoveToTarget;
        MouseManager.Instance.OnEnemyAttack += EventAttack;
        GameManager.Instance.RegisterPlayer(characterData);
    }

    private void Start()
    {
        SaveManager.Instance.LoadPlayerData();
        dashMask.fillAmount = 0;
    }

    private void OnDisable()
    {
        MouseManager.Instance.OnMouseClick -= MoveToTarget;
        MouseManager.Instance.OnEnemyAttack -= EventAttack;
    }

    private void Update()
    {
        SetAnim();
        if(lastAttackTime > 0)
            lastAttackTime -= Time.deltaTime;
        Dead();
        if (Input.GetKeyDown(KeyCode.P) && !canDash)
            ReadyToDash();
    }

    private void FixedUpdate()
    {
        if (canDash)
            Dash();
    }

    private void Dash()
    {
        var shadow = ShadowPaul.Instance.PopPaul();
        shadow.transform.position = transform.position;
        dashingTime += Time.fixedDeltaTime;
        if (dashingTime >= dashTime)
        {
            StopDash();
        }
        dashMask.fillAmount = 1 - dashingTime / dashTime;
    }

    private void StopDash()
    {
        agent.destination = transform.position;
        agent.speed = oriSpeed;
        canDash = false;
    }

    private void ReadyToDash()
    {
        print(transform.forward * dashSpeed * dashTime);
        dashMask.fillAmount = 1;
        agent.isStopped = false;
        canDash = true;
        dashStartTime = Time.time;
        agent.destination += transform.forward * dashSpeed * dashTime;
        agent.speed = dashSpeed;
        dashingTime = 0;
    }

    public void SetAnim()
    {
        anim.SetFloat("Speed", agent.velocity.sqrMagnitude);
        anim.SetBool("Dead", characterData.isDead);
    }

    public void MoveToTarget(Vector3 destination)
    {
        if (characterData.isDead)
            return;
        //StopCoroutine(MoveToAttack());
        //StopCoroutine("MoveToAttack");
        StopAllCoroutines();
        agent.isStopped = false;
        agent.destination = destination;
    }

    public void EventAttack(GameObject target)
    {
        if (characterData.isDead)
            return;
        if(target != null)
        {
            attackTarget = target;
            characterData.isCritical = Random.value <= characterData.criticalChance;
            StartCoroutine(MoveToAttack());
        }
    }

    IEnumerator MoveToAttack()
    {
        agent.isStopped = false;


        //TODO:
        while(Vector3.Distance(transform.position,attackTarget.transform.position) > characterData.attackRange)
        {
            agent.destination = attackTarget.transform.position;
            yield return null;
        }
        transform.LookAt(attackTarget.transform);
        //Attack

        agent.isStopped = true;

        if (lastAttackTime <= 0)
        {
            lastAttackTime = attackCD;
            anim.SetBool("Critical", characterData.isCritical);
            print(characterData.isCritical);
            anim.SetTrigger("Attack");
        }

    }

    public void Hit()
    {

        if(attackTarget.CompareTag("Attackable"))
        {
            if (attackTarget.GetComponent<Rock>())
            {
                attackTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
                attackTarget.GetComponent<Rigidbody>().velocity = Vector3.one;
                attackTarget.GetComponent<Rock>().rockStata = RockStatas.HitEnemy;
            }
        }
        else
        {
            var target = attackTarget.GetComponent<CharacterData>();
            characterData.TakeDamage(characterData, target);
        }


    }

    public void Dead()
    {
        if(characterData.isDead)
            GameManager.Instance.EndGame();
    }
}
