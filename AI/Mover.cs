using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

namespace RPG.AI
{
    public class Mover : MonoBehaviour
    {

        [SerializeField] float maxSpeed = 2f;
        public float timeToStrafe = 5f;
        public float timeInStrafe = 0f;
        public int strafeDir = 0;
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private GameObject target;


        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            //TODO: weird ass portal bug that randomly places you?!
            navMeshAgent.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimator();
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            if (target == null)
            {
                animator.SetFloat("Forward", speed);
            }
            else
            {
                animator.SetFloat("Forward", 0f);
                animator.SetFloat("Sideways", strafeDir);
            }
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void SetTarget(GameObject target)
        {
            if (this.target == null)
            {
                this.target = target;
                timeInStrafe = 0f;
                strafeDir = Random.Range(-1, 1);
                strafeDir = (strafeDir < 0) ? -1 : 1;
            }
            if(target == null)
            {
                this.target = null;
            }
        }

        void HandleRotation(float moveAmount, Vector3 targetDir, float delta)
        {
            float moveOverride = moveAmount;
            if (target)
            {
                moveOverride = 1;
            }

            targetDir.Normalize();
            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;

            float actualRotationSpeed = navMeshAgent.angularSpeed;
            

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(
                transform.rotation, tr,
                delta * moveOverride * actualRotationSpeed);

            transform.rotation = targetRotation;
        }

        public void StrafeLeft()
        {
            if(timeInStrafe > timeToStrafe)
            {
                timeInStrafe = 0;
                timeToStrafe = Random.Range(3, 10);
                strafeDir = Random.Range(-1, 1);
                strafeDir = (strafeDir < 0) ? -1 : 1;
            }

            timeInStrafe += Time.deltaTime;
            var offsetPlayer = transform.position - target.transform.position;
            var dir = Vector3.Cross(offsetPlayer, Vector3.up);
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(transform.position + (strafeDir* dir));

            var rotationDir = target.transform.position - transform.position;

            //HandleRotation(1f, rotationDir, Time.deltaTime);

            transform.LookAt(target.transform);

        }
    }
}
