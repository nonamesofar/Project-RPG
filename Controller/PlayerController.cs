using Cinemachine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public CinemachineFreeLook cmFreeLook = null;
        public Transform target = null;

        private Transform camera;       
        private Mover mover;
        private Fighter fighter;

        

        private void Start()
        {
            mover = GetComponent<Mover>();
            camera = Camera.main.transform;
            mover.SetCamera(camera);

            fighter = GetComponent<Fighter>();
        }

        void Update()
        {
            mover.SetTarget(target);
            MovePlayer();

            fighter.AttackBehaviour();

        }

        private float MovePlayer()
        {
            float horizontal = Input.GetAxisRaw(Constants.INPUT_HORIZONTAL);
            float vertical = Input.GetAxisRaw(Constants.INPUT_VERTICAL);
            return mover.MoveController(horizontal, vertical);

        }
    }
}