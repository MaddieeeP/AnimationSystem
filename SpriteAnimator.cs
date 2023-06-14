using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimatorAttribute
{
    float lerpCounter;
    float transitionTime;
    Vector3 attributeOrigin;
    Vector3 attributeTarget;

    public SpriteAnimatorAttribute(Vector3 attributeARG)
    {
        Set(attributeARG);
    }

    public void Set(Vector3 attributeTargetARG)
    {
        transitionTime = 0f;
        lerpCounter = 1f;
        attributeOrigin = attributeTargetARG;
        attributeTarget = attributeTargetARG;
    }

    public void Transition(Vector3 attributeCurrent, Vector3 attributeTargetARG, float transitionTimeARG)
    {
        transitionTime = transitionTimeARG;
        attributeOrigin = attributeCurrent;
        attributeTarget = attributeTargetARG;
        lerpCounter = 0f;
    }

    private Vector3 CurrentTarget() //Called from CurrentAttribute //Called every frame
    {
        if (transitionTime == 0f)
            return attributeTarget;

        lerpCounter += Time.deltaTime / transitionTime;
        if (lerpCounter > 1f)
            lerpCounter = 1f;

        return Vector3.Lerp(attributeOrigin, attributeTarget, lerpCounter);
    }

    public Vector3 CurrentAttribute(Vector3 attributeCurrent) //Call manually from Update() in SpriteAnimator //Allows animations to interrupt and flow better
    {
        float dist = Vector3.Distance(attributeCurrent, Vector3.Lerp(attributeOrigin, attributeTarget, lerpCounter));
        float lerpValue = 2f / (dist+0.2f);
        if (lerpValue > 1f)
            lerpValue = 1f;

        return Vector3.Lerp(attributeCurrent, CurrentTarget(), lerpValue);
    }
}

public class SpriteAnimator : MonoBehaviour
{
    Anim anim;
    float animTime = 0f;
    Dictionary<string, SpriteAnimatorAttribute> attributes;

    public void Start()
    {
        attributes = new Dictionary<string, SpriteAnimatorAttribute>() {
            {"rotation", new SpriteAnimatorAttribute(new Vector3(0f,0f,0f))},
            {"position", new SpriteAnimatorAttribute(new Vector3(0f,0f,0f))},
            {"scale", new SpriteAnimatorAttribute(new Vector3(1f,1f,1f))},
        };
    }

    public void Update()
    {
        if (anim == null)
            return;

        animTime += Time.deltaTime; //FIX - variable speed

        Vector3 currentRot = gameObject.transform.localEulerAngles;
        Vector3 currentPos = gameObject.transform.localPosition;
        Vector3 currentScale = gameObject.transform.localScale;

        //FIX - add start animation frame functionality
        foreach (KeyFrame frame in anim.GetKeyFrames())
        {
            if (!Que(frame))
                continue;
            attributes["rotation"].Transition(currentRot, frame.GetRotation(currentRot), 1f); //FIX - transition time
            attributes["position"].Transition(currentPos, frame.GetPosition(currentPos), 1f);
            attributes["scale"].Transition(currentScale, frame.GetScale(currentScale), 1f);

            break;
        }

        gameObject.transform.localRotation = Quaternion.Euler(attributes["rotation"].CurrentAttribute(currentRot));
        gameObject.transform.localPosition = attributes["position"].CurrentAttribute(currentPos);
        gameObject.transform.localScale = attributes["scale"].CurrentAttribute(currentScale);
    }

    public bool Que(KeyFrame frame)
    {
        float timeStamp = frame.GetTimeStamp();
        if (timeStamp < animTime && timeStamp + Time.deltaTime >= animTime) //FIX - more accurate? - no skip, no repeat
            return true;
        return false;
    }

    public void Animate(Anim animARG)
    {
        anim = animARG;
        animTime = 0f;
    }
}   