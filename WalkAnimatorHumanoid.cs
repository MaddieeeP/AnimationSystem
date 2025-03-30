using System;
using UnityEngine;

public class WalkAnimatorHumanoid : WalkAnimator //Dictates when feet move in a walk animation
{
    [SerializeField] private Actor _actor;
    [SerializeField] private WalkAnimatorFoot _dominantFoot;
    [SerializeField] private WalkAnimatorFoot _secondFoot;

    [SerializeField] private AnimationCurve _xCurve;
    [SerializeField] private AnimationCurve _yCurve;

    [SerializeField] private float _oppositeStepStageOffset = 0.5f;

    [SerializeField] protected float _stepTimeLength;

    [SerializeField] private float _minimumStepDistance = 0.1f; //smallest distance from target position for which a leg should move
    [SerializeField] private float _maximumStepDistance = 1f; //longest distance from target position for which a leg can delay moving until 
    [SerializeField] private int _minimumStepDegrees = 15; //smallest difference from target rotation for which a leg should move
    [SerializeField] private int _maximumStepDegrees = 60; //largest difference from target rotation for which a leg can delay moving until 

    //getters and setters
    protected override float stepTimeLength { get { return _stepTimeLength; } }


    public override void Start()
    {
        _dominantFoot.Initialize(_actor.transform);
        _secondFoot.Initialize(_actor.transform);
    }

    public override void LateUpdate()
    {
        float deltaTime = SimulationController.globalTimeScale * _actor.relativeTimeScale * Time.deltaTime;
        Vector3 velocity = _actor.velocity;
        Vector3 angularVelocity = _actor.angularVelocity;

        _dominantFoot.currentTime = Math.Clamp(_dominantFoot.currentTime, 0f, stepTimeLength);
        _secondFoot.currentTime = Math.Clamp(_secondFoot.currentTime, 0f, stepTimeLength);

        TransformInfo restTransformInfoDominant = _dominantFoot.GetRestTransformInfo(_actor.transform);
        TransformInfo restTransformInfoSecond = _secondFoot.GetRestTransformInfo(_actor.transform);
        TransformInfo targetTransformInfoDominant = GetTargetTransformInfo(restTransformInfoDominant, _actor.up, velocity, angularVelocity, stepTimeLength - _dominantFoot.currentTime, _maxStepHeight, _dominantFoot.ignoreColliders);
        TransformInfo targetTransformInfoSecond = GetTargetTransformInfo(restTransformInfoSecond, _actor.up, velocity, angularVelocity, stepTimeLength - _secondFoot.currentTime, _maxStepHeight, _secondFoot.ignoreColliders);
        
        if (_dominantFoot.currentTime == stepTimeLength)
        {
            if (MustMove(_dominantFoot, velocity, angularVelocity, restTransformInfoDominant, targetTransformInfoDominant) || CanMove(_dominantFoot, velocity, angularVelocity, restTransformInfoDominant, targetTransformInfoDominant) && _secondFoot.currentTime >= stepTimeLength * _oppositeStepStageOffset)
            {
                _dominantFoot.StartStep();
            }
        }
        if (_secondFoot.currentTime == stepTimeLength)
        {
            if (MustMove(_secondFoot, velocity, angularVelocity, restTransformInfoSecond, targetTransformInfoSecond) || CanMove(_secondFoot, velocity, angularVelocity, restTransformInfoSecond, targetTransformInfoSecond) && _dominantFoot.currentTime >= stepTimeLength * _oppositeStepStageOffset)
            {
                _secondFoot.StartStep();
            }
        }

        Next(_dominantFoot, restTransformInfoDominant, targetTransformInfoDominant, deltaTime);
        Next(_secondFoot, restTransformInfoSecond, targetTransformInfoSecond, deltaTime);
    }

    public override bool CanMove(WalkAnimatorFoot foot, Vector3 velocity, Vector3 angularVelocity, TransformInfo restTransformInfo, TransformInfo targetTransformInfo)
    {
        foot.subject.rotation.ShortestRotation(targetTransformInfo.rotation).ToAngleAxis(out float angleDifference, out Vector3 _);
        return Vector3.Distance(foot.subject.position, targetTransformInfo.position) > _minimumStepDistance || angleDifference > _minimumStepDegrees;
    }

    public override bool MustMove(WalkAnimatorFoot foot, Vector3 velocity, Vector3 angularVelocity, TransformInfo restTransformInfo, TransformInfo targetTransformInfo)
    {
        foot.subject.rotation.ShortestRotation(targetTransformInfo.rotation).ToAngleAxis(out float angleDifference, out Vector3 _);
        return Vector3.Distance(foot.subject.position, targetTransformInfo.position) > _maximumStepDistance || angleDifference > _maximumStepDegrees;
    }

    public override void ApplyAnimation(WalkAnimatorFoot foot, float t)
    {
        foot.subject.position += _actor.transform.rotation * new Vector3(foot.IsRightFoot() ? _xCurve.Evaluate(t) : -_xCurve.Evaluate(t), _yCurve.Evaluate(t), 0f);
    }
}
