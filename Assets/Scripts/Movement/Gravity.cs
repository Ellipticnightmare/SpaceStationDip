using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public bool grounded;
    private Vector3 posCur;
    public float rotation;
    public float distance;
    public GameObject obj;
    // Update is called once per frame
    void Update()
    {
        Ray rayBase = new Ray(transform.position, -transform.up);
        RaycastHit baseHit;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            grounded = false;
            Ray rayTop = new Ray(transform.position, transform.up);
            RaycastHit topHit;
            if (Physics.Raycast(rayTop, out topHit, 10f) == true)
            {
                Vector3 normal = topHit.normal;

                Quaternion q1 = Quaternion.AngleAxis(rotation, normal);
                Quaternion q2 = Quaternion.FromToRotation(Vector3.up, normal);
                Quaternion quat = q1 * q2;

                obj.transform.position = topHit.point + normal * distance;
                obj.transform.rotation = quat;
            }
            else
            {
                grounded = true;
            }
        }

        if((Physics.Raycast(rayBase, out baseHit, 10f) == true) && grounded)
        {
            Vector3 normal = baseHit.normal;

            Quaternion q1 = Quaternion.AngleAxis(rotation, normal);
            Quaternion q2 = Quaternion.FromToRotation(Vector3.up, normal);
            Quaternion quat = q1 * q2;

            obj.transform.position = baseHit.point + normal * distance;
            obj.transform.rotation = quat;
        }

        
    }
}
