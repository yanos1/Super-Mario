using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Camera mainCamera; // Reference to your main camera
    public Mario character; // Reference to your character's transform

    void Update()
    {
        float viewportWidth = mainCamera.pixelWidth;

        float cameraX = mainCamera.transform.position.x;

        
        for (int i = 0;i < character.transform.childCount;i++)
        {
            Transform child = character.transform.GetChild(i);
            if (child.gameObject.activeSelf && child.gameObject.transform.position.x >= cameraX)
            {
                Vector3 newCameraPosition = mainCamera.transform.position;
                newCameraPosition.x = child.position.x;
                mainCamera.transform.position = newCameraPosition;

            }

        }
    }
}