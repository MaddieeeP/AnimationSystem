using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reach : ProceduralAnimator
{
    [SerializeField] private Transform boundingTransform;
    [SerializeField] private Vector3 reachFor;
    [SerializeField] private bool ignoreBounds = false;
    [SerializeField] private Bounds bounds;

    void Start()
    {
        InitializeDynamics();
        Initialize();
    }

    void Update()
    {
        Apply();
    }

    public void ReachFor(Vector3 position)
    {
        reachFor = position;
    }

    public void ReachFor(Transform transform) => ReachFor(transform.position);

    public void Apply()
    {
        Vector3 position = reachFor; 
        if (bounds == null || ignoreBounds)
        {
            position = dynamics.Next(reachFor);
        }
        else
        {
            position = position.Clamp(bounds.min, bounds.max);
            position = dynamics.Next(position);
            position = position.Clamp(bounds.min, bounds.max);
        }
        subject.position = position;
    }
}