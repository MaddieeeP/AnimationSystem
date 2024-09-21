using System.Collections.Generic;
using UnityEngine;

public class FootAnimator : MonoBehaviour
{
    [SerializeField] private PhysicActor _actor;
    [SerializeField] private Transform _hip;
    [SerializeField] private List<Collider> _ignoreColliders;
    [SerializeField] private AnimationCurve _footArc;
    [SerializeField] private float _speedDistanceScale = 1f;
    [SerializeField] private float _speedHeightScale = 1f;
    [SerializeField] private float _minimumStepDistance = 0.1f;
    [SerializeField] private float _maximumLegLength = 0f;
    [SerializeField] private float _stepTimeLength = 0.1f;
    [SerializeField] private float _oppositeStepStageOffset = 0.5f;
    [SerializeField] private FootAnimator _oppositeFoot;

    private float _currentTime = 0f;
    private Vector3 _prevGroundedPosition;
    private Vector3 _position;
    private Quaternion _rotation;
    private bool _grounded = true;

    //getters and setters
    protected virtual Vector3 up { get { return Vector3.up; } }
    public bool grounded { get { return _grounded; } }
    public float currentTime { get { return _currentTime; } }
    public float stepTimeLength { get { return _stepTimeLength; } }

    public void Start()
    {
        _position = transform.position;
        _rotation = transform.rotation;

        _prevGroundedPosition = transform.position;
        _currentTime = _stepTimeLength;
    }

    public void Update()
    {
        float speedProportion = _actor.subjectiveVelocity.magnitude / _actor.moveMaxSpeed;

        Vector3 nextGroundedPosition = (_hip.position - _maximumLegLength * up) + _actor.subjectiveVelocity * _stepTimeLength * 0.5f;

        if (_grounded)
        {
            if (Vector3.Distance(transform.position, nextGroundedPosition) > speedProportion * _speedDistanceScale + _minimumStepDistance && _oppositeFoot.currentTime > _oppositeFoot.stepTimeLength * _oppositeStepStageOffset) //Start step
            {
                _grounded = false;
                _currentTime = 0f;
            }
            else
            {
                _position = _position * 1f; //FIX - slipping
                _rotation = _rotation * Quaternion.identity;
            }
            
        }
        else
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _stepTimeLength)
            {
                _currentTime = _stepTimeLength;
                _grounded = true;
                _prevGroundedPosition = nextGroundedPosition;

                _position = nextGroundedPosition.RemoveComponentAlongAxis(up) + _hip.position.ComponentAlongAxis(up) - _maximumLegLength * up;
                _rotation = _actor.transform.rotation;
            }
            else
            {
                float t = _currentTime / _stepTimeLength;
                _position = (Vector3.Lerp(_prevGroundedPosition, nextGroundedPosition, t)).RemoveComponentAlongAxis(up) + _hip.position.ComponentAlongAxis(up) + (_footArc.Evaluate(t) * speedProportion * _speedHeightScale - _maximumLegLength) * up;
                _rotation = Quaternion.Lerp(transform.rotation, _actor.transform.rotation, t);
            }
        }

        transform.position = _position;
        transform.rotation = _rotation;
    }

    Vector3 GetFootPlacement(Vector3 origin, float maxDistance)
    {
        RaycastHit[] hits = Physics.RaycastAll(origin, -up, maxDistance);
        foreach (RaycastHit hit in hits)
        {
            if (!_ignoreColliders.Contains(hit.transform.GetComponent<Collider>()))
            {
                _grounded = true;
                return hit.point;
            }
        }
        _grounded = false;
        return origin - up * maxDistance;
    }

    //position stays constant until it is far enough away from where the foot should be (offset from other feet)
    //check if terrain is moving as well
    //rotation should also be considered (rotation of ik constraint must be influenced by transform)
    //when foot is picked up, local rotation in direction of gravity should become body rotation
    //how far up to kick foot? what angle?
    //arc of foot moving forward - animation curve?
    //raycast down to the floor (or min of leg) and leave in place until cycle repeats
    //velocity should influence both speed of movement and the distance travelled
    //when stationary, place feet directly downwards with small arc (Vector3.Lerp)?
}