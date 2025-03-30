using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WalkAnimator : MonoBehaviour
{
    [SerializeField] protected float _maxStepHeight; //maximum height for which a foot can step
    [SerializeField] protected AnimationCurve _progressCurve;

    protected abstract float stepTimeLength { get; }

    public abstract void Start();
    public abstract void LateUpdate();

    public abstract bool CanMove(WalkAnimatorFoot foot, Vector3 velocity, Vector3 angularVelocity, TransformInfo restTransformInfo, TransformInfo targetTransformInfo);

    public abstract bool MustMove(WalkAnimatorFoot foot, Vector3 velocity, Vector3 angularVelocity, TransformInfo restTransformInfo, TransformInfo targetTransformInfo);

    public virtual void Next(WalkAnimatorFoot foot, TransformInfo restTransformInfo, TransformInfo targetTransformInfo, float deltaTime)
    {
        if (foot.currentTime == stepTimeLength) //grounded
        {
            //FIX - slipping
            foot.baseTransformInfo.CopyTo(foot.subject);
            return;
        }

        foot.baseTransformInfo = GetNextBaseTransformInfo(foot.baseTransformInfo, targetTransformInfo, foot.currentTime, deltaTime);

        float t = foot.currentTime / stepTimeLength;
        foot.baseTransformInfo.CopyTo(foot.subject);
        ApplyAnimation(foot, t);

        foot.currentTime = Math.Clamp(foot.currentTime + deltaTime, 0f, stepTimeLength);
    }

    public abstract void ApplyAnimation(WalkAnimatorFoot foot, float t);

    public virtual TransformInfo GetTargetTransformInfo(TransformInfo restTransformInfo, Vector3 up, Vector3 velocity, Vector3 angularVelocity, float remainingStepTime, float maxDistance, List<Collider> ignoreColliders)
    {
        TransformInfo targetTransformInfo = new TransformInfo(restTransformInfo.position + velocity * remainingStepTime, restTransformInfo.rotation, Vector3.one);

        RaycastHit[] hits = Physics.RaycastAll(restTransformInfo.position + velocity * remainingStepTime + up * maxDistance, -up, maxDistance);
        foreach (RaycastHit hit in hits)
        {
            if (!ignoreColliders.Contains(hit.transform.GetComponent<Collider>()))
            {
                targetTransformInfo.position = hit.point;
                //FIX - rotation
                break;
            }
        }

        return targetTransformInfo;
    }

    public virtual TransformInfo GetNextBaseTransformInfo(TransformInfo previousBaseTransformInfo, TransformInfo targetTransformInfo, float currentTime, float deltaTime)
    {
        float prevProgress = _progressCurve.Evaluate(Math.Clamp(currentTime - deltaTime, 0f, stepTimeLength) / stepTimeLength);
        float currentProgress = _progressCurve.Evaluate(currentTime / stepTimeLength);

        float t = (currentProgress - prevProgress) / (1 - prevProgress);
        Vector3 position = Vector3.Lerp(previousBaseTransformInfo.position, targetTransformInfo.position, t);
        Quaternion rotation = Quaternion.Lerp(previousBaseTransformInfo.rotation, targetTransformInfo.rotation, Math.Clamp(t * 4f, 0f, 1f));

        return new TransformInfo(position, rotation, Vector3.one);
    }
}