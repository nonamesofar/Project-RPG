using Cinemachine;
using UnityEngine;

namespace RPG.Controller

{
    public class CameraManager : MonoBehaviour
    {
        private PlayerControls defaultcontrols; //default controls is just the Csharp code you generate from the action maps asset
        private Vector2 LookDelta;
        public bool Lockon; //this would be used to switch to a virtual camera for a lockon system

        private void Awake() => defaultcontrols = new PlayerControls();

        private void OnEnable() => defaultcontrols.Player.Enable();
        private void OnDisable() => defaultcontrols.Player.Disable();

        private void Start()
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
        }

        public float GetAxisCustom(string axisName)
        {
            LookDelta = defaultcontrols.Player.Camera.ReadValue<Vector2>(); // reads theavailable camera values and uses them.
            LookDelta.Normalize();

            if (axisName == "Mouse X")
            {
                return LookDelta.x;
            }
            else if (axisName == "Mouse Y")
            {
                return LookDelta.y;
            }
            return 0;
        }
    }
}
