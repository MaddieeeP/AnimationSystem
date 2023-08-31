using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RigJoint : MonoBehaviour
{
    Vector3 rotDefault = new Vector3(0f, 0f, 0f);
    Vector3 rotMin = new Vector3(-180f, -180f, -180f); //corrects for rotDefault
    Vector3 rotMax = new Vector3(0f, 0f, 0f); //corrects for rotDefault

    float _elastic = 0f;
    public float elastic //priority of returning to default rotation
    {
        get
        {
            if (loose)
            {
                return 0f;
            }
            return _elastic;
        }
        set
        {
            _elastic = value;
        }
    }

    float _resistance = 0f;
    public float resistance //priority of maintaining rotation
    {
        get
        {
            if (loose)
            {
                return 0f;
            }
            return _resistance;
        }
        set
        {
            _resistance = value;
        }
    }

    public bool loose = false; //under effect of gravity - still respects resistance

    public Vector3 position
    {
        get { return transform.position; }
    }

    public Vector3 localPosition
    {
        get { return transform.localPosition; }
    }

    public void RotateTo(Vector3 newRot)
    {
        transform.rotation = Quaternion.Euler(ClampRotation(newRot));
    }

    public void LookAt(Vector3 target)
    {
        transform.LookAt(target, transform.up);
        transform.rotation = Quaternion.Euler(ClampRotation(transform.eulerAngles));
    }

    public void LookAt(Vector3 target, Vector3 up)
    {
        transform.LookAt(target, up);
        transform.rotation = Quaternion.Euler(ClampRotation(transform.eulerAngles));
    }

    public Vector3 ClampRotation(Vector3 newRot)
    {
        newRot.StandardizeRotation();

        Vector3 min = rotDefault + rotMin;
        Vector3 max = rotDefault + rotMax;

        return new Vector3(Math.Clamp(newRot.x, min.x, max.x), Math.Clamp(newRot.y, min.y, max.y), Math.Clamp(newRot.z, min.z, max.z));
    }
}
