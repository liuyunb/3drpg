using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RockStatas { HitPlayer,HitEnemy,HitNothing}
public class Rock : MonoBehaviour
{
    Rigidbody rb;
    public GameObject target;
    public float throwForce = 20f;
    public float kickFoece = 15f;
    public RockStatas rockStata;
    public GameObject breakEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rockStata = RockStatas.HitPlayer;
    }
    private void Start()
    {
        FlyToTarget();
        rb.velocity = Vector3.one;
    }

    private void FixedUpdate()
    {
        if(Vector3.SqrMagnitude(rb.velocity) < 1f)
        {
            rockStata = RockStatas.HitNothing;
        }    
    }

    public void FlyToTarget()
    {
        if (target == null)
            target = FindObjectOfType<PlayerController>().gameObject;
        Vector3 dir = target.transform.position - transform.position + Vector3.up;
        dir.Normalize();

        rb.AddForce(dir * throwForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(rockStata)
        { 
            case RockStatas.HitPlayer:
                HitPlayer();
                break;
            case RockStatas.HitEnemy:
                HitEnemy(collision);
                break;
            case RockStatas.HitNothing:
                HitNothing();
                break;
        }
    }

    public void HitPlayer()
    {
        Vector3 dir = target.transform.position - transform.position + Vector3.up;
        dir.Normalize();

        target.GetComponent<NavMeshAgent>().isStopped = true;
        target.GetComponent<NavMeshAgent>().velocity = dir * kickFoece;

        target.GetComponent<Animator>().SetTrigger("Dizzy");
        target.GetComponent<CharacterData>().TakeDamage(10, target.GetComponent<CharacterData>());

        rockStata = RockStatas.HitNothing;

    }
    public void HitEnemy(Collision collision)
    {
        if(collision.gameObject.GetComponent<Golem>())
        {
            collision.gameObject.GetComponent<CharacterData>().TakeDamage(10, collision.gameObject.GetComponent<CharacterData>());
            Instantiate(breakEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void HitNothing()
    {

    }
}
