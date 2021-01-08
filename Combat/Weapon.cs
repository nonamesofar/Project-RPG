using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] GameObject[] weaponPrefabs = null;

        [SerializeField] AnimatorOverrideController animatorOverride = null;

        [SerializeField] AnimationClip attack_R1 = null;
        [SerializeField] AnimationClip attack_L1 = null;

        [SerializeField] float weaponDamage = 20f;
        [SerializeField] float weaponRange = 0.2f;
        [SerializeField] bool isShield = false;

        const string weaponName = "Weapon";
        WeaponCollider collider = null;

        public float WeaponDamage { get => weaponDamage; set => weaponDamage = value; }
        public float WeaponRange { get => weaponRange; set => weaponRange = value; }
        public AnimationClip Attack_R1 { get => attack_R1; set => attack_R1 = value; }
        public AnimationClip Attack_L1 { get => attack_L1; set => attack_L1 = value; }
        public bool IsShield { get => isShield; set => isShield = value; }
        public WeaponCollider Collider { get => collider; set => collider = value; }

        public void Spawn(Transform transformHand, Animator animator, int hand)
        {
            if(hand < weaponPrefabs.Length)
            {
                GameObject weaponPrefab = weaponPrefabs[hand];
                if (weaponPrefab != null)
                {
                    DestroyOldWeapon(transformHand);
                    GameObject weapon = Instantiate(weaponPrefab, transformHand);
                    weapon.name = weaponName;
                    Collider = weaponPrefab.GetComponent<WeaponCollider>();
                }
            }

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
            //else
            //{
            //    var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            //    if (overrideController != null)
            //    {
            //        animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            //    }
            //}
        }

        private void DestroyOldWeapon(Transform hand)
        {
            Transform oldWeapon = hand.Find(weaponName);

            if (oldWeapon == null) return;
            //change name to avoid confusion on pick-up destroy
            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }
    }
}