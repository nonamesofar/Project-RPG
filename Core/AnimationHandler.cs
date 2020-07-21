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

        public void SetFloat(string animationName, float value, float damp, float delta)
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

        public void PlayAnimation(string animClip)
        {
            animator.Play(animClip);
        }

        public bool IsInAction()
        {
            return animator.GetBool(Constants.ANIMATION_INACTION);
        }
    }
}
