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

    public abstract bool CanMove(WalkAnimatorFoot foot, Vector3 velocity, TransformData restTransformData, TransformData targetTransformData);

    public abstract bool MustMove(WalkAnimatorFoot foot, Vector3 velocity, TransformData restTransformData, TransformData targetTransformData);

    public virtual void Next(WalkAnimatorFoot foot, TransformData restTransformData, TransformData targetTransformData, float deltaTime)
    {
        if (foot.currentTime == stepTimeLength) //grounded
        {
            //FIX - slipping
            foot.baseTransformData.CopyTo(foot.subject);
            return;
        }

        foot.baseTransformData = GetNextBaseTransformData(foot.baseTransformData, targetTransformData, foot.currentTime, deltaTime);

        float t = foot.currentTime / stepTimeLength;
        foot.baseTransformData.CopyTo(foot.subject);
        ApplyAnimation(foot, t);

        foot.currentTime = Math.Clamp(foot.currentTime + deltaTime, 0f, stepTimeLength);
    }

    public abstract void ApplyAnimation(WalkAnimatorFoot foot, float t);

    public virtual TransformData GetTargetTransformData(TransformData restTransformData, Vector3 up, Vector3 velocity, float remainingStepTime, float maxDistance, List<Collider> ignoreColliders)
    {
        TransformData targetTransformData = new TransformData(restTransformData.position + velocity * remainingStepTime, restTransformData.rotation, Vector3.one);

        RaycastHit[] hits = Physics.RaycastAll(restTransformData.position + velocity * remainingStepTime + up * maxDistance, -up, maxDistance);
        foreach (RaycastHit hit in hits)
        {
            if (!ignoreColliders.Contains(hit.transform.GetComponent<Collider>()))
            {
                targetTransformData.position = hit.point;
                //FIX - rotation
                break;
            }
        }

        return targetTransformData;
    }

    public virtual TransformData GetNextBaseTransformData(TransformData previousBaseTransformData, TransformData targetTransformData, float currentTime, float deltaTime)
    {
        float prevProgress = _progressCurve.Evaluate(Math.Clamp(currentTime - deltaTime, 0f, stepTimeLength) / stepTimeLength);
        float currentProgress = _progressCurve.Evaluate(currentTime / stepTimeLength);

        float t = (currentProgress - prevProgress) / (1 - prevProgress);
        Vector3 position = Vector3.Lerp(previousBaseTransformData.position, targetTransformData.position, t);
        Quaternion rotation = Quaternion.Lerp(previousBaseTransformData.rotation, targetTransformData.rotation, Math.Clamp(t * 4f, 0f, 1f));

        return new TransformData(position, rotation, Vector3.one);
    }
}