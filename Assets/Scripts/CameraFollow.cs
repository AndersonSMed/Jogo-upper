using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    [SerializeField]
    // Target that will be followed
    private Transform target;
    [SerializeField]
    // Smooth speed used to calculate camera's smooth movement speed
    private float smoothSpeed = 2;
    [SerializeField]
    
    // Stores the minimum x position and the maximum x position the camera can achieve
    private float minX, maxX;
    // Stores the desired position the camera will follow
    private Vector2 desiredPosition;

    // This Update is the last update function called by frame
    void LateUpdate() {
        // Update the desired position
        UpdateDesiredPosition();

        // Step stores the smooth speed * Time.deltaTime to create smoothness in the movement
        float step = smoothSpeed * Time.deltaTime;
        // newPos stores the camera new position following a smooth movement
        Vector2 newPos = Vector2.MoveTowards(transform.position, desiredPosition, step);
        // Finnaly stores the new position in the transform component
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }

    // Here we update our desired position
    void UpdateDesiredPosition() {
        // Initialy the desired position receives the target x position
        desiredPosition.x = target.position.x;
        desiredPosition.y = transform.position.y;
        // If that position minus the center of the camera is minus than or equals to minX
        if(desiredPosition.x - Camera.main.orthographicSize / 2 <= minX) {
            // Then the desiredPosition.x is the minX
            desiredPosition.x = minX;
        // Almost the same here, but now we are comparing the desired position x plus the size of camera / 2 with the maxX
        }else if(desiredPosition.x + Camera.main.orthographicSize / 2 >= maxX) {
            // If it is greater than or equals to maxX, the desiredPosition.x is the maxX
            desiredPosition.x = maxX;
        }
    }

}
