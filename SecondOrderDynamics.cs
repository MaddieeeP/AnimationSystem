using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDynamics
{
    private Vector3 lastInputPos;
    private Vector3 position;
    private Vector3 velocity;
    private float k1;
    private float k2;
    private float k3;

    public SecondOrderDynamics(float f, float z, float r, Vector3 initialInputPos)
    {
        SetParameters(f, z, r); 

        lastInputPos = initialInputPos;
        position = initialInputPos;
        velocity = Vector3.zero;
    }

    public void SetParameters(float f, float z, float r)
    {
        f = Math.Abs(f);
        z = Math.Min(Math.Abs(z), 20f);
        
        k1 = z / ((float)Math.PI * f);
        k2 = 1f / ((2f * (float)Math.PI * f) * (2f * (float)Math.PI * f));
        k3 = r * z / (2f * (float)Math.PI * f);
    }

    public Vector3 Next(Vector3 inputPos, Vector3 inputVel, float deltaTime)
    {
        lastInputPos = inputPos;

        float k2Stabilized = Math.Max(k2, 1.1f * deltaTime * (deltaTime / 4f + k1 / 2f));
        position = position + velocity * deltaTime;
        velocity = velocity + deltaTime * (inputPos + k3 * inputVel - position - k1 * velocity) / k2Stabilized;
        return position;
    }

    public Vector3 Next(Vector3 inputPos) => Next(inputPos, (inputPos - lastInputPos) / Time.deltaTime, Time.deltaTime);

    public Vector3 Next(Vector3 inputPos, Vector3 inputVel) => Next(inputPos, inputVel, Time.deltaTime);
} 
