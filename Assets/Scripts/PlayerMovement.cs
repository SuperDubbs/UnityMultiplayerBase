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
    private Vector3 moveDir;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundmask;

    Vector3 velocity;
    bool isGrounded;

    public Transform cameraTransform;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Sprint.performed += ctx => Sprint();
    }


    void Start()
    {
        if (IsOwner)
        {
            Debug.Log("Enabled1");
            FirstPersonCamera.Instance.FollowPlayer(cameraTransform);
            move = controls.Player.Move;
            controls.Player.Enable();
            Debug.Log("Enabled2");
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
        moveDir = transform.right * move.ReadValue<Vector2>().x + transform.forward * move.ReadValue<Vector2>().y;
        characterController.Move(moveDir *speed * Time.deltaTime);

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

    }
}
