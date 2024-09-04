using System;
using UnityEngine;

[System.Serializable]
public static class SecondOrderDynamics
{
    public static void CalculateNextPosition(Vector3 inputPos, Vector3 inputVel, float deltaTime, float k1, float k2, float k3, ref Vector3 prevPos, ref Vector3 prevVel)
    {
        float k2Stabilized = Math.Max(k2, 0.55f * deltaTime * (deltaTime * 0.5f + k1));
        prevPos = prevPos + prevVel * deltaTime;
        prevVel = prevVel + deltaTime * (inputPos + k3 * inputVel - prevPos - k1 * prevVel) / k2Stabilized;
    }

    public static void InternalValuesToParameters(float k1, float k2, float k3, out float frequency, out float dampening, out float response)
    {
        frequency = 0.5f / ((float)Math.PI * (float)Math.Sqrt(k2));
        dampening = k1 * (float)Math.PI * frequency;
        response = (2f * k3) / k1;
    }

    public static void ParametersToInternalValues(float frequency, float dampening, float response, out float k1, out float k2, out float k3)
    {
        k1 = dampening / ((float)Math.PI * frequency);
        k2 = 0.25f / ((float)Math.PI * (float)Math.PI * frequency * frequency);
        k3 = response * dampening / (2f * (float)Math.PI * frequency);
    }

    public static void SanitizeParameters(ref float frequency, ref float dampening, ref float response)
    {
        frequency = frequency == 0f ? 0.0001f : Math.Abs(frequency);
        dampening = dampening == 0f ? 0.0001f : Math.Abs(dampening);
        response = Math.Clamp(response, -20f, 20f);
    }
}