using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject opponent;

    internal Animator anim;
    internal ClickToMove cm;

    private float attackCounter;

    [SerializeField] internal bool isAttacking;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cm = GetComponent<ClickToMove>();
    }

    // Update is called once per frame
    void Update()
    {


    }


}
public static partial class AnimatorParameters
{
    public static int hashMove = Animator.StringToHash("isMove");
    public static int hashSpeed = Animator.StringToHash("moveSpeed");
    public static int hashAttack = Animator.StringToHash("attack");
}
