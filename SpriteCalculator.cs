using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCalculator : MonoBehaviour
{
    public bool billboard = true;

    public Transform cameraTransform;
    public List<Sprite> spriteList;
    public List<Quaternion> spriteRotations;
    public GameObject spriteObj;

    public Quaternion relativeRotation
    {
        get { return transform.rotation.DivideBy(cameraTransform.rotation); } //relative to camera
    }

    void Awake()
    {
        foreach (Sprite sprite in spriteList)
        {
            string[] rots = Regex.Split(sprite.name, ",");
            Quaternion rotation = Quaternion.Euler(new Vector3(int.Parse(rots[0]), int.Parse(rots[1]), int.Parse(rots[2])));
            spriteRotations.Add(rotation);
        }
    }

    void LateUpdate()
    {
        SpriteUpdate();
    }



    void SpriteUpdate()
    {
        int indexOfClosest = relativeRotation.FindClosest(spriteRotations);
        Quaternion closestRot = spriteRotations[indexOfClosest]; 

        spriteObj.GetComponent<SpriteRenderer>().sprite = spriteList[indexOfClosest];

        Debug.Log(relativeRotation.DivideBy(relativeRotation.DivideBy(closestRot) * closestRot));

        if (billboard)
        {
            spriteObj.transform.rotation = cameraTransform.rotation;//FIX - ~ + Vector3.forward * relativeRot.z);
        } else
        {
            spriteObj.transform.rotation = cameraTransform.rotation;
        }
    }
}