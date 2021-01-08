using UnityEngine;

namespace RPG.AI
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.red;
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetWaypoint(i), 0.2f);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextWaypointIndex(i)));
            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public Vector3 GetNextWaypoint(int i)
        {
            if (i == transform.childCount - 1)
            {
                return transform.GetChild(0).position;
            }
            else
            {
                return transform.GetChild(i + 1).position;
            }
        }

        public int GetNextWaypointIndex(int i)
        {
            if (i == transform.childCount - 1)
            {
                return 0;
            }
            return i + 1;

        }

    }
}