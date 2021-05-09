using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGizmos : MonoBehaviour
{
    public Transform target;
    public float sphereRadius;
    public void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, 2f);
        Gizmos.DrawLine(transform.position, target.position);
    }
}
