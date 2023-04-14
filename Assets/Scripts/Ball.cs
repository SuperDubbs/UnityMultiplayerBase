using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float gravity;
    [Range(0f,1f)]
    public float bounciness = 0.9f;

    Vector3 velocity;

    Vector3 targetLocation;

    Rigidbody rb;

    bool isBouncing;

    // Update is called once per frame

    private void OnCollisionEnter(Collision other) 
    {
        Debug.Log("collision happened");
        Debug.Log("velocity1: " + velocity);
        if (!isBouncing)
        {
            for (int i = 0; i<2; i++)
            {
                if (other.contacts[0].normal[i] != 0f)
                {
                    velocity[i] *= -bounciness;
                    isBouncing = true;
                    Debug.Log("velocity2: " + velocity);
                }
            }
        }
    }
    private void OnCollisionExit(Collision other)
    {
        isBouncing = false;
    }

    
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {


        if(transform.position.y <= 0.05f && velocity.y < 0f)
        {
            //velocity.y *= -bounciness; 
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        if (velocity.y < 0.01f && transform.position.y < 0.05f)
        {
            velocity.y = 0f;
        }

        rb.MovePosition(transform.position + (velocity * Time.deltaTime));
    }  

    void OnEnable() 
    {
        Serve();
    }

    void GetHit()
    {
        
    }

    void Serve()
    {
        targetLocation = Court.Instance.GetTargetLocation(transform.position);
        //velocity.y = Random.Range(5f,10f);
        velocity.y = 7f;
        Vector3 direction = targetLocation - transform.position;
        direction.y = 0f;
        direction = ((direction)/CalculateTimeBeforeHittingGround());
        Debug.DrawRay(transform.position, direction, Color.green, 5f);
        

        velocity.x = direction.x;
        velocity.z = direction.z;
        Debug.Log(CalculateTimeBeforeHittingGround());
    }

    float CalculateTimeBeforeHittingGround()
    {
        //https://www.youtube.com/watch?v=tfItlGfPHyo  25min;
        return ((velocity.y) + Mathf.Sqrt((velocity.y * velocity.y) + ((-4)*(-gravity/2)*( - targetLocation.y - transform.position.y))))/(-gravity);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetLocation, 0.2f);
    }
}
