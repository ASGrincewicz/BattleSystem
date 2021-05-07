using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parent_Turret_Y : MonoBehaviour
{

    public GameObject Parent_Point;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, Parent_Point.transform.position.y, Parent_Point.transform.position.z);
    }
}
