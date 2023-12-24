using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPlacement : ProceduralAnimator
{
    [SerializeField] private Transform hip;
    [SerializeField] private Vector3 gravityDirection = Vector3.down;
    [SerializeField] private List<Collider> ignoreColliders;
    private float _legLength = 0f;
    public float legLength { get { return _legLength; } }
    private Vector3 position = Vector3.zero;
    private Quaternion rotation = Quaternion.identity;
    private bool _grounded = false;
    public bool grounded { get { return _grounded; } }

    void Start()
    {
        Initialize();
        Apply();
    }

    void Update()
    {
        Apply();
    }

    public void Initialize()
    {
        _legLength = GetLegLength(hip);
        position = subject.position;
        rotation = subject.rotation;
    }

    public void Apply()
    {
        GetFootPlacement(hip.position, _legLength); //updates _grounded

        subject.position = position;
        subject.rotation = rotation;
    }

    public bool GetNeedsMovement()
    {
        if (Vector3.Distance(hip.position, subject.position) > _legLength || !_grounded)
        {
            return true;
        }
        return false;
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
        RaycastHit[] hits = Physics.RaycastAll(origin, gravityDirection, maxDistance);
        foreach (RaycastHit hit in hits)
        {
            if (!ignoreColliders.Contains(hit.transform.GetComponent<Collider>()))
            {
                _grounded = true;
                return hit.point;
            }
        }
        _grounded = false;
        return origin + gravityDirection * maxDistance;
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
