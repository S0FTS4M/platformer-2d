using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingBlock : MonoBehaviour
{
    [SerializeField]
    Vector3 direction;

    [SerializeField]
    float speed;

    [SerializeField]
    int blockCount;

    Vector3 _startPos;

    private void Start()
    {
        _startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateTile();
    }

    private void AnimateTile()
    {
        if (Vector3.Magnitude(transform.localPosition - _startPos) >= blockCount)
        {
            direction *= -1;
            _startPos = transform.localPosition;
        }

        transform.localPosition += direction * speed * Time.deltaTime; 
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, Vector3.Magnitude(transform.localPosition - _startPos).ToString());
    }
}
