using System;
using UnityEngine;

public class ProceduralAnimator : MonoBehaviour
{
    [SerializeField] protected Transform _subject;
    [SerializeField] protected float _dynamicFrequency = 1f;
    [SerializeField] protected float _dynamicDamp = 0.5f;
    [SerializeField] protected float _dynamicResponse = 2f;
    protected SecondOrderDynamics _dynamics;

    public Transform subject { get { return _subject; } }

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
        _dynamics = new SecondOrderDynamics(_dynamicFrequency, _dynamicDamp, _dynamicResponse, _subject.position);
    }

    public void ChangeDynamics(float frequency, float damping, float response)
    {
        _dynamicFrequency = frequency;
        _dynamicDamp = damping;
        _dynamicResponse = response;
        ChangeDynamics();
    }

    public void ChangeDynamics()
    {
        _dynamics.SetParameters(_dynamicFrequency, _dynamicDamp, _dynamicResponse);
    }

    public Vector3 DynamicsNext(Vector3 input)
    {
        return _dynamics.Next(input);
    }

    public void Initialize() { }

    public void Apply() { }
}
