using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Golem : EnemyController
{
    public float kickForce = 15f;
    public GameObject Rock;
    public Transform handPos;
    public void KickOff()
    {
        if (attackTarget != null && transform.IsAttackArea(attackTarget.transform))
        {
            Vector3 dir = attackTarget.transform.position - transform.position;
            dir.Normalize();

            attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
            attackTarget.GetComponent<NavMeshAgent>().velocity = dir * kickForce;
            attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

            var target = attackTarget.GetComponent<CharacterData>();
            characterData.TakeDamage(characterData, target);
        }
    }

    public void ThrowRock()
    {
 
        var rock = Instantiate(Rock, handPos.position, Quaternion.identity);
        rock.GetComponent<Rock>().target = attackTarget;
        
    }
}
