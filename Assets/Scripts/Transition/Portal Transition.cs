using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTransition : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }

    [Header("Transition Info")] 
    public string sceneName;
    public TransitionType transitionType;
    public TransitionDestination.DestinationType destinationPoint;
    private bool isTrans;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isTrans)
        {
            //通过SceneController实现传送
            SceneController.Instance.TransitionToDesitination(this);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            isTrans = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isTrans = false;
    }
}
