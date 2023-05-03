using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : SingleTon<GameManager>
{
    public CharacterData playerData;

    CinemachineFreeLook followLook;


    List<IEndGameObserver> observers = new List<IEndGameObserver>();


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RegisterPlayer(CharacterData playerData)
    {
        this.playerData = playerData;
        followLook = FindObjectOfType<CinemachineFreeLook>();
        if(followLook != null)
        {
            followLook.Follow = playerData.transform;
            followLook.LookAt = playerData.transform;
        }

    }

    public void RegisterObserver(IEndGameObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        observers.Remove(observer);
    }

    public void EndGame()
    {
        foreach (IEndGameObserver observer in observers)
        {
            observer.EndNotify();
        }
    }
}
