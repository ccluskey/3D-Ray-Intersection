using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RayIntersectionTests
{
    private static readonly float MARGIN_OF_ERROR = 0.00001F;
    [Test]
    public void SimpleIntersectingRays()
    {
        Vector3 ray1Start = new Vector3(0,1,0);
        Vector3 ray1Direction = new Vector3(1,0,1);
        Vector3 ray2Start = new Vector3(1,0,0);
        Vector3 ray2Direction = new Vector3(0,1,1);
        Vector3 expectedIntersection = new Vector3(1,1,1);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);

        Assert.True(computedIntersection.getIntersects());
        Assert.AreEqual(expectedIntersection, computedIntersection.getIntersectionPoint());
        Assert.AreEqual(0, computedIntersection.getDistance());
    }

    [Test]
    public void IntersectingBehindDoesNotCount()
    {
        Vector3 ray1Start = new Vector3(0,1,0);
        Vector3 ray1Direction = new Vector3(-1,0,-1);
        Vector3 ray2Start = new Vector3(1,0,0);
        Vector3 ray2Direction = new Vector3(0,1,1);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);

        Assert.False(computedIntersection.getIntersects());
    }

    [Test]
    public void SimpleNonIntersectingRays()
    {
        Vector3 ray1Start = new Vector3(0,1,1);
        Vector3 ray1Direction = new Vector3(1,0,0);
        Vector3 ray2Start = new Vector3(1,0,0);
        Vector3 ray2Direction = new Vector3(0,1,0);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);
        float expectedDistance = 1;

        Assert.False(computedIntersection.getIntersects());
        Assert.IsTrue(Mathf.Abs(expectedDistance - computedIntersection.getDistance()) < MARGIN_OF_ERROR);
    }

    [Test]
    public void ComplexNonIntersectingRays()
    {
        Vector3 ray1Start = new Vector3(-3, -8, 7);
        Vector3 ray1Direction = new Vector3(0,-1,1);
        Vector3 ray2Start = new Vector3(6,3,0);
        Vector3 ray2Direction = new Vector3(-4,-3,0);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);
        float expectedDistance = 11 / Mathf.Sqrt(41);

        Assert.False(computedIntersection.getIntersects());
        Assert.IsTrue(Mathf.Abs(expectedDistance - computedIntersection.getDistance()) < MARGIN_OF_ERROR);
    }

    [Test]
    public void ComplexIntersectingRays()
    {
        Vector3 ray1Start = new Vector3(-2,-1,0);
        Vector3 ray1Direction = new Vector3(1,1,1);
        Vector3 ray2Start = new Vector3(8,-6,-11);
        Vector3 ray2Direction = new Vector3(-2,3,5);
        Vector3 expectedIntersection = new Vector3(2,3,4);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);

        Assert.True(computedIntersection.getIntersects());
        Assert.IsTrue(Mathf.Abs(expectedIntersection.x - computedIntersection.getIntersectionPoint().Value.x) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(expectedIntersection.y - computedIntersection.getIntersectionPoint().Value.y) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(expectedIntersection.z - computedIntersection.getIntersectionPoint().Value.z) < MARGIN_OF_ERROR);
        Assert.AreEqual(0, computedIntersection.getDistance());
    }

    [Test]
    public void SimpleSkewLines()
    {
        Vector3 ray1Start = new Vector3(0,4,0);
        Vector3 ray1Direction = new Vector3(1,0,1);
        Vector3 ray2Start = new Vector3(2,0,0);
        Vector3 ray2Direction = new Vector3(-1,0,1);
        Vector3 expectedIntersection = new Vector3(1,2,1);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);

        Assert.False(computedIntersection.getIntersects());
        Assert.IsTrue(Mathf.Abs(expectedIntersection.x - computedIntersection.getIntersectionPoint().Value.x) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(expectedIntersection.y - computedIntersection.getIntersectionPoint().Value.y) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(expectedIntersection.z - computedIntersection.getIntersectionPoint().Value.z) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(4 - computedIntersection.getDistance()) < MARGIN_OF_ERROR);
        Assert.IsTrue(computedIntersection.getPositiveSkewPoint());
    }

    [Test]
    public void NegativeSkewPoint()
    {
        Vector3 ray1Start = new Vector3(2,4,2);
        Vector3 ray1Direction = new Vector3(1,0,1);
        Vector3 ray2Start = new Vector3(2,0,0);
        Vector3 ray2Direction = new Vector3(-1,0,1);
        Vector3 expectedIntersection = new Vector3(1,2,1);
        RayIntersectionResult computedIntersection = RayIntersection.FindRayIntersection(ray1Start, ray1Direction, ray2Start, ray2Direction);

        Assert.False(computedIntersection.getIntersects());
        Assert.IsTrue(Mathf.Abs(expectedIntersection.x - computedIntersection.getIntersectionPoint().Value.x) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(expectedIntersection.y - computedIntersection.getIntersectionPoint().Value.y) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(expectedIntersection.z - computedIntersection.getIntersectionPoint().Value.z) < MARGIN_OF_ERROR);
        Assert.IsTrue(Mathf.Abs(4 - computedIntersection.getDistance()) < MARGIN_OF_ERROR);
        Assert.IsFalse(computedIntersection.getPositiveSkewPoint());
    }
}
