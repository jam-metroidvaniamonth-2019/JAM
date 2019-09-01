using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointComponent : MonoBehaviour
{
    [SerializeField]
    Transform[] waypoints;

    [SerializeField]
    float moveSpeed=2f;

    [SerializeField]
    int wayPointIndex = 0;


    private void Start()
    {
        transform.position = waypoints[wayPointIndex].transform.position;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                waypoints[wayPointIndex].transform.position,
                                                moveSpeed * Time.deltaTime);

        if (transform.position == waypoints[wayPointIndex].transform.position)
        {
            wayPointIndex += 1;
        }

        if (wayPointIndex == waypoints.Length)
            wayPointIndex = 0;
    }

}
