using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WalkAnimatorFoot
{
    [SerializeField] private Vector3 _relativeRestPosition;
    [SerializeField] private Transform _subject;
    [SerializeField] private List<Collider> _ignoreColliders;

    private float _currentTime = 100f;
    private TransformData _baseTransformData;

    //getters and setters
    public Transform subject { get { return _subject; } }
    public float currentTime { get { return _currentTime; } set { _currentTime = value; } }
    public List<Collider> ignoreColliders { get { return _ignoreColliders; } }
    public TransformData baseTransformData { get { return _baseTransformData; } set { _baseTransformData = value; } }

    public bool IsRightFoot()
    {
        return _relativeRestPosition.x > 0;
    }

    public TransformData GetRestTransformData(Transform obj)
    {
        return new TransformData(obj.position + obj.rotation * _relativeRestPosition, obj.rotation, Vector3.one);
    }

    public void Initialize(Transform obj)
    {
        _baseTransformData = GetRestTransformData(obj);
    }

    public void StartStep()
    {
        _currentTime = 0f;
    }
}