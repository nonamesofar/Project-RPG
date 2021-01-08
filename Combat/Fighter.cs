using RPG.Core;
using RPG.Movement;
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
        private PlayerMovement controller;

        // Start is called before the first frame update
        void Start()
        {
            animationHandler = GetComponent<AnimationHandler>();
            controller = GetComponent<PlayerMovement>();
            if(rightHandWeapon != null)
            {
                rightHandWeapon.Spawn(rightHand, GetComponent<Animator>(), Constants.WEAPON_RIGHT_HAND);
            }
            if (leftHandWeapon != null)
            {
                leftHandWeapon.Spawn(leftHand, GetComponent<Animator>(), Constants.WEAPON_LEFT_HAND);
            }

        }

        public ILockable FindLocakbleTarget()
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 20);
            for (int i = 0; i < cols.Length; i++)
            {
                ILockable lockable = cols[i].GetComponent<ILockable>();
                if (lockable != null)
                {
                    return lockable;
                }
            }

            return null;
        }

        public bool AttackBehaviour(ButtonStatus rb, ButtonStatus lb, ButtonStatus rt, ButtonStatus lt)
        {
            if (animationHandler.IsInAction())
            {
                return true;
            }
            if (rightHandWeapon != null)
            {
                if (rb == ButtonStatus.ButtonDown)
                {
                    animationHandler.SetBool(Constants.ANIMATION_INACTION, true);
                    animationHandler.PlayAnimation(rightHandWeapon.Attack_R1);
                    return true;
                }
            }

            if (leftHandWeapon != null)
            {
                if (leftHandWeapon.IsShield)
                {
                    if (lb == ButtonStatus.ButtonDown)
                    {
                        animationHandler.SetBool(Constants.ANIMATION_BLOCK, true);
                    }
                    if (lb == ButtonStatus.ButtonUp)
                    {
                        animationHandler.SetBool(Constants.ANIMATION_BLOCK, false);
                    }
                }
                else
                {
                    if (lb == ButtonStatus.ButtonDown)
                    {
                        animationHandler.SetBool(Constants.ANIMATION_INACTION, true);
                        animationHandler.PlayAnimation(leftHandWeapon.Attack_L1);
                    }
                    return true;
                }
                
            }
            return false;
        }

        //Animation Events
        void Hit()
        {
            
            if(rightHandWeapon.Collider.Target != null)
            {
                print("Hit");
            }
        }
    }
}
