using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class BlockUtilities
{

    public Vector3 CalculateRoundedPoint(RaycastHit hit, Vector3 SnapFactors, Vector3 OffsetFactors)
    {
        Vector3 point = hit.point;
        Vector3 MyNormal = hit.normal;
        Vector3 pointRounted = new Vector3();

        //this will convert that normal from being relative to global axis to relative to an
        //objects local axis

        MyNormal = hit.transform.TransformDirection(MyNormal);

        //this next line will compare the normal hit to the normals of each plane to find the 
        //side hit
        float partialX = point.x / SnapFactors.x - Mathf.Floor(point.x / SnapFactors.x);
        float partialY = point.y / SnapFactors.y - Mathf.Floor(point.y / SnapFactors.x);
        float partialZ = point.z / SnapFactors.z - Mathf.Floor(point.z / SnapFactors.x);

        if (MyNormal == hit.transform.up)
        {
            Debug.Log("top");
            if (partialX > 0.5f)
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            else
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);

            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == -hit.transform.up) //important note the use of the '-' sign this inverts the direction, -up == down. Down doesn't exist as a stored direction, you invert up to get it. 
        {
            Debug.Log("bottom");
            if (partialX > 0.5f)
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            else
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y - OffsetFactors.y);

            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == hit.transform.right)
        {
            Debug.Log("hit from right");
            pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);
            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == -hit.transform.right) //note the '-' sign converting right to left
        {
            Debug.Log("hit from left");
            pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);

            if (partialZ > 0.5f)
            {
                pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
            else
            {
                pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
            }
        }
        else if (MyNormal == -hit.transform.forward)
        {
            Debug.Log("hit from forward");
            if (partialX > 0.5f)
            {
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }
            else
            {
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);
            pointRounted.z = (float)(System.Math.Floor(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);

        }
        else if (MyNormal == hit.transform.forward)
        {
            Debug.Log("hit from behind");
            if (partialX > 0.5f)
            {
                pointRounted.x = (float)(System.Math.Ceiling(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }
            else
            {
                pointRounted.x = (float)(System.Math.Floor(point.x / SnapFactors.x) * SnapFactors.x + OffsetFactors.x);
            }

            pointRounted.y = (float)(System.Math.Floor(point.y / SnapFactors.y) * SnapFactors.y + OffsetFactors.y);
            pointRounted.z = (float)(System.Math.Ceiling(point.z / SnapFactors.z) * SnapFactors.z + OffsetFactors.z);
        }

        return pointRounted;
    }

}