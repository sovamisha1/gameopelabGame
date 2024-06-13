using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly : Interactable
{
    private Inventory inventory;
    private List<Transform> waypoints;
    private int currentWaypointIndex;
    private float speed;

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        waypoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            waypoints.Add(child);
        }

        // Unparent the waypoints so they don't move with the firefly
        foreach (Transform waypoint in waypoints)
        {
            waypoint.parent = null;
        }

        if (waypoints.Count > 0)
        {
            currentWaypointIndex = 0;
            speed = Random.Range(1.0f, 5.0f);
        }
    }

    void Update()
    {
        if (waypoints.Count == 0)
            return;
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            speed = Random.Range(10.0f, 15.0f);
        }
    }

    public override void Interact()
    {
        inventory.AddItem("Светлячок");
        if (inventory.DoesContainItem("Светлячок", 5))
        {
            inventory.RemoveItem("Светлячок", 5);
            inventory.AddItem("Банка Cветлячков");
        }
        foreach (Transform waypoint in waypoints)
        {
            Destroy(waypoint.gameObject);
        }

        Destroy(gameObject);
    }
}
