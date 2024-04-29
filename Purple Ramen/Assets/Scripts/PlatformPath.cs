using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPath : MonoBehaviour
{
    public Transform getWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int getWaypointIndex(int currentIndex)
    {
        int nextIndex = currentIndex + 1;

        if(nextIndex == transform.childCount)
        {
            nextIndex = 0;
        }

        return nextIndex;
    }
}
