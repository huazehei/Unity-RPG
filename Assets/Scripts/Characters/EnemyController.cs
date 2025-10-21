
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState{Guard,Patrol,Chase,Dead}

[RequireComponent(typeof(NavMeshAgent))] //没有该组件时会自动添加
public class EnemyController : MonoBehaviour,IEndGameObserver
{
    private NavMeshAgent agent;
    private EnemyState enemyState;
    private Animator anim;
    protected CharacterStats characterStats;
    private Collider coll;
    private bool isWin;

    [Header("Basice Setting")] 
    public float sightRadius;
    protected GameObject attackTarget;
    private float speed;
    public bool isGuard;
    
    //到达某一位置的巡视时间
    public float lookAtTime;
    private float remainLookAtTime;
    private float lastAttackTime;

    [Header("Patrol Setting")] 
    public float patrolRange;
    
    private Vector3 wayPoint;

    private Vector3 guardPos;

    private Quaternion guardRot;
    //通过bool将代码与anim关联
    private bool isWalk, isChase, isFollow,isDead;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        characterStats = GetComponent<CharacterStats>();
        coll = GetComponent<BoxCollider>();
        
        speed = agent.speed;
        guardPos = transform.position;
        guardRot = transform.rotation;

        remainLookAtTime = lookAtTime;
    }

    private void Start()
    {
        if (isGuard)
        {
            enemyState = EnemyState.Guard;
        }
        else
        {
            enemyState = EnemyState.Patrol;
            GetRandomWayPoint();
        }
        //FIXME:在场景切换后修改
        //GameManager.Instance.AddObserver(this);
    }
    //虽然都是继承了 Singleton<T> 的单例模式，但是在 Awake 中实例化仍然有顺序，
    //Unity 会逐一运行每一个物体代码的 Awake 再逐一执行每一个物体代码的 Start 。这导致在注册观察者模式中遇到问题
    //需修改代码执行顺序，project setting -- script excute order，小于default就行
    // 场景切换时启用
    // 观察者模式，订阅与取消订阅
     void OnEnable()
     {
         GameManager.Instance.AddObserver(this);
     }

    void OnDisable()
    {
        if (!GameManager.IsInitialized()) return;
        GameManager.Instance.RemoveObserver(this);
        
        if(GetComponent<LootSpawner>() && isDead)
            GetComponent<LootSpawner>().SpwanLoot();
        
        //敌人死亡检测任务进度
        if (QuestManager.IsInitialized() && isDead)
        {
            QuestManager.Instance.UpdateQuestProgress(this.name,1);
        }
    }

    void Update()
    {
        if (characterStats.CurrentHealth == 0)
            isDead = true;
        if (!isWin)
        {
            SwitchState();
            SwitchAnim();
            lastAttackTime -= Time.deltaTime;
        }
    }

    void SwitchAnim()
    {
        anim.SetBool("Walk",isWalk);
        anim.SetBool("Chase",isChase);
        anim.SetBool("Follow",isFollow);
        anim.SetBool("Critical",characterStats.isCritical);
        anim.SetBool("Death",isDead);
    }
    void SwitchState()
    {
        //如果死亡就切换到Dead
        if (isDead)
            enemyState = EnemyState.Dead;
        //如果发现Player,就切换到Chase
        else if (FoundPlayer())
        {
            enemyState = EnemyState.Chase;
            //Debug.Log("找到Player了");
        }
            
        
        switch (enemyState)
        {
            case EnemyState.Guard:
                isChase = false;
                
                if (transform.position != guardPos)
                {
                    isWalk = true;
                    agent.isStopped = false;
                    agent.destination = guardPos;
                    if (Vector3.Magnitude(guardPos - transform.position ) <= agent.stoppingDistance)
                    {
                        isWalk = false;
                        transform.rotation = Quaternion.Lerp(transform.rotation,guardRot,0.05f);
                    }
                }
                break;
            case EnemyState.Patrol:
                isChase = false;
                agent.speed = speed * 0.5f;
                //判断是否到达巡逻点
                if (Vector3.Distance(wayPoint, transform.position) <= agent.stoppingDistance)
                {
                    isWalk = false;
                    if (remainLookAtTime > 0)
                        remainLookAtTime -= Time.deltaTime;
                    else
                        GetRandomWayPoint();
                }
                else
                {
                    isWalk = true;
                    agent.destination = wayPoint;
                }
                break;
            case EnemyState.Chase:
                isWalk = false;
                isChase = true;
                agent.speed = speed;
                if (!FoundPlayer())
                {
                    isFollow = false;
                    
                    //脱离攻击范围后回到上一个状态
                    if (remainLookAtTime > 0)
                    {
                        agent.destination = transform.position;
                        remainLookAtTime -= Time.deltaTime;
                    }
                    else if (isGuard)
                        enemyState = EnemyState.Guard;
                    else
                        enemyState = EnemyState.Patrol;
                }
                else
                {
                    isFollow = true;
                    agent.isStopped = false;
                    agent.destination = attackTarget.transform.position;
                }
                //追到并攻击
                if (AttackRange()||SkillRange())
                {
                    isFollow = false;
                    agent.isStopped = true;
                    if (lastAttackTime < 0)
                    {
                        lastAttackTime = characterStats.attackData.coolDown;
                        //暴击判断并执行攻击
                        characterStats.isCritical = Random.value < characterStats.attackData.criticalChance;
                        Attack();
                    }
                }
                break;
            case EnemyState.Dead:
                coll.enabled = false;
                //agent.enabled = false;
                agent.radius = 0;
                
                Destroy(gameObject,2f);
                break;
        }
    }
    //判断是否在攻击距离内
    bool AttackRange()
    {
        if (FoundPlayer())
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <
                   characterStats.attackData.attackRange;
        }
        else
            return false;
    }

    bool SkillRange()
    {
        if (FoundPlayer())
        {
            return Vector3.Distance(attackTarget.transform.position, transform.position) <
                   characterStats.attackData.skillRange;
        }
        else
            return false;
    }

    void Attack()
    {
        transform.LookAt(attackTarget.transform.position);
        if (AttackRange())
        {
            //执行近战攻击
            anim.SetTrigger("Attack");
        }

        if (SkillRange())
        {
            //执行远程攻击
            anim.SetTrigger("Skill");
        }
    }
    void GetRandomWayPoint()
    {
        //重新开始站桩时间
        remainLookAtTime = lookAtTime;
        //获取随机巡逻点
        float RandomX = Random.Range(-patrolRange, patrolRange);
        float RandomZ = Random.Range(-patrolRange, patrolRange);
        Vector3 randomVector = new Vector3(guardPos.x + RandomX, transform.position.y, guardPos.z + RandomZ);
        //确保随机巡逻点不会出现在不可移动区域
        //NavMeshHit hit;
        wayPoint = NavMesh.SamplePosition(randomVector,out NavMeshHit hit,patrolRange,1)?randomVector:transform.position;
    }
    bool FoundPlayer()
    {
        var colliders = Physics.OverlapSphere(transform.position, sightRadius);
        foreach (Collider target in colliders)
        {
            if (target.CompareTag("Player"))
            {
                attackTarget = target.gameObject;
                return true;
            }
        }

        attackTarget = null;
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,sightRadius);
    }
    //Animation Event
    void Hit()
    {
        if (attackTarget != null&&transform.isForward(attackTarget.transform)&&enemyState!=EnemyState.Dead)
        {
            CharacterStats targetStats = attackTarget.GetComponent<CharacterStats>();
            targetStats.TakeDamage(characterStats,targetStats);
        }
    }

    public void EndNotify()
    {
        //胜利动画
        isWin = true;
        anim.SetBool("Win",true);
        //停止移动
        isChase = false;
        isWalk = false;
        //关闭agent
        attackTarget = null;
    }
}
