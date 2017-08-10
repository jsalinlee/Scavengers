using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Camera mainCamera;
    public GameObject followTarget;
    private Vector3 targetPosition;
    public float moveSpeed;
    public int cameraSize;
	void Start () {
        mainCamera = GetComponent<Camera>();
        SetCameraSize(cameraSize);
	}
    public void SetTarget(GameObject newTarget)
    {
        followTarget = newTarget;
    }

    public void SetCameraSize(int size)
    {
        mainCamera.orthographicSize = size;
    }
	void Update ()
    {
        targetPosition = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

	}
}
