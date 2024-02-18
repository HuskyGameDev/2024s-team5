using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*
 * This script can be attached to the player if we want the player to have the ability
 * to zoom the camera in and out.
 */

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float minZoom = 4f;
    public float maxZoom = 6f;
    // Update is called once per frame
    void Update()
    {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && virtualCamera.m_Lens.OrthographicSize > minZoom)
            {
                virtualCamera.m_Lens.OrthographicSize--;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && virtualCamera.m_Lens.OrthographicSize < maxZoom)
            {
                virtualCamera.m_Lens.OrthographicSize++;
            }
    
    }
}
