using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollider : MonoBehaviour
{
    GameObject target = null;

    public GameObject Target { get => target; set => target = value; }

    private void OnTriggerEnter(Collider other)
    {
        Target = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        Target = null;
    }
}
