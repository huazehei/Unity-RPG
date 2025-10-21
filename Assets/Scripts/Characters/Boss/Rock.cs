using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rock : MonoBehaviour
{
    public enum RockStates{HitPlayer,HitEnemy,HitNothing}
    private Rigidbody rb;
    public RockStates rockState;
    [Header("Basic Setting")]
    public float force = 15;
    private Vector3 direction;
    public int damage = 8;
    public GameObject target;
    public GameObject breakingEffect;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one;
        RocktoTarget();
        rockState = RockStates.HitPlayer;
    }

    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            rockState = RockStates.HitNothing;
        }
    }

    public void RocktoTarget()
    {
        if (target == null)
            target = FindFirstObjectByType<PlayerController>().gameObject;
        direction = (target.transform.position - transform.position + Vector3.up).normalized;
        rb.AddForce(direction * force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (rockState)
        {
            case RockStates.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage,other.gameObject.GetComponent<CharacterStats>());
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    rockState = RockStates.HitNothing;
                }
                break;
            case RockStates.HitEnemy:
                if (other.gameObject.GetComponent<Golem>())
                {
                    var otherState = other.gameObject.GetComponent<CharacterStats>();
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damage,otherState);
                    Instantiate(breakingEffect, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }

                break;
        }
    }
}
