using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticMethods 
{
    public static float Squared(this float num)
    {
        return num * num;
    }

    public static Quaternion DivideBy(this Quaternion quaternion, Quaternion divisor)
    {
        return Quaternion.Inverse(divisor) * quaternion;
    }

    public static Quaternion RotationComponentAboutAxis(this Quaternion rotation, Vector3 direction)
    {
        Vector3 rotationAxis = new Vector3(rotation.x, rotation.y, rotation.z);
        float dotProd = (float)Vector3.Dot(direction, rotationAxis);
        Vector3 projection = dotProd * direction;

        Quaternion twist = new Quaternion(projection.x, projection.y, projection.z, rotation.w);
        if (dotProd < 0f)
        {
            twist.x = -twist.x;
            twist.y = -twist.y;
            twist.z = -twist.z;
            twist.w = -twist.w;
        }
        return twist;
    }

    public static Vector3 FindClosest(this Quaternion quaternion, List<Vector3> vectors)
    {
        float minAngle = float.MaxValue;

        int indexOfBest = 0;

        for (int i = 0; i < vectors.Count; i++)
        {
            Quaternion spriteRotation = Quaternion.Euler(vectors[i]);
            Quaternion finalRot = quaternion.DivideBy(spriteRotation) * spriteRotation; //.RotationComponentAboutAxis(vectors[i])

            float angle = Vector3.Angle(finalRot * new Vector3(1f, 1f, 1f), quaternion * new Vector3(1f, 1f, 1f));
            if (angle < minAngle)
            {
                indexOfBest = i;
                minAngle = angle;
            }
        }

        return vectors[indexOfBest];
    }

    public static Vector3 FindClosestNo(this Quaternion quaternion, List<Vector3> vectors)
    {
        float minDist = float.MaxValue;

        int indexOfBest = 0;

        for (int i = 0; i < vectors.Count; i++)
        {
            Quaternion spriteRotation = Quaternion.Euler(vectors[i]);
            Quaternion finalRot = quaternion.DivideBy(spriteRotation).RotationComponentAboutAxis(vectors[i]) * spriteRotation;

            float dist = Vector3.Distance(finalRot * new Vector3(1f, 1f, 1f), quaternion * new Vector3(1f, 1f, 1f));
            if (dist < minDist)
            {
                indexOfBest = i;
                minDist = dist;
            }
        }

        return vectors[indexOfBest];
    }

    public static Vector3 StandardizeRotation(this Vector3 rotation)
    {
        while (rotation.x <= -180f)
        {
            rotation += new Vector3(360f, 0f, 0f);
        }
        while (rotation.x > 180f)
        {
            rotation -= new Vector3(360f, 0f, 0f);
        }
        while (rotation.y <= -180f)
        {
            rotation += new Vector3(0f, 360f, 0f);
        }
        while (rotation.y > 180f)
        {
            rotation -= new Vector3(0f, 360f, 0f);
        }
        while (rotation.z <= -180f)
        {
            rotation += new Vector3(0f, 0f, 360f);
        }
        while (rotation.z > 180f)
        {
            rotation -= new Vector3(0f, 0f, 360f);
        }

        return rotation;
    }

    public static float StandardizedAxisDistance(this float rotation, float targetRot)
    {
        float difference = 180f;
        difference = Mathf.Min(difference, (float)Math.Abs(targetRot - rotation));
        difference = Mathf.Min(difference, (float)Math.Abs(targetRot - (rotation - 360f)));
        difference = Mathf.Min(difference, (float)Math.Abs(targetRot - (rotation + 360f)));
        return difference;
    }

    public static float StandardizedDistance(this Vector3 rotation, Vector3 targetRotation)
    {
        Vector3 difference = default(Vector3);
        difference.x = rotation.x.StandardizedAxisDistance(targetRotation.x);
        difference.y = rotation.y.StandardizedAxisDistance(targetRotation.y);
        difference.z = rotation.z.StandardizedAxisDistance(targetRotation.z);
        return difference.magnitude;
    }

    public static float StandardizedDistance(this Quaternion rotation, Quaternion targetRotation)
    {
        Vector3 rot = rotation.eulerAngles;
        Vector3 targetRot = targetRotation.eulerAngles;

        Vector3 difference = default(Vector3);
        difference.x = rotation.x.StandardizedAxisDistance(targetRot.x);
        difference.y = rotation.y.StandardizedAxisDistance(targetRot.y);
        difference.z = rotation.z.StandardizedAxisDistance(targetRot.z);
        return difference.magnitude;
    }

    public static Vector3 ComponentInDirection(this Vector3 vector, Vector3 direction)
    {
        float angle = (float)Math.Acos(Vector3.Dot(vector, direction) / vector.magnitude / direction.magnitude);
        Vector3 component = direction.normalized * vector.magnitude * (float)Math.Cos(angle);
        if (Double.IsNaN((double)component.magnitude))
        {
            return new Vector3(0f, 0f, 0f);
        }
        return component;
    }

    public static Vector3 RemoveComponentInDirection(this Vector3 vector, Vector3 direction)
    {
        return vector - vector.ComponentInDirection(direction);
    }

    public static Vector3 ToTransDirection(this Vector3 vector, Transform transform)
    {
        Vector3 vectorInDirection = transform.right * vector.x + transform.up * vector.y + transform.forward * vector.z;
        return vectorInDirection;
    }

    public static float AngleFrom(this Vector3 vector, Vector3 direction, bool absolute = false)
    {
        //vector = vector.normalized;
        //direction = direction.normalized;   

        float angle = (Math.Acos(Vector3.Dot(vector, direction) / vector.magnitude / direction.magnitude)).RadiansToDegrees();
        if (Double.IsNaN((double)angle))
        {
            angle = 0f;
        }

        if (!vector.IsComponentInDirectionPositive(direction))
        {
            angle = 180f - angle;
        }

        Vector3 perpendicular = vector.RemoveComponentInDirection(direction).normalized;
        if (!absolute && !perpendicular.IsComponentInDirectionPositive(new Vector3(1f, 1f, 1f)))
        {
            return - angle;
        }

        return angle;
    }

    public static float RadiansToDegrees(this double angle)
    {
        return (float)(angle / Math.PI) * 180f;
    }

    public static Vector3 FlattenAgainstDirection(this Vector3 vector, Vector3 direction)
    {
        float vectorMagnitude = vector.magnitude;
        vector = vector.normalized;
        vector -= vector.ComponentInDirection(direction);
        return vector.normalized * vectorMagnitude;
    }

    public static bool IsComponentInDirectionPositive(this Vector3 vector, Vector3 direction)
    {
        if (vector.ComponentInDirection(direction).normalized == direction.normalized)
        {
            return true;
        }
        return false;
    }

    public static Vector3 CalculateForceToReachVelocity(this Rigidbody rigidbody, Vector3 targetVelocity, float deltaTime = 0.01f)
    {
        return (rigidbody.mass * targetVelocity) - (rigidbody.mass * rigidbody.velocity) / deltaTime;
    }
}
