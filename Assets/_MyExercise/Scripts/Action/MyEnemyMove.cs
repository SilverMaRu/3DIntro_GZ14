using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.Others;
using System;

public class MyEnemyMove : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Transform playerTrans;
    private Collider coll;

    // Start is called before the first frame update
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        playerTrans = GameObject.FindGameObjectWithTag("Player")?.transform;
        coll = GetComponent<Collider>();

        EventManager.BindingEvent<GameObject>("CharacterDie", OnEnemyDie);
    }

    // Update is called once per frame
    void Update()
    {
        ResetDestination();
        SendEventMessage();
    }

    private void ResetDestination()
    {
        if (navAgent.enabled && !navAgent.isStopped)
        {
            navAgent.SetDestination(playerTrans.position);
        }
    }

    private void SendEventMessage()
    {
        if (navAgent.enabled && !navAgent.isStopped && navAgent.remainingDistance > navAgent.stoppingDistance)
        {
            EventManager.OnEvent("CharacterMove", gameObject);
        }
        else
        {
            EventManager.OnEvent("CharacterIde", gameObject);
        }
    }

    private void OnEnemyDie(GameObject deathCharacterGameObj)
    {
        if (deathCharacterGameObj == gameObject)
        {
            navAgent.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            navAgent.isStopped = true;
            EventManager.OnEvent("HitCharacter", gameObject, collision);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            navAgent.isStopped = false;
        }
    }

}
