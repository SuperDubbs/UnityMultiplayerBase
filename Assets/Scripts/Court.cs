using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Court : MonoBehaviour
{
    public static Court Instance;
    public Vector3 courtPos;
    public float courtWidth;
    public float courtLength;
    public float courtInnerLength;

    void Awake()
    {
        Instance = this;
    }

    public Vector3 GetTargetLocation(Vector3 pos)
    {
        Vector3 posFromCenter;
        Vector3 targetLocation = Vector3.zero;
        posFromCenter = pos - transform.position;
        targetLocation.x = Random.Range(-courtWidth/2 , courtWidth/2);
        targetLocation.z = Random.Range(transform.position.z +1.6f, transform.position.z + courtLength/2);
        if (posFromCenter.z > 0f)
        {
            targetLocation.z *= -1f;
        }
        return targetLocation;
        
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(courtWidth,0f,courtLength));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(courtWidth,0f,courtInnerLength));
    }

}
