using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sem2_lab4
{
    public static class RandomArray
    {
        public static double?[,] CreateRandonArray(int countRows = 1, int countColumns = 2, int minVal = -100, int maxVal = 100)
        {
            double?[,] array = new double?[countRows, countColumns];

            Random random = new Random(Guid.NewGuid().GetHashCode());

            for (int indexRow = 0; indexRow < countRows; ++indexRow)
            {
                for (int indexColumn = 0; indexColumn < countColumns; ++indexColumn)
                {
                    array[indexRow, indexColumn] = random.Next(minVal, maxVal) + random.NextDouble();

                    if (array[indexRow, indexColumn] > maxVal)
                    {
                        array[indexRow, indexColumn] = maxVal;
                    }
                }
            }

            return array;
        }
    }
}