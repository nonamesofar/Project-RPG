using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelInteraction : StateMachineBehaviour
{
    public string targetBool;
    public bool targetValue;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, targetValue);

    }
    //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.SetBool(targetBool, targetValue);
    //}
}
