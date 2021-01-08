using System;
using RPG.Combat;
using RPG.Core;
using UnityEngine;

namespace RPG.AI
{
    public class BasicAIController : MonoBehaviour, ILockable
    {

        public Transform lockOnTarget;
        public Transform GetLockOnTarget(Transform from)
        {
            return lockOnTarget;
        }

        public bool IsAlive()
        {
            return true;
        }
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspiciousTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float pauseAtwaypoint = 5f;
        [Range(0, 1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;
        public float lockRange = 4f;

        public float weaponRange = 2f;

        Fighter enemyFighter;
        GameObject player;
        Mover mover;

        Vector3 guardPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeAtWaypoint = Mathf.Infinity;
        int currentWaypointIndex = 0;
        private float timeSinceLastAttack = 0;
        public float timeBetweenAttacks = 2f;
        private AnimationHandler animationHandler;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");

            enemyFighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            animationHandler = GetComponent<AnimationHandler>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (InAttackRangeOfPlayer())
            {
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspiciousTime)
            {
                //just chill here maybe he comes back
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();

            }
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;
            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }
            if (timeAtWaypoint > pauseAtwaypoint)
            {
                mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextWaypointIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            if (distanceToWaypoint < waypointTolerance)
                return true;
            return false;
        }

        private void SuspicionBehaviour()
        {
            //GetComponent<ActionScheduler>().CancelCurrentAction();
            print("SUSPICIOS AF!!!");
        }

        private void AttackBehaviour()
        {
            timeSinceLastAttack += Time.deltaTime;
            timeSinceLastSawPlayer = 0;

            if (animationHandler.IsInAction())
                return;
            if (IsInRange())
            {
                mover.Cancel();
                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    transform.LookAt(player.transform);
                    enemyFighter.AttackBehaviour(ButtonStatus.ButtonDown, ButtonStatus.NONE, ButtonStatus.NONE, ButtonStatus.NONE);
                    timeSinceLastAttack = 0;
                }
            }
            else
            {
                mover.MoveTo(player.transform.position, 1f);
            }
        }

        private bool IsInRange()
        {
            return Vector3.Distance(transform.position, player.transform.position) < weaponRange;
        }

        private bool InAttackRangeOfPlayer()
        {
            return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
        }

        private bool IsInLockRange()
        {
            return Vector3.Distance(player.transform.position, transform.position) < lockRange;
        }

        // Called by Unity
        private void OnDrawGizmos()
        {

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }

        #region AnimationEvents
        void FootL() { }
        void FootR() { }
        void Land() { }
        #endregion
    }
}

