using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour, IDamageable
{
    private NavMeshAgent agent;
    
    [SerializeField]
    private float strength;

    private float attackCD = 0f;

    private float maxHp = 10, hp = 10;
    
    
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

        if (GetComponent<Rigidbody>()?.velocity.magnitude > 1f)
        {
            agent.enabled = false;
        }
        else
        {
            agent.enabled = true;
        }
    }

    private void Attack(Player target)
    {
        if (attackCD > 0) return;
        target.rb.AddForce(((target.rb.position - transform.position) * 3 + Vector3.up*6) * 10f * strength, ForceMode.VelocityChange);
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


    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            hp = 0;
            OnDeath();
        }
        GetComponentInChildren<SpriteRenderer>().size = new Vector2(hp/maxHp, 1);
    }

    public void OnDeath()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        float t = 5;
        GetComponent<Rigidbody>().maxAngularVelocity = 1000000;
        GetComponent<Rigidbody>().angularVelocity = new Vector3(100000, 100000, 100000);
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
