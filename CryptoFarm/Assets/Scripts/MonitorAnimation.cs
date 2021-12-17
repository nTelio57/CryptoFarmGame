using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorAnimation : MonoBehaviour
{
    public Material material;
    public float speed;
    private Vector2 offset;

    private void Start()
    {
        offset = material.mainTextureOffset;
    }

    private void Update()
    {
        offset.y += speed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
