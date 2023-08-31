using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimationController : MonoBehaviour
{
    public string state = "idle";

    public Transform model;
    
    public List<string> colors; //0-primary 1-hair 2-clothes 3-accessory1 4-accessory2 5-accessory3
}   

