using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigPart : MonoBehaviour
{
    public RigJoint anchor
    {
        get { return transform.parent.GetComponent<RigJoint>(); }
    }
    public RigJoint end
    {
        get { return transform.GetChild(1).GetComponent<RigJoint>(); }
    }
    public Vector3 position
    {
        get { return transform.position; }
    }
    public RigPart partParent
    {
        get 
        {
            if (anchor == null)
            {
                return null;
            }

            if (anchor.transform.parent.TryGetComponent<RigPart>(out RigPart parent))
                return parent; 

            return null;
        }
    }

    public bool persistentLength = true;

    float _stretch = 0f;
    public float stretch //max stretched length multiplier
    {
        get
        {
            if (persistentLength)
            {
                return 0f;
            }
            return _stretch;
        }
        set
        {
            _stretch = value;
        }
    }

    float _squash = 0f;
    public float squash //max squashed length multiplier
    {
        get
        {
            if (persistentLength)
            {
                return 0f;
            }
            return _squash;
        }
        set
        {
            _squash = value;
        }
    }

    float _elastic = 1f;
    float elastic //priority of returning to default length
    {
        get
        {
            if (persistentLength)
            {
                return 1f;
            }
            return _elastic;
        }
        set
        {
            _elastic = value;
        }
    }

    public float weight = 10f; //approximation in kilograms

    float currentLengthChange = 0f; //always >-1

    public float GetCurrentLength()
    {
        return Vector3.Distance(anchor.transform.position, end.transform.position);
    }

    public float GetDefaultLength()
    {
        return GetCurrentLength() / (1 + currentLengthChange);
    }

    public float GetMaxLength()
    {
        return GetDefaultLength() * (1f + stretch);
    }

    public float GetMinLength()
    {
        return GetDefaultLength() * (1f - squash);
    }

    public List<RigPart> GetLimb()
    {
        List<RigPart> limb = new List<RigPart>();

        if (partParent != null)
        {
            foreach (RigPart part in partParent.GetLimb())
            {
                limb.Add(part);
            }
        }

        limb.Add(this);

        return limb;
    }
}