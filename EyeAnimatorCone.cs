using UnityEngine;

public class EyeAnimatorCone : LookAtTargetCone
{
    [SerializeField] Transform _target;
    [SerializeField] Transform _head;
    [SerializeField] Quaternion _headTransformRotationOffset = Quaternion.identity;

    protected override Vector3 forward { get { return _headTransformRotationOffset * _head.forward; } }
    protected override Vector3 up { get { return _headTransformRotationOffset * _head.up; } }
    protected override Vector3 targetPosition { get { return _target.position; } }
}