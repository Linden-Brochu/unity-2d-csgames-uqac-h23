using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float movementSpeed = 10.0f;

    [SerializeField] Transform thePlayer;
    private Transform[] waypoints;
    [SerializeField] private GameObject GameObjectWithWaypoints;

    private int currentWaypointIndex = 0;

    void Start()
    {
        //Recupere les waypoints sans inclure l'objet de la liste
        waypoints = GameObjectWithWaypoints.GetComponentsInChildren<Transform>().Where(test=>test!=GameObjectWithWaypoints.transform).ToArray();

        //Start at first waypoint
        transform.position = waypoints[0].position;
        //Debug.Log(transform.position);
    }


    //Patrouille


    void Move(Vector2 targetPos)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * movementSpeed); 
    }

    void followWaypoints()
    {
        Vector3 waypointPos = waypoints[currentWaypointIndex].position;
        if (Vector2.Distance(waypointPos, transform.position) < .1f)
        {
            currentWaypointIndex = (currentWaypointIndex+1) % waypoints.Length;
            //Debug.Log(currentWaypointIndex);
        }

        Move(waypointPos);
    }



    // Update is called once per frame
    void Update()
    {
        //Niveau 1: suivi d'une trajectoire
        followWaypoints();
        //Niveau 3
        //Move(thePlayer.position);
    }
}
