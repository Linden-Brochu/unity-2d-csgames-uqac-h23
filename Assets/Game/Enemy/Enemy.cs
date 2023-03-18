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

    private float StartSearchingTime; //Not working yet

    private int currentWaypointIndex = 0;
    private bool searchFinished = false;

    SpriteRenderer m_SpriteRenderer;

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

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.green;

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
        //Fonction 1 : Tourne sur soi-même
        float rotationSpeed = .1f;
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
        //Debug.Log("rotation = " + transform.rotation[2].ToString());


        if (Time.time >= StartSearchingTime + 5f)
        {
            // 5 secondes
            //Debug.Log("Finished rotation");
            transform.rotation = Quaternion.Euler(0, 0, 0);
            searchFinished = true;
        }

        //TODO Fonction 2: Tourne autour d'un point, (recherche plus visuelle)
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
            //Debug.Log("Follow"); 
            actionStatus = Status.Follow;
        }
        else
        {
            if (actionStatus == Status.Follow){
                //Debug.Log("Start Search");
                actionStatus = Status.Search;
                StartSearchingTime = Time.time;
                searchFinished = false;
            }
            else
            {
                if (actionStatus == Status.Search && searchFinished) {
                    //Debug.Log("Patrolling");
                    actionStatus = Status.Patrol;
                }
            } 
            
        }



        switch(actionStatus)
        {
            case Status.Patrol:
                followWaypoints();
                m_SpriteRenderer.color = Color.green;
                break;
            case Status.Follow:
                Move(thePlayer.position);
                m_SpriteRenderer.color = Color.red;
                break;
            case Status.Search:
                search();
                m_SpriteRenderer.color = Color.blue;
                break;
        }
        
    }
}
