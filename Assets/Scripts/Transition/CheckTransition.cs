using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class CheckTransition : MonoBehaviour
{
    public float checkLength = 15;

    public GameObject player;

    private GameObject _target;

    public LineRenderer lineTrans;

    private bool _isTransition;
    private bool _canTransition = true;
    
    public Vector3 rate;

    private void Update()
    {
        if(_canTransition)
            CheckTrans();
        if (Input.GetKeyDown(KeyCode.T))
            _isTransition = true;
    }

    private void FixedUpdate()
    {
        if (_isTransition && _target != null && _canTransition)
        {
            _canTransition = false;
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        player.GetComponent<NavMeshAgent>().enabled = false;
        lineTrans.SetPosition(0, _target.transform.position);
        while (Vector3.Distance(player.transform.position, _target.transform.position) > 0.1f)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, _target.transform.position, 0.02f);
            lineTrans.SetPosition(1, player.transform.position);
            yield return null;
        }
        player.GetComponent<NavMeshAgent>().enabled = true;
        _canTransition = true;
        _isTransition = false;
    }

    public void CheckTrans()
    {
        Vector3 centerPos = transform.position + transform.forward * checkLength * 0.5f;

        var cols = Physics.OverlapSphere(centerPos, rate.x * checkLength,
            1 << LayerMask.NameToLayer("Transition"));
        foreach (var col in cols)
        {
            _target = col.gameObject;
            break;
        }
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     // Gizmos.DrawCube(transform.position + transform.forward * checkLength * 0.5f, rate * checkLength * 2);
    //     Gizmos.DrawSphere(transform.position + transform.forward * checkLength * 0.5f, rate.x * checkLength);
    // }
}
