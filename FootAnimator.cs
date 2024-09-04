using System.Collections.Generic;
using UnityEngine;

public class FootAnimator : MonoBehaviour
{
    [SerializeField] private PhysicActor _actor;
    [SerializeField] private Transform _hip;
    [SerializeField] private List<Collider> _ignoreColliders;
    [SerializeField] private SecondOrderAnimator _secondOrderAnimator = new SecondOrderAnimator();
    [SerializeField] private AnimationCurve _footArc;
    [SerializeField] private float _speedDistanceScale = 1f;
    [SerializeField] private float _speedHeightScale = 1f;
    [SerializeField] private float _minimumStepDistance = 0.1f;
    [SerializeField] private float _stepTimeLength = 0.1f;
    [SerializeField] private float _maximumLegLength = 0f;
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
        
        _secondOrderAnimator.SetPosition(_hip.position - up * _maximumLegLength); //FIX
    }

    public void Update()
    {
        _secondOrderAnimator.Next(_hip.position - up * _maximumLegLength); //FIX - use Vector manipulation to transfer height of position?

        float speedProportion = _actor.subjectiveVelocity.magnitude / _actor.moveMaxSpeed;

        if (Vector3.Distance(transform.position, _secondOrderAnimator.position) < _actor.subjectiveVelocity.magnitude * _speedDistanceScale / _actor.moveMaxSpeed + _minimumStepDistance && _grounded) //Maintain placement //FIX - rotation too, new parameter for resting leg length?
        {
            transform.position = _position;
            transform.rotation = _rotation;
            return;
        }

        if (_grounded && _oppositeFoot.currentTime > _oppositeFoot.stepTimeLength * 0.5f) //Start step
        {
            _grounded = false;
            _currentTime = 0f;

            transform.position = _position;
            transform.rotation = _rotation;
            return;
        }

        _currentTime += Time.deltaTime;

        if (_currentTime >= _stepTimeLength)
        {
            _currentTime = _stepTimeLength;
            _grounded = true;
            _prevGroundedPosition = _secondOrderAnimator.position;
        }

        float t = _currentTime / _stepTimeLength;
        _position = (Vector3.Lerp(_prevGroundedPosition, _secondOrderAnimator.position, t)).RemoveComponentAlongAxis(up) + _hip.position.ComponentAlongAxis(up) + (_footArc.Evaluate(t) * speedProportion * _speedHeightScale - _maximumLegLength) * up;

        transform.position = _position;
        transform.rotation = _rotation;
    }

    float GetLegLength(Transform parentBone)
    {
        if (parentBone.childCount == 0)
        {
            return 0f;
        }
        else
        {
            Transform childBone = parentBone.GetChild(0);
            return Vector3.Distance(parentBone.position, childBone.position) + GetLegLength(childBone);
        }
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