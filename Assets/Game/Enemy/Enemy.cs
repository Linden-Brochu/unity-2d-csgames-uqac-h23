using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float movementSpeed = 1000.0f;
    new Rigidbody2D rb;

    [SerializeField] Rigidbody2D thePlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GetComponent<Rigidbody2D>().isKinematic = false;
        GetComponent<Rigidbody2D>().angularDrag = 0.0f;
        GetComponent<Rigidbody2D>().gravityScale = 0.0f;
    }


    //Patrouille


    // Deplacement de mon ennemi (from PlayerController)
    void Move(Vector2 targetVelocity)
    {
        // Set rigidbody velocity
        rb.velocity = (targetVelocity * movementSpeed) * Time.deltaTime; // Multiply the target by deltaTime to make movement speed consistent across different framerates

        //_anim.SetFloat("velocity_x", targetVelocity.x);
        //_anim.SetFloat("velocity_y", targetVelocity.y);
    }

    Vector2 GetDirectionToObject(Rigidbody2D other)
    {
        Vector2 directionToObject = other.position - rb.position;
        return directionToObject;
    }



    // Update is called once per frame
    void Update()
    {
        //Niveau 1: suivi d'une trajectoire

        //Niveau 3
        //Move(GetDirectionToPlayer(thePlayer));
    }
}
