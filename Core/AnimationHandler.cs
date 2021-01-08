using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [RequireComponent(typeof(Animator))]
    public class AnimationHandler : MonoBehaviour
    {
        private Animator animator;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        bool AnimatorIsPlaying()
        {
            return animator.GetCurrentAnimatorStateInfo(0).length >
                   animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        public bool AnimatorIsPlaying(string stateName)
        {
            return AnimatorIsPlaying() && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
        }

        public void SetFloat(string animationName, float value, float damp = 0, float delta = 0)
        {
            animator.SetFloat(animationName, value, damp, delta);
        }
        public void SetBool(string boolName, bool value)
        {
            animator.SetBool(boolName, value);
        }
        public void SetTrigger(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }
        public void PlayAnimation(AnimationClip animClip)
        {
            if(animClip!=null)
                animator.Play(animClip.name);
        }

        public void CrossFadeAnimation(string animClip)
        {
            animator.CrossFade(animClip, 0.2f);
        }

        public void PlayAnimation(string animClip)
        {
            animator.Play(animClip);
        }

        //public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool isMirror = false, float velocityMultiplier = 1)
        //{
        //    anim.SetBool("isMirror", isMirror);
        //    anim.SetBool("isInteracting", isInteracting);
        //    anim.CrossFade(targetAnim, 0.2f);
        //    this.isInteracting = isInteracting;
        //    this.velocityMultiplier = velocityMultiplier;
        //}

        public bool IsInAction()
        {
            return animator.GetBool(Constants.ANIMATION_INACTION);
        }

        //dummy methods for now
        public bool CanRotate()
        {
            return true;
        }
    }
}
