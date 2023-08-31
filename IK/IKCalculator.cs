using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IKCalculator //FIX - rotation constraints, squash, stretch, elastic, loose
{
    public static int recursionCap = 10;

    public static void Solve(List<RigPart> parts, Vector3 target, float errorMargin = 0.01f, float rotUrgency = 1f, float lenUrgency = 1f)
    {
        if (!InReach(parts, target))
        {
            foreach (RigPart part in parts)
            {
                part.anchor.LookAt(target);
            }
            return;
        }

        parts = Backward(parts, parts[0].anchor.position, target, errorMargin, rotUrgency, lenUrgency);
        return;
    }

    static bool InReach(List<RigPart> parts, Vector3 target)
    {
        float maxLength = 0f;
        foreach (RigPart part in parts)
        {
            maxLength += part.GetMaxLength();
        }
        if (maxLength > Vector3.Distance(parts[0].anchor.position, target))
        {
            return true;
        }
        return false;
    }

    static List<RigPart> Backward(List<RigPart> parts, Vector3 start, Vector3 target, float errorMargin = 0.01f, float rotUrgency = 1f, float lenUrgency = 1f, int currentRecursionDepth = 0)
    {
        parts.Reverse();

        List<Vector3> currentPositions = CopyRigPartPositions(parts); //begins at the end of the limb and end at the start
        List<Vector3> positions = new List<Vector3>(currentPositions);
        List<Vector3> newPositions = new List<Vector3>(currentPositions);

        positions.Add(start);
        newPositions.Insert(0, target);

        for (int i = 1; i < parts.Count; i++)
        {
            Vector3 heading = (positions[i - 1] - newPositions[i - 1]).normalized;
            newPositions[i] = newPositions[i - 1] + heading * parts[i - 1].GetDefaultLength();
        }

        parts.Reverse();
        newPositions.Reverse();

        newPositions.Add(target);

        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].anchor.transform.position = newPositions[i];
            parts[i].anchor.LookAt(newPositions[i + 1]);
        }

        return Forward(parts, start, target, errorMargin, rotUrgency, lenUrgency, currentRecursionDepth);
    }

    static List<RigPart> Forward(List<RigPart> parts, Vector3 start, Vector3 target, float errorMargin = 0.01f, float rotUrgency = 1f, float lenUrgency = 1f, int currentRecursionDepth = 0)
    {
        List<Vector3> currentPositions = CopyRigPartPositions(parts); //begins at the start of the limb and end at the end
        List<Vector3> positions = new List<Vector3>(currentPositions);
        List<Vector3> newPositions = new List<Vector3>(currentPositions);

        newPositions.Insert(0, start);

        for (int i = 1; i < parts.Count; i++)
        {
            Vector3 heading = (positions[i] - newPositions[i - 1]).normalized;
            newPositions[i] = newPositions[i - 1] + heading * parts[i].GetDefaultLength();
        }

        for (int i = 0; i < parts.Count; i++)
        {
            parts[i].anchor.transform.position = newPositions[i];
            parts[i].anchor.LookAt(newPositions[i+1]);
        }

        currentRecursionDepth += 1;

        if (Vector3.Distance(parts.Last().end.position, target) < errorMargin || currentRecursionDepth > recursionCap)
        {
            return parts;
        }
        return Backward(parts, start, target, errorMargin, rotUrgency, lenUrgency, currentRecursionDepth);
    }

    static List<Vector3> CopyRigPartPositions(List<RigPart> parts)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (RigPart part in parts)
        {
            positions.Add(part.anchor.position);
        }
        return positions;
    }

    //RigJoint resistance as multiplier for change in rotation
}
