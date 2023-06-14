using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFrame
{
    const float nullRep = 999999999f; // 
    float timeStamp;
    string smoothing; //"none", "i", "o", "io"
    string animFrame;
    Dictionary<string, Vector3> attributes;

    public KeyFrame(float timeStampARG = 0f, string smoothingARG = "none", string animFrameARG = "", 
        float rotationX = nullRep, float rotationY = nullRep, float rotationZ = nullRep, 
        float positionX = nullRep, float positionY = nullRep, float positionZ = nullRep, 
        float scaleX = nullRep, float scaleY = nullRep, float scaleZ = nullRep)
    {
        timeStamp = timeStampARG;
        smoothing = smoothingARG;
        animFrame = animFrameARG;
        attributes = new Dictionary<string, Vector3>() {
            {"rotation", new Vector3(rotationX, rotationY, rotationZ)},
            {"position", new Vector3(positionX, positionY, positionZ)},
            {"scale", new Vector3(scaleX, scaleY, scaleZ)},
        };
    }

    public float GetTimeStamp()
    {
        return timeStamp;
    }

    public string GetSmoothing()
    {
        return smoothing;
    }

    public string GetAnimFrame()
    {
        return animFrame;
    }

    private Vector3 GetAttribute(Vector3 currentAttribute, string attributeString)
    {
        Vector3 attribute = currentAttribute; //Nulls will not overwrite current attribute data
        Vector3 newAttribute = attributes[attributeString];

        if (newAttribute.x != nullRep)
            attribute.x = newAttribute.x;

        if (newAttribute.y != nullRep)
            attribute.y = newAttribute.y;

        if (newAttribute.z != nullRep)
            attribute.z = newAttribute.z;

        return attribute;
    }

    public Vector3 GetRotation(Vector3 currentRotation)
    {
        return GetAttribute(currentRotation, "rotation");
    }

    public Vector3 GetPosition(Vector3 currentPosition)
    {
        return GetAttribute(currentPosition, "position");
    }

    public Vector3 GetScale(Vector3 currentScale)
    {
        return GetAttribute(currentScale, "scale");
    }
}

public class Anim
{
    bool startIdle; //FIX - allow frame to be referenced from other animation
    bool endIdle; //^^^
    List<KeyFrame> keyFrames;

    public Anim(List<KeyFrame> keyFramesARG, bool startIdleARG = true, bool endIdleARG = true)
    {
        keyFrames = keyFramesARG;
        startIdle = startIdleARG;
        endIdle = endIdleARG;
    }

    public bool GetStartIdle()
    {
        return startIdle;
    }

    public bool GetEndIdle()
    {
        return endIdle;
    }

    public List<KeyFrame> GetKeyFrames()
    {
        return keyFrames;
    }
}