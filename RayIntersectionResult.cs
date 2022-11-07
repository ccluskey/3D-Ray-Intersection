using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to hold the result of a ray intersection check. Contains the following
// bool intersects            - True if the rays intersect, false if they do not
// Vector3? intersectionPoint - If 'intersects' is true, this is the intersection point
//                              If 'intersects' is false, and the lines are not parallel, this is 
//                              the point such that the total distance from each ray is minimized
//                              NULL if the lines are parallel
// float distance             - If 'intersects' is true, 0
//                            - If 'intersects' is false, the minimum distance between to two rays
// bool positiveSkewPoint     - True if the point where the distance between the two rays is minimuzed is
//                              in the positive direction on both vectors
public class RayIntersectionResult
{
    
    private bool intersects;
    private Vector3? intersectionPoint;
    private float distance;
    private bool positiveSkewPoint;


    public RayIntersectionResult(bool intersects, Vector3? intersectionPoint, float distance, bool positiveSkewPoint)
    {
        this.intersects = intersects;
        this.intersectionPoint = intersectionPoint;
        this.distance = distance;
        this.positiveSkewPoint = positiveSkewPoint;
    }

    public bool getIntersects()
    {
        return intersects;
    }

    public Vector3? getIntersectionPoint()
    {
        return intersectionPoint;
    }

    public float getDistance()
    {
        return distance;
    }

    public bool getPositiveSkewPoint()
    {
        return positiveSkewPoint;
    }
}