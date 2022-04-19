using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Helpers
{
    public static Quaternion SetZRotation(this Quaternion oldRot, float newZRot)
    {
        var newX = oldRot.x;
        var newY = oldRot.y;
        var newZ = newZRot;
        var newW = oldRot.w;

        var newRot = new Quaternion(newX, newY, newZ, newW);
        return newRot;

    }

        public static void ClampRBSpeed(this Rigidbody rb, float maxSpeed)
        {
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxSpeed;
            }
        }

        private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

        public static WaitForSeconds GetWait(float time)
        {
            if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

            WaitDictionary[time] = new WaitForSeconds(time);
            return WaitDictionary[time];

        }
}

    


