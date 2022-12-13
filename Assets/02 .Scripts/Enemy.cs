using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    // ���� ǥ���� ���� ������ ���� ����
    public enum State  
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state = State.IDLE;

    public float attackDist = 2f; // ���� �Ÿ�
    public float traceDist = 5f; // ���� �Ÿ�
    public float moveSpeed = 2f;
    private float damping = 1f; // ȸ�� �ӵ� ���� ����

    private Transform playerTr; // �÷��̾� ��ġ
    private Transform enemyTr; // �� ��ġ
    private Animator anim;
    private WaitForSeconds ws;
    private NavMeshAgent agent;
    private Rigidbody rigid;

    public bool isDie = false;

    private Vector3 _traceTarget; // ���� ��� ��ġ ���� ����
    
    
    public Vector3 traceTarget
    {
        get {  return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = moveSpeed;
            damping = 7f;
            TraceTarget(_traceTarget);
        }
    }
    public float speed
    {
        get { return agent.velocity.magnitude; }
    }

    private readonly int hashMove = Animator.StringToHash("isMove");
    private readonly int hashSpeed = Animator.StringToHash("speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashOffset = Animator.StringToHash("Offset");

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {
            playerTr = player.GetComponent<Transform>();
        }
        ws = new WaitForSeconds(0.3f);
        enemyTr = GetComponent<Transform>();
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rigid.sleepThreshold = 0.0f;
        agent.updateRotation = false;
        agent.autoBraking = false;
        agent.speed = moveSpeed;

    }
    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());

    }
    // ���� �˻� �ڷ�ƾ (0.3�ʸ��� �˻�)

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == State.DIE) // ������¸� �ڷ�ƾ ����
                yield break;

            // �÷��̾� - �� ���� �Ÿ� ���
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);

            if(dist <= attackDist)  // ���� �Ÿ� �̳��� ��
            {
                state = State.ATTACK;
            }
            else if(dist <= traceDist) // ���� �Ÿ� �̳��� ��
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }

            yield return ws;
        }
    }
    // ���¿� ���� �ൿ�� ó���ϴ� �ڷ�ƾ �Լ�
    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return ws;

            switch (state)
            {
                case State.IDLE:
                    Stop();
                    anim.SetBool(hashMove, false);
                    break;
                case State.TRACE:
                    traceTarget = playerTr.position;
                    anim.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                    anim.SetTrigger("Attack");
                    Stop();
                    anim.SetBool(hashMove, false);
                    break;
                case State.DIE:
                    this.gameObject.tag = "Untagged";
                    isDie = true;
                    Stop();
                    anim.SetBool(hashDie, true);
                    break;
            }
        } 
         
    }
    public void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;


    }
    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
    }
    public void StartMove()
    {
        agent.isStopped = false;
    }
    void Update()
    {
        if (!agent.isStopped)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
    }

    private void OnMouseOver()
    {
        playerTr.GetComponent<PlayerBehaviour>().opponent = this.gameObject;
        Cursor.SetCursor(GameManager.instance.cursers[1], Vector2.zero, CursorMode.Auto);
    }
    private void OnMouseExit()
    {
        Cursor.SetCursor(GameManager.instance.cursers[0], Vector2.zero, CursorMode.Auto);

    }

}
