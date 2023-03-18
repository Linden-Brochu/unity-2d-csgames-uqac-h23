using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeField] public float movementSpeed;

    [SerializeField] Transform thePlayer;

    private Transform[] waypoints;
    [SerializeField] private GameObject GameObjectWithWaypoints;

    private float fieldOfViewRadius;

    [SerializeField] private GameObject fieldOfViewCircle;

    private int currentWaypointIndex = 0;

    void Start()
    {
        //Recupere les waypoints sans inclure l'objet de la liste
        waypoints = GameObjectWithWaypoints.GetComponentsInChildren<Transform>().Where(test=>test!=GameObjectWithWaypoints.transform).ToArray();

        //Start at first waypoint
        transform.position = waypoints[0].position;

        fieldOfViewRadius = fieldOfViewCircle.transform.localScale[0] / 2;
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
        //followWaypoints();
        //Niveau 2: Detection joueur
        if (Vector2.Distance(transform.position, thePlayer.position) < fieldOfViewRadius)
        {
            Move(thePlayer.position);
        }
        else
        {
            followWaypoints();
        }

        //Niveau 3
        //Move(thePlayer.position);
    }
}
