using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private PlatformPath pointsForPlatform;
    [SerializeField] private float speed;
    private int targetIndex;
    private Transform previousPoint;
    private Transform targetPoint;
    private float duration;
    private float elapsed;
    // Start is called before the first frame update
    void Start()
    {
        targetNextWaypoint();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsed += Time.deltaTime;

        float elapsedPercent = elapsed / duration;

        transform.position = Vector3.Lerp(previousPoint.position, targetPoint.position, elapsedPercent);
        transform.rotation = Quaternion.Lerp(previousPoint.rotation, targetPoint.rotation, elapsedPercent);

        if (elapsedPercent >= 1)
        {
            targetNextWaypoint();
        }
    }

    private void targetNextWaypoint()
    {
        previousPoint = pointsForPlatform.getWaypoint(targetIndex);
        targetIndex = pointsForPlatform.getWaypointIndex(targetIndex);
        targetPoint = pointsForPlatform.getWaypoint(targetIndex);

        elapsed = 0;

        float distance = Vector3.Distance(previousPoint.position, targetPoint.position);
        duration = distance / speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(transform);
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
