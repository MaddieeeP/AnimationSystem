using UnityEngine;

public class NeckAnimator : LookAtTargetFrustum
{
    [SerializeField] Transform _target;
    [SerializeField] Actor _actor;
    [SerializeField] float _strength;

    protected override Vector3 forward { get { return _actor.transform.forward; } }
    protected override Vector3 up { get { return _actor.up; } }
    protected override Vector3 targetPosition { get { return _target.position; } }

    public override void Update()
    {
        _subject.rotation = Quaternion.Lerp(Quaternion.LookRotation(forward, up), Quaternion.LookRotation(targetPosition - transform.position, up).ClampRotation(forward, up, _maxXRotation, _maxYRotation), _strength);
    }
}