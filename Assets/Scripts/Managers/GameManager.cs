using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStates;
    private CinemachineFreeLook cinemachineFreeLook;
    List<IEndGameObserver> observers = new();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(CharacterStats stats)
    {
        playerStates = stats;
        cinemachineFreeLook = FindFirstObjectByType<CinemachineFreeLook>();
        if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.Follow = playerStates.transform.GetChild(2);
            cinemachineFreeLook.LookAt = playerStates.transform.GetChild(2);
        }
    }

    public void AddObserver(IEndGameObserver observer)
    {
        observers.Add(observer);
    }
    public void RemoveObserver(IEndGameObserver observer)
    {
        observers.Remove(observer);
    }
    //PlayerController.Update -- GameManger.NotifyObservers -- IEndGameObserver.EndNotify
    public void NotifyObservers()
    {
        foreach (var observer in observers)
        {
            observer.EndNotify();
        }
    }

    public Transform GetEntrances()
    {
        foreach (var item in FindObjectsByType<TransitionDestination>(FindObjectsSortMode.None))
        {
            if (item.destinationType == TransitionDestination.DestinationType.ENTER)
            {
                return item.transform;
            }
        }

        return null;
    }
}
