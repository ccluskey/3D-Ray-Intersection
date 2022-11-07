using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixTools
{
    // If there is no inverse, the original matrix is returned
    public static float[,] Matrix2x2Inverse(float[,] matrix) {
        if (matrix.GetLength(0) != 2 || matrix.GetLength(1) != 2)
        {
            return matrix;
        }
        float determinant = Matrix2x2Determinant(matrix);
        if (0 == determinant) {
            return matrix;
        }

        // Doing this manually sadge
        float[,] result = new float[2,2];
        result[0,0] = matrix[1,1] / determinant;
        result[0,1] = (-1) * matrix[0,1] / determinant;
        result[1,0] = (-1) * matrix[1,0] / determinant;
        result[1,1] = matrix[0,0] / determinant;
        return result;
    }

    public static float[,] Matrix3x3Inverse(float[,] matrix) {
        if (matrix.GetLength(0) != 3 || matrix.GetLength(1) != 3)
        {
            return matrix;
        }
        float determinant = MatrixDeterminant(matrix);
        if (0 == determinant) {
            return matrix;
        }

        float[,] result = new float[3,3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // Find the cofactor matrix
                float[,] subMatrix = new float[2,2];
                for (int si = 0; si < 2; si++)
                {
                    int addOneI = si >= i ? 1 : 0; 
                    for (int sj = 0; sj < 2; sj++)
                    {
                        int addOneJ = sj >= j ? 1 : 0;
                        // i and j are intentionally swapped because we are taking the transform of the original matrix
                        subMatrix[si, sj] = matrix[sj + addOneJ,si + addOneI];
                    }
                }

                result[i,j] = Matrix2x2Determinant(subMatrix) / determinant;
                if ((i + j) % 2 == 1)
                    result[i,j] *= -1;
            }
        }
        return result;
    }

    public static float Matrix2x2Determinant(float[,] matrix)
    {
        return matrix[0, 0] * matrix[1, 1] - matrix[1, 0] * matrix[0, 1];
    }

    // Matrix must be square
    public static float MatrixDeterminant(float[,] matrix)
    {
        if (matrix.GetLength(0) == 2)
        {
            return Matrix2x2Determinant(matrix);
        }
        else
        {
            float det = 0;
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                float[,] subMatrix = new float[matrix.GetLength(0) - 1, matrix.GetLength(0) - 1];
                for (int j = 0; j < matrix.GetLength(0) - 1; j++)
                {
                    for (int k = 0; k < matrix.GetLength(0) - 1; k++)
                    {
                        // To construct the submatrix, skip the ith column
                        int addOne = k >= i ? 1 : 0;
                        subMatrix[j,k] = matrix[j + 1,k+addOne];
                    }
                }
                det += Mathf.Pow(-1, i) * matrix[0,i] * MatrixDeterminant(subMatrix);
            }
            return det;
        }
    }
}
