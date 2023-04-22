using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float gravity;
    [Range(0f,1f)]
    public float bounciness = 0.9f;
    [Range(0f,1f)]
    public float drag = 0.1f;

    int bounces;

    Vector3 velocity;

    Vector3 targetLocation;

    Rigidbody rb;


    // Update is called once per frame

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.layer == 7 || other.gameObject.layer == 6) // ground and environment layers
        {
            if (other.gameObject.layer == 6)
            {
                bounces += 1;
            }
            for (int i = 0; i<2; i++)
            {
                if (other.contacts[0].normal[i] != 0f)
                {
                    velocity[i] *= -bounciness;
                    return;
                }
            }
            velocity.z *= -bounciness;
        }
        if (other.gameObject.tag == "Player")
        {
            if (bounces <2)
            {
                GetHit();
                bounces = 0;
            }
        }
    }
    private void OnCollisionStay(Collision other)
    {
        velocity *= 1 - drag;
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
        GetHit();
    }

    void GetHit()
    {
        targetLocation = Court.Instance.GetTargetLocation(transform.position);
        float targetHeight = Random.Range(1.4f,4.5f);
        velocity.y = CalculateInitialVelocityForTargetHeight(targetHeight);
        //velocity.y = 7f;
        Vector3 direction = targetLocation - transform.position;
        direction.y = 0f;
        direction = ((direction)/CalculateTimeBeforeHittingGround());
        Debug.DrawRay(transform.position, direction, Color.green, 5f);
        

        velocity.x = direction.x;
        velocity.z = direction.z;
    }

    void Serve()
    {

    }

    float CalculateTimeBeforeHittingGround()
    {
        //https://www.youtube.com/watch?v=tfItlGfPHyo  25min;  a = -gravity b= -velocity.y c= DistanceToGround
        return ((velocity.y) + Mathf.Sqrt((velocity.y * velocity.y) + ((-4)*(-gravity/2)*( - targetLocation.y - transform.position.y))))/(-gravity);
    }
    float CalculateInitialVelocityForTargetHeight(float targetHeight) // returns y velocity needed to reach targetHeight
    {
        //https://openstax.org/books/university-physics-volume-1/pages/4-3-projectile-motion#:~:text=h%20%3D%20v%200%20y%202,component%20of%20the%20initial%20velocity.
        float initialVelocity;
        if (targetHeight >= transform.position.y)
        {
            initialVelocity = (Mathf.Sqrt(2*-gravity*(targetHeight - transform.position.y)));
        }
        else
        {
            initialVelocity = -(Mathf.Sqrt(Mathf.Abs(2*-gravity*(targetHeight - transform.position.y))));
        }
        return (initialVelocity);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetLocation, 0.2f);
    }
}
