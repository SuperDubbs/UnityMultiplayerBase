using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    private PlayerControls controls;
    public CharacterController characterController;
    private InputAction move;
    private InputAction sprint;

    private Vector3 wishDir;
    private Vector3 vel;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundmask;

    Vector3 velocity;
    bool isGrounded;

    public float walkSpeed = 6f;
    public float runSpeed = 8f;
    public float crouchSpeed = 4f;
    public float velocityToSlide = 5f;
    public float slideBoost = 3f;
    private float currentSpeed;
    private float targetSpeed;
    
    public float maxSpeed = 350f;
    public float maxAccel;
    public float friction = 4f;

    public Transform cameraTransform;

    public GameObject ball;
    public Transform ballSpawn;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Sprint.performed += ctx => Sprint();
        controls.Player.Sprint.canceled += ctx => Sprint();
        controls.Player.Crouch.performed += ctx => Crouch();
        controls.Player.Crouch.canceled += ctx => Sprint();
        controls.Player.Serve.performed += ctx => Serve();

        maxAccel = 10f * maxSpeed;
    }


    void Start()
    {
        if (IsOwner)
        {
            FirstPersonCamera.Instance.FollowPlayer(cameraTransform);
            move = controls.Player.Move;
            sprint = controls.Player.Sprint;
            controls.Player.Enable();
        }
    }
    void OnDisable() 
    {
        if (IsOwner)
        {
            controls.Player.Disable();
        }
    }
    void Update()
    {
        if (!IsOwner) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundmask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //Matchs yrotation to virtual camera
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x,Camera.main.transform.rotation.eulerAngles.y,transform.rotation.z));
        // handling regular movement
        wishDir = (transform.right * move.ReadValue<Vector2>().x + transform.forward * move.ReadValue<Vector2>().y).normalized;
        if (isGrounded)
        {
            //vel = UpdateGroundVel(vel, wishDir);
            vel = wishDir * speed * Time.deltaTime; 
        }

        if (!isGrounded)
        {
            //UpdateAirVel(vel, wishDir);
        }
        characterController.Move(vel);

        //Gravity stuff__________________________________
        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
        //_______________________________________________
    }

    void Jump()
    {
        if(isGrounded && IsOwner)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }
    }

    void Sprint()
    {
        Debug.Log("Sprinting");
    }
    void Crouch()
    {
        if (velocity.magnitude > velocityToSlide)
        {

        }
    }
    void Slide()
    {

    }
    void Serve()
    {
        SpawnBallServerRpc();
    }

    [ServerRpc]
    private void SpawnBallServerRpc()
    {
        SpawnBallClientRpc();
    }

    [ClientRpc]
    private void SpawnBallClientRpc()
    {
        GameObject spawnedBall = Instantiate(ball, ballSpawn.position, Quaternion.identity);
        spawnedBall.GetComponent<NetworkObject>().Spawn(true);
    }



/*
    Vector3 UpdateGroundVel(Vector3 vel, Vector3 wishDir)
    {
        float addSpeed;

        //vel = applyFriction(vel);

        currentSpeed = Vector3.Dot(vel, wishDir);
        addSpeed = Mathf.Clamp(maxSpeed - currentSpeed, 0f, maxAccel * Time.deltaTime);

        return (vel + (addSpeed * wishDir));
    }

    Vector3 UpdateAirVel(Vector3 vel, Vector3 wishDir)
    {
        float addSpeed;

        currentSpeed = Vector3.Dot(vel, wishDir);
        addSpeed = Mathf.Clamp(maxSpeed - currentSpeed, 0f, maxAccel * Time.deltaTime);

        return (vel + (addSpeed * wishDir));
    }

    Vector3 applyFriction(Vector3 vel)
    {
        Vector3 groundVel = new Vector3(vel.x, 0f, vel.y);
        
        return vel - (friction* Time.deltaTime);
    }
*/
}



