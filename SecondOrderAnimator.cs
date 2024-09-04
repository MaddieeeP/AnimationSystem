using UnityEngine;

[System.Serializable]
public class SecondOrderAnimator
{
    [SerializeField] protected float _k1;
    [SerializeField] protected float _k2;
    [SerializeField] protected float _k3;

    private Vector3 _prevInputPos;
    private Vector3 _position;
    private Vector3 _velocity;

    //getters and setters
    public float k1 { get { return _k1; } set { _k1 = value; } }
    public float k2 { get { return _k2; } set { _k2 = value; } }
    public float k3 { get { return _k3; } set { _k3 = value; } }
    public Vector3 position { get { return _position; } }
    public Vector3 velocity { get { return _velocity; } }

    public void SetPosition(Vector3 value)
    {
        _prevInputPos = value;
        _position = value;
        _velocity = Vector3.zero;
    }

    public void Next(Vector3 inputPos, float deltaTime)
    {
        SecondOrderDynamics.CalculateNextPosition(inputPos, (inputPos - _prevInputPos) / deltaTime, deltaTime, _k1, _k2, _k3, ref _position, ref _velocity);
        _prevInputPos = inputPos;
    }

    public void Next(Vector3 inputPos) => Next(inputPos, Time.deltaTime);
}