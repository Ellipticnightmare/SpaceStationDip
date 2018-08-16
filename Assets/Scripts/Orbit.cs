using System.Collections;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    #region variables
    public bool hideCursor = false;
    public Transform target;
    public Vector3 offset = new Vector3(0, 1f, 0);
    public float maxDistance;
    public float xSpeed;
    public float ySpeed;

    public float yMinLimit;
    public float yMaxLimit;

    public float distanceMin;
    public float distanceMax;
    [Header("Collision")]
    public bool cameraCollision;
    public float camRadius = 1f;
    public float rayDistance = 1000f;
    public LayerMask ignoreLayers;

    private Vector3 originalOffset;
    private float distance;
    private float x;
    private float y;
    #endregion
    // Use this for initialization
    void Start()
    {
        #region variable values
        maxDistance = 5f;
        xSpeed = 120f;
        ySpeed = 120f;

        yMinLimit = -20f;
        yMaxLimit = 80f;

        distanceMin = .5f;
        distanceMax = 15f;

        cameraCollision = true;

        distance = 5f;
        x = 0.1f;
        y = 0.1f;
        #endregion

        originalOffset = transform.position - target.position;
        rayDistance = originalOffset.magnitude;

        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        transform.SetParent(null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, camRadius);
    }

    private void FixedUpdate()
    {
        if (target)
        {
            if (cameraCollision)
            {
                Ray camRay = new Ray(target.position, -transform.forward);
                RaycastHit hit;
                if(Physics.Raycast(camRay, out hit, rayDistance, ~ignoreLayers, QueryTriggerInteraction.Ignore))
                {
                    distance = hit.distance;
                    return;
                }
            }

            distance = originalOffset.magnitude;
        }
    }

    public void Look(float mouseX, float mouseY)
    {
        x += mouseX * xSpeed * Time.deltaTime;
        y -= mouseY * ySpeed * Time.deltaTime;

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        transform.rotation = rotation;
    }

    private void LateUpdate()
    {
        if (target)
        {
            if (target)
            {
                Vector3 localOffset = transform.TransformDirection(offset);
                transform.position = (target.position + offset) + -transform.forward * distance;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
