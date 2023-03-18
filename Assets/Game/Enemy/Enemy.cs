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

    Time StartSearchingTime; //Not working yet

    private int currentWaypointIndex = 0;
    private bool searchFinished = false;
    enum Status
    {
        Patrol,
        Follow,
        Search
    }

    private Status actionStatus = Status.Patrol;

    void Start()
    {
        //Recupere les waypoints sans inclure l'objet de la liste
        waypoints = GameObjectWithWaypoints.GetComponentsInChildren<Transform>().Where(test=>test!=GameObjectWithWaypoints.transform).ToArray();

        //Start at first waypoint
        transform.position = waypoints[0].position;

        //Get field of view radius from size in unity
        fieldOfViewRadius = fieldOfViewCircle.transform.localScale[0] / 2;
    }

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


    void search()
    {
        float rotationSpeed = 1f;
        transform.Rotate(new Vector3(0, 0, transform.rotation[2] + rotationSpeed));
        //Debug.Log("rotation = " + transform.rotation[2].ToString());

        if(true/*Time.time - StartSearchingTime > 5*/)
        {
            // 5 secondes
            Debug.Log("Finished rotation");
            transform.Rotate(new Vector3(0, 0, 0)); //Reset rotation
            searchFinished = true;
        }
    }
    



    // Update is called once per frame
    void Update()
    {
        //Niveau 1: suivi d'une trajectoire
        //followWaypoints();

        //Niveau 2: Detection joueur
        //if (Vector2.Distance(transform.position, thePlayer.position) < fieldOfViewRadius)
        //{
        //    Debug.Log("Following player");
        //    Move(thePlayer.position);
        //}
        //else
        //{
        //    followWaypoints();
        //}

        //Niveau 3


        if (Vector2.Distance(transform.position, thePlayer.position) < fieldOfViewRadius)
        {
            actionStatus = Status.Follow;
            Debug.Log("Follow");
        }
        else
        {
            if (actionStatus == Status.Follow){
                actionStatus = Status.Search;
                Debug.Log("Start Search");
                //StartSearchingTime = Time.time;
                searchFinished = false;
            }
            else
            {
                if (actionStatus == Status.Search && searchFinished) {
                    actionStatus = Status.Patrol;
                    Debug.Log("Patrolling");
                }
            } 
            
        }



        switch(actionStatus)
        {
            case Status.Patrol:
                followWaypoints();
                break;
            case Status.Follow:
                Move(thePlayer.position);
                break;
            case Status.Search:
                //tourne sur lui même
                search();
                break;
        }
        
    }
}
