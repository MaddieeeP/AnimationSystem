using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : ProceduralAnimator
{
    [SerializeField] private Transform target;

    void Start()
    {
        InitializeDynamics();
        Initialize();
    }

    void Update()
    {
        Apply();
    }

    public void Apply()
    {

    }
}
