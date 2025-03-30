using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkAnimatorFoot
{
    [SerializeField] private Vector3 _relativeRestPosition;
    [SerializeField] private Transform _subject;
    [SerializeField] private List<Collider> _ignoreColliders;

    private float _currentTime = 100f;
    private TransformInfo _baseTransformInfo;

    //getters and setters
    public Transform subject { get { return _subject; } }
    public float currentTime { get { return _currentTime; } set { _currentTime = value; } }
    public List<Collider> ignoreColliders { get { return _ignoreColliders; } }
    public TransformInfo baseTransformInfo { get { return _baseTransformInfo; } set { _baseTransformInfo = value; } }

    public bool IsRightFoot()
    {
        return _relativeRestPosition.x > 0;
    }

    public TransformInfo GetRestTransformInfo(Transform obj)
    {
        return new TransformInfo(obj.position + obj.rotation * _relativeRestPosition, obj.rotation, Vector3.one);
    }

    public void Initialize(Transform obj)
    {
        _baseTransformInfo = GetRestTransformInfo(obj);
    }

    public void StartStep()
    {
        _currentTime = 0f;
    }
}