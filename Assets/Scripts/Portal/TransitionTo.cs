using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTo : MonoBehaviour
{
    public string sceneName;
    public enum SceneType { SameScene, DifferentScene}

    public SceneType sceneType;

    public TransitionDestination.TransitionType destination;

    bool canTran;
    bool GetE;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canTran)
            GetE = true;
    }

    private void FixedUpdate()
    {
        if(GetE)
        {
            SceneController.Instance.OnTransition(this);
            GetE = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            canTran = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
            canTran = false;
    }
}
