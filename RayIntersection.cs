using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayIntersection : MonoBehaviour
{
    private static readonly float MARGIN_OF_ERROR = 0.00001F;
    public static RayIntersectionResult FindRayIntersection(Vector3 ray1Origin, Vector3 ray1Direction,
                                   Vector3 ray2Origin, Vector3 ray2Direction)
    {
        // Find intersection on the x y plane
        float[,] matrixXY = 
        {
            {ray2Direction.x, -1 * ray1Direction.x},
            {ray2Direction.y, -1 * ray1Direction.y}
        };

        if (MatrixTools.Matrix2x2Determinant(matrixXY) != 0)
        {
            // A^-1*b gives the intersection point
            float[,] matrixXYInverse = MatrixTools.Matrix2x2Inverse(matrixXY);

            // Multiply them manually
            float[] result = {
                matrixXYInverse[0,0] * (ray1Origin.x - ray2Origin.x) + matrixXYInverse[0,1] * (ray1Origin.y - ray2Origin.y),
                matrixXYInverse[1,0] * (ray1Origin.x - ray2Origin.x) + matrixXYInverse[1,1] * (ray1Origin.y - ray2Origin.y)
            };

            // Check if z also intersects
            float ray1Z = ray1Origin.z + result[1] * ray1Direction.z;
            float ray2Z = ray2Origin.z + result[0] * ray2Direction.z;

            if (Mathf.Abs(ray1Z - ray2Z) < MARGIN_OF_ERROR)
            {
                // Also verify result matrix is positive, otherwise we intersected behind where the ray started
                if (result[0] >= 0 && result[1] >= 0)
                {
                    return new RayIntersectionResult(
                        true,
                        new Vector3(
                            ray1Origin.x + result[1] * ray1Direction.x,
                            ray1Origin.y + result[1] * ray1Direction.y,
                            ray1Origin.z + result[1] * ray1Direction.z
                        ),
                        0,
                        true
                    );
                }
            }
        }

        // The lines don't intersect, calculate the distance between them
        Vector3 ray2ToRay1 = new Vector3(ray1Origin.x - ray2Origin.x, ray1Origin.y - ray2Origin.y, ray1Origin.z - ray2Origin.z);

        Vector3 normalVector = Vector3.Cross(ray1Direction, ray2Direction);

        // distance = normal dot ray1Toray2 / |normal|
        float normalVectorMagnitude = Mathf.Sqrt(normalVector.x * normalVector.x + normalVector.y * normalVector.y + normalVector.z * normalVector.z );
        float distance = Mathf.Abs(ray2ToRay1.x * normalVector.x + ray2ToRay1.y * normalVector.y + ray2ToRay1.z * normalVector.z) / normalVectorMagnitude;

        // Determine the point at which the total distance between the two lines is minimized
        // This will be returned in the 'intersectionPoint' field
        // See https://math.stackexchange.com/a/1993990
        float[,] matrixA = new float[,]
        {
            {ray1Direction.x, (-1) * ray2Direction.x, normalVector.x},
            {ray1Direction.y, (-1) * ray2Direction.y, normalVector.y},
            {ray1Direction.z, (-1) * ray2Direction.z, normalVector.z}
        };

        if (MatrixTools.MatrixDeterminant(matrixA) != 0)
        {
            float[,] matrixAInverse = MatrixTools.Matrix3x3Inverse(matrixA);
            float[] matrixB = new float[] { ray2Origin.x - ray1Origin.x, ray2Origin.y - ray1Origin.y, ray2Origin.z - ray1Origin.z };

            // A inverse * b but we only need the first two terms
            float distanceAlongRay1 = matrixAInverse[0,0] * matrixB[0] + matrixAInverse[0,1] * matrixB[1] + matrixAInverse[0,2] * matrixB[2];
            float distanceAlongRay2 = matrixAInverse[1,0] * matrixB[0] + matrixAInverse[1,1] * matrixB[1] + matrixAInverse[1,2] * matrixB[2];

            // Find the point on both lines.  The midpoint of these two points is our answer!
            Vector3 pointOnRay1 = ray1Origin + distanceAlongRay1 * ray1Direction;
            Vector3 pointOnRay2 = ray2Origin + distanceAlongRay2 * ray2Direction;

            return new RayIntersectionResult(false, Vector3.Lerp(pointOnRay1, pointOnRay2, 0.5f), distance, distanceAlongRay1 >= 0 && distanceAlongRay2 >= 0);
        }

        // The lines must be parallel
        return new RayIntersectionResult(false, null, distance, false);
    }
}
