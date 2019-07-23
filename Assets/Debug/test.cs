using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    private NavMeshAgent agent;
    
    [SerializeField]
    private float strength;

    private float attackCD = 0f;

    
    
    [SerializeField]
    private Rigidbody target;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!agent) return;
        if (!target) return;
        agent.SetDestination(target.position);
        if (Vector3.Distance(transform.position, target.position) <= agent.stoppingDistance * 0.7f)
        {
            Attack(target.GetComponent<Player>());
        }
    }

    private void Attack(Player target)
    {
        if (attackCD > 0) return;
        target.rb.AddForce(((target.rb.position - transform.position) * 3 + Vector3.up*6) * 10f, ForceMode.VelocityChange);
        target.TakeDamage(strength);
        StopCoroutine(CoolDownAttack());
        StartCoroutine(CoolDownAttack());
    }

    private IEnumerator CoolDownAttack()
    {
        attackCD = 2f;
        while (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
            yield return null;
        }
    }
    
    
}
