using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private Transform playerTransform;

    [SerializeField] 
    private Vector3 posOffset = new Vector3(0f, 1.5f, -10f);

    [SerializeField] 
    private float smooth = 0.25f;

    private Vector3 velocity = Vector3.zero;
    #endregion

    private void LateUpdate()
    {
        Vector3 targetPosition = playerTransform.position + posOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smooth);
    }
}
