using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public float findDist = 6f; // 적 찾는 거리
    private float dist;
    public float attackDist = 2f; // 공격 사거리
    public GameObject enemy;
    public GameObject opponent;
    internal Animator anim;
    internal ClickToMove cm;

    private float attackCounter;

    public bool isDie;
    [SerializeField] internal bool isAttacking;

    private Transform enemyTr; // 적 위치



    [SerializeField] private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            TraceTarget(_traceTarget);
        }
    }
    public void TraceTarget(Vector3 pos)
    {
        if (cm.agent.isPathStale) return;

        cm.agent.destination = pos;
        cm.agent.isStopped = false;
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cm = GetComponent<ClickToMove>();

        enemy = GameObject.FindGameObjectWithTag("Enemy");

        if (enemy != null)
        {
            enemyTr = enemy.GetComponent<Transform>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(enemyTr.position, transform.position);

        if (Input.GetKeyDown(KeyCode.A))
        {
            isAttacking = true;
        }
        if (Input.GetMouseButtonDown(0) && isAttacking)
        {
            AttackPosition();
        }

        if (dist <= 3 && isAttacking)
        {
            cm.StopMove();
            transform.LookAt(traceTarget);
            anim.SetBool(AnimatorParameters.hashAttacking, true);
        }
    }
    void AttackPosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100))
        {
            AttackDestination(hit.point);

            //if (hit.collider.gameObject.tag == "Enemy")
            //{
            //    traceTarget = enemyTr.position;
            //    hit.point = traceTarget;
            //    AttackDestination(hit.point);
            //}
        }
    }
    void AttackDestination(Vector3 _destination)
    {
        cm.StartMove();
        cm.agent.SetDestination(_destination);

    }


    //void Attack(Vector3 _destination)
    //{
    //    StartCoroutine(AutoAttackCheck());

    //    cm.StartMove();
    //    traceTarget = enemyTr.position;


    //    if (state == AutoAttackState.ATTACK)
    //    {
    //        _destination = traceTarget;
    //        cm.agent.SetDestination(_destination);
    //        cm.StopMove();
    //        Debug.Log("Attack");
    //    }
    //    if(state == AutoAttackState.TRACE)
    //    {
    //        _destination = traceTarget;
    //        cm.agent.SetDestination(_destination);
    //        Debug.Log("Trace");
    //    }
    //    else
    //    {
    //        cm.agent.SetDestination(_destination);

    //    }


    //}
    //void Attack(Vector3 destination)
    //{
    //    float dist = Vector3.Distance(transform.position, enemyTr.transform.position);

    //    readyToAttack = false;
    //    cm.StartMove();

    //    if (dist <= attackDist)
    //    {
    //        state = State.ATTACK;
    //        Debug.Log("Attack!");
    //        cm.StopMove();
    //    }

    //    if (dist <= findDist)
    //    {
    //        state = State.TRACE;
    //        Debug.Log("Trace!");
    //        traceTarget = enemyTr.position;

    //        destination = traceTarget;
    //        cm.agent.SetDestination(destination);
    //    }
    //    else
    //    {
    //        state = State.IDLE;
    //        cm.agent.SetDestination(destination);
    //    }
    //}

    //IEnumerator AutoAttackCheck()
    //{
    //    while (!isDie)
    //    {
    //        if (state == AutoAttackState.DIE)
    //            yield break;

    //        float dist = Vector3.Distance(transform.position, enemyTr.transform.position);

    //        if(dist <= attackDist)
    //        {
    //            state = AutoAttackState.ATTACK;
    //        }
    //        else if(dist <= findDist)
    //        {
    //            state = AutoAttackState.TRACE;
    //        }
    //        else
    //        {
    //            state = AutoAttackState.LOCOMOTION;
    //        }

    //        yield return new WaitForSeconds(0.3f);
    //    }
    //}
}
public static partial class AnimatorParameters
{
    public static int hashMove = Animator.StringToHash("isMove");
    public static int hashAttack = Animator.StringToHash("NormalAttack");
    public static int hashAttacking = Animator.StringToHash("isAttack");
}
