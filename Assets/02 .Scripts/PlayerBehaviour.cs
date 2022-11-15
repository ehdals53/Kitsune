using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject opponent;

    internal Animator anim;
    internal ClickToMove cm;

    public float attackDelay = 0.5f;
    private float attackCounter;

    [SerializeField] internal bool isAttacking;
    [SerializeField] internal bool lockMovement = false;
    [SerializeField] internal bool lockRotation = false;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cm = GetComponent<ClickToMove>();
    }

    // Update is called once per frame
    void Update()
    {
        AttackInput();
        ControlAttackBehaviour();

    }


    //protected virtual bool AttackConditions()
    //{
    //    return
    //}
    protected virtual void ControlAttackBehaviour()
    {
        if (!isAttacking) return;

        attackCounter -= Time.deltaTime;
        
        if(attackCounter <= 0)
        {
            attackCounter = 0;
            isAttacking = false;
            lockRotation = false;
            lockMovement = false;
            //anim.ResetTrigger(AnimatorParameters.hashAttack);
        }
    }
    protected virtual void AttackInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    public virtual void Attack()
    {

        if (!isAttacking)
        {
            attackCounter = attackDelay;
            lockRotation = true;
            lockMovement = true;
            isAttacking = true;
            cm.Stop();

            anim.SetTrigger(AnimatorParameters.hashAttack);
            transform.LookAt(opponent.transform.position);
        }
    }
}
public static partial class AnimatorParameters
{
    public static int hashMove = Animator.StringToHash("isMove");
    public static int hashSpeed = Animator.StringToHash("moveSpeed");
    public static int hashAttack = Animator.StringToHash("attack");
}
