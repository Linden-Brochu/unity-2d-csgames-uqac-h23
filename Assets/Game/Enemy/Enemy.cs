using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    public float movementSpeed;

    [SerializeField] 
    Transform thePlayer;
    
    [SerializeField] 
    private GameObject GameObjectWithWaypoints;
    
    [SerializeField] 
    private GameObject fieldOfViewCircle;

    private Transform[] waypoints;
    private float fieldOfViewRadius;

    private float startSearchingTime;

    private int currentWaypointIndex = 0;
    private bool searchFinished = false;

    SpriteRenderer m_SpriteRenderer;

    private float nearbyDistanceThresh = 10;
    enum Status
    {
        Patrol,
        Follow,
        Search,
        FollowFromFriends
    }

    private Status actionStatus;

    void Start()
    {
        //Recupere les waypoints sans inclure l'objet de la liste
        waypoints = GameObjectWithWaypoints.GetComponentsInChildren<Transform>().Where(test=>test!=GameObjectWithWaypoints.transform).ToArray();

        //Start at first waypoint
        transform.position = waypoints[0].position;

        //Get field of view radius from size in unity
        fieldOfViewRadius = fieldOfViewCircle.transform.localScale[0] / 2;

        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        EnemyManager.singleton.enemies.Add(this);
        actionStatus = Status.Patrol;
    }

    void Move(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed); 
    }


    void FollowWaypoints()
    {
        Vector3 waypointPos = waypoints[currentWaypointIndex].position;
        if (Vector2.Distance(waypointPos, transform.position) < .1f)
        {
            currentWaypointIndex = (currentWaypointIndex+1) % waypoints.Length;
        }
        Move(waypointPos);
    }


    void SearchPlayer()
    {
        //Spin searching for player
        float rotationSpeed = .1f;
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
        
        if (Time.time >= startSearchingTime + 5f)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            searchFinished = true;
        }
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, thePlayer.position) < fieldOfViewRadius)
        {
            actionStatus = Status.Follow;
        }
        else if (actionStatus == Status.Follow){
            //Enemy not following anymore
            actionStatus = Status.Search;
            startSearchingTime = Time.time;
            searchFinished = false;
        }
        else if ((actionStatus == Status.Search && searchFinished) || actionStatus == Status.FollowFromFriends)
        {
            actionStatus = Status.Patrol;
        }
    }
    
    private void LateUpdate()
    {
        //Check if another enemy sees/follow player
        if (EnemyManager.singleton.enemies.Any(enemy => enemy.actionStatus == Status.Follow 
            && Vector2.Distance(transform.position, enemy.transform.position) < nearbyDistanceThresh)
            && actionStatus != Status.Follow)
        {
            actionStatus = Status.FollowFromFriends;
        }

        switch (actionStatus)
        {
            case Status.Patrol:
                FollowWaypoints();
                m_SpriteRenderer.color = Color.green;
                break;
            case Status.Follow:
                Move(thePlayer.position);
                m_SpriteRenderer.color = Color.red;
                break;
            case Status.Search:
                SearchPlayer();
                m_SpriteRenderer.color = Color.blue;
                break;
            case Status.FollowFromFriends:
                Move(thePlayer.position);
                m_SpriteRenderer.color = Color.yellow;
                break;
        }
    }

}
