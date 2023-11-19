using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : ProceduralAnimator
{
    private bool _auto = false;
    public bool auto { get { return _auto; } set { _auto = value; } }
    private float _maxDist = 10f;
    public float maxDist { get { return _maxDist; } set { _maxDist = value; } }
    [SerializeField] private Transform origin;

    void Start()
    {
        InitializeDynamics();
        Initialize();
    }

    void Update()
    {
        Apply();
    }

    void Apply()
    {
        if (auto)
        {
            //Physics.Raycast();
        }
    }
}
