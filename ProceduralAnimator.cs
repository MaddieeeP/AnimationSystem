using System;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    [SerializeField] protected Transform subject;
    protected SecondOrderDynamics dynamics;
    [SerializeField] protected float dynamicFrequency = 1f;
    [SerializeField] protected float dynamicDamping = 0.5f;
    [SerializeField] protected float dynamicResponse = 2f;

    void Start()
    {
        InitializeDynamics();
        Initialize();
    }

    void Update()
    {
        Apply();
    }

    public void InitializeDynamics()
    {
        dynamics = new SecondOrderDynamics(dynamicFrequency, dynamicDamping, dynamicResponse, subject.position);
    }

    public void ChangeDynamics(float frequency, float damping, float response)
    {
        dynamicFrequency = frequency;
        dynamicDamping = damping;
        dynamicResponse = response;
        ChangeDynamics();
    }

    public void ChangeDynamics()
    {
        dynamics.SetParameters(dynamicFrequency, dynamicDamping, dynamicResponse);
    }

    public void Initialize() { }

    public void Apply() { }
}
