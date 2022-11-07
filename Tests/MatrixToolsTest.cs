using System;
using NUnit.Framework;
using UnityEngine;

public class MatrixToolsTest
{
    private static readonly float MARGIN_OF_ERROR = 0.00001F;

    [Test]
    public void TestValid2x2MatrixInverse()
    {
        float[,] matrix = 
        {
            {-2, 1},
            {5, -3}
        };
        float[,] matrixInverse = 
        {
            {-3, -1},
            {-5, -2}
        };

        float[,] result = MatrixTools.Matrix2x2Inverse(matrix);

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Assert.IsTrue(Mathf.Abs(matrixInverse[i,j] - result[i,j]) < MARGIN_OF_ERROR);
            }
        }
    }

    [Test]
    public void TestInvalid2x2MatrixInverse()
    {
        float[,] matrix = 
        {
            {1, 2},
            {2, 4}
        };
        float[,] matrixInverse = 
        {
            {1, 2},
            {2, 4}
        };

        // If there is no inverse, the original matrix is returned
        Assert.AreEqual(MatrixTools.Matrix2x2Inverse(matrix), matrixInverse);
    }

    [Test]
    public void Test2x2MatrixDeterminant()
    {
        float[,] matrix = 
        {
            {1, 2},
            {2, 4}
        };
        // det([a, b], [c, d]) = a*d - c*d
        float determinant = 0;

        Assert.AreEqual(determinant, MatrixTools.Matrix2x2Determinant(matrix));
    }

    [Test]
    public void Test3x3MatrixDeterminant()
    {
        float[,] matrix = 
        {
            {1, 2, 4},
            {2, -1, 3},
            {4, 0, 1}
        };
        float determinant = 35;

        Assert.AreEqual(determinant, MatrixTools.MatrixDeterminant(matrix));
    }

        [Test]
    public void TestAnother3x3MatrixDeterminant()
    {
        float[,] matrix = 
        {
            {1, 1, 0},
            {0, 0, 2},
            {1, -1, 0}
        };
        float determinant = 4;

        Assert.AreEqual(determinant, MatrixTools.MatrixDeterminant(matrix));
    }

    [Test]
    public void TestValid3x3MatrixInverse()
    {
        float[,] matrix = 
        {
            {1, 2, 4},
            {2, -1, 3},
            {4, 0, 1}
        };
        float[,] matrixInverse = 
        {
            {-1/35f, -2/35f, 10/35f},
            {10/35f, -15/35f, 5/35f},
            {4/35f, 8/35f, -5/35f}
        };

        float[,] result = MatrixTools.Matrix3x3Inverse(matrix);

        for (int i = 0; i < matrix.GetLength(0); i++)
            for (int j = 0; j < matrix.GetLength(1); j++)
                Assert.IsTrue(Mathf.Abs(matrixInverse[i,j] - result[i,j]) < MARGIN_OF_ERROR);
    }
}
