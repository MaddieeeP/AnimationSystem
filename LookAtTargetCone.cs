using UnityEngine;

public abstract class LookAtTargetCone : MonoBehaviour //FIX - create second order dynamics for quaternion
{
    [SerializeField] protected Transform _subject;
    [SerializeField] protected float _maxRotation;

    protected abstract Vector3 forward { get; }
    protected abstract Vector3 up { get; }
    protected abstract Vector3 targetPosition { get; }

    public virtual void Update()
    {
        _subject.rotation = Quaternion.LookRotation(targetPosition - _subject.position, up).ClampRotation(forward, _maxRotation);
    }
}