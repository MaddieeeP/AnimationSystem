using UnityEngine;

public class LookAtTarget : MonoBehaviour
{

}
//intended behaviour decouple rotation from parent, restrict viewing x and y frustum relative to specified transform
//when target moves, movement should be procedural, when parent moves, tracking should not be disturbed
//virtual vector3 field to get look at target in world space
//calculate local space coordinates
//use producural animation result as local target
//set rotation to lookrotation local target with virtual up field clamped within frustum