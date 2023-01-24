using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FirstPersonCamera : MonoBehaviour
{
    public static FirstPersonCamera Instance;
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    void Awake() 
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        Instance = this;
    }

    public void FollowPlayer(Transform transform)
    {
        cinemachineVirtualCamera.Follow = transform;
    }
}
