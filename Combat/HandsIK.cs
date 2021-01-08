using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class HandsIK : MonoBehaviour
    {
        private Animator animator;
        public Transform leftHandObj;
        public Transform attachLeft;
        public bool canBeUsed;
        [Range(0, 1)] public float leftHandPositionWeight;
        [Range(0, 1)] public float leftHandRotationWeight;
        private Transform blendToTransform;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            StartCoroutine(_BlendIK(true, 0f, 0.2f, 1));
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (leftHandObj != null && canBeUsed)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandPositionWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandRotationWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, attachLeft.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, attachLeft.rotation);
            }
        }

        public IEnumerator _BlendIK(bool blendOn, float delay, float timeToBlend, int weapon)
        {
            if (canBeUsed)
            { 

                blendToTransform = attachLeft;
                yield return new WaitForSeconds(delay);
                float t = 0f;
                float blendTo = 0;
                float blendFrom = 0;
                if (blendOn)
                {
                    blendTo = 1;
                }
                else
                {
                    blendFrom = 1;
                }
                while (t < 1)
                {
                    t += Time.deltaTime / timeToBlend;
                    attachLeft = blendToTransform;
                    leftHandPositionWeight = Mathf.Lerp(blendFrom, blendTo, t);
                    leftHandRotationWeight = Mathf.Lerp(blendFrom, blendTo, t);
                    yield return null;
                }
                yield break;
            }
        }
    }
}