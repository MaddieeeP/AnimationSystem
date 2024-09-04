using UnityEngine;

public class Reach : MonoBehaviour
{
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private bool ignoreBounds = false;
    [SerializeField] private Bounds bounds;

    public void ReachFor(Vector3 position)
    {
        _targetPosition = position;
    }

    public void ReachFor(Transform transform) => ReachFor(transform.position);

    public void Update()
    {
        if (_targetPosition == null)
        {
            return;
        }

        if (bounds == null || ignoreBounds)
        {
            //Next(_targetPosition);
        }
        else
        {
            //position = position.ClampInBounds(bounds.min, bounds.max);
            //position = DynamicsNext(position);
            //position = position.ClampInBounds(bounds.min, bounds.max);
        }
        //transform.position = position;
    }
}