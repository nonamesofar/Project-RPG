using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {

        [SerializeField] Weapon rightHandWeapon;
        [SerializeField] Weapon leftHandWeapon;
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;

        private AnimationHandler animationHandler;

        // Start is called before the first frame update
        void Start()
        {
            animationHandler = GetComponent<AnimationHandler>();
            if(rightHandWeapon != null)
            {
                rightHandWeapon.Spawn(rightHand, GetComponent<Animator>(), Constants.WEAPON_RIGHT_HAND);
            }
            if (leftHandWeapon != null)
            {
                leftHandWeapon.Spawn(leftHand, GetComponent<Animator>(), Constants.WEAPON_LEFT_HAND);
            }

        }

        public void AttackBehaviour()
        {
            if (rightHandWeapon != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    animationHandler.SetBool(Constants.ANIMATION_INACTION, true);
                    animationHandler.PlayAnimation(rightHandWeapon.Attack_R1);
                }
            }

            if (leftHandWeapon != null)
            {
                if (leftHandWeapon.IsShield)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        animationHandler.SetBool(Constants.ANIMATION_BLOCK, true);
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        animationHandler.SetBool(Constants.ANIMATION_BLOCK, false);
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        animationHandler.SetBool(Constants.ANIMATION_INACTION, true);
                        animationHandler.PlayAnimation(leftHandWeapon.Attack_L1);
                    }

                }
            }
        }

        //Animation Events
        void Hit()
        {
            print("Hit");
        }
    }
}
