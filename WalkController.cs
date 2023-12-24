using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour
{
    [SerializeField] private List<List<FootPlacement>> syncedLegGroups = new List<List<FootPlacement>>() { };
    [SerializeField] private AnimationCurve footArc;

    public void Update()
    {
        
//-When movement starts, move first leg group immediately
//-Get next placement by adding velocity of object* current placement to the current spot the hip is over and use raycast
//-Desync legs by dividing the time per cycle(animation curve) by the number of synced leg groups
//-Use animation curve to determine arc of foot
//-Multiply curve length by velocity to determine leg speed
    }
}
