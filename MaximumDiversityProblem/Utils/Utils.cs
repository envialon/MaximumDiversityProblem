namespace MaximumDiversityProblem
{
    /// <summary>
    /// Utils is a class with static methods that provide common functionalities used 
    /// to solve the Maximum Diversity Problem.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// This function calculates the centroid given a set of vectors
        /// </summary>
        static public List<float> GetCentroid(List<List<float>> vectors, HashSet<int> solutionSet)
        {
            List<int> solutionIndexes = solutionSet.ToList();
            List<float> result = vectors[solutionIndexes[0]];

            result = new List<float>(vectors[solutionIndexes[0]]);

            for (int i = 1; i < solutionIndexes.Count; i++)
            {
                List<float> currentVector = vectors[solutionIndexes[i]];
                for (int j = 0; j < currentVector.Count; j++)
                {
                    result[j] += currentVector[j];
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i] /= solutionIndexes.Count;
            }

            return result;
        }

        /// <summary>
        /// This funciton calculates the distance of a given solution
        /// </summary>
        static public float GetSolutionDistance(Solution solution)
        {
            float totalDistance = 0;
            List<int> indexList = solution.solution.ToList();
            List<List<float>> vectors = solution.vectors;

            for (int origin = 0; origin < indexList.Count; origin++)
            {
                for (int destination = origin + 1; destination < indexList.Count; destination++)
                {
                    totalDistance += GetDistance(vectors[indexList[origin]], vectors[indexList[destination]]);
                }
            }

            return (float)Math.Round(totalDistance, 2);
        }


        /// <summary>
        /// This funciton calculates the distance of a given indexList and the problem with the vector information
        /// </summary>
        static public float GetSolutionDistance(List<int> indexList, Problem problem) {
            float totalDistance = 0;
            List<List<float>> vectors = problem.vectors;

            for (int origin = 0; origin < indexList.Count; origin++)
            {
                for (int destination = origin + 1; destination < indexList.Count; destination++)
                {
                    totalDistance += GetDistance(vectors[indexList[origin]], vectors[indexList[destination]]);
                }
            }

            return (float)Math.Round(totalDistance, 2);
        }

        /// <summary>
        /// This funciton calculates the distance between two vectors
        /// </summary>
        static public float GetDistance(List<float> origin, List<float> target)
        {
            float result = 0;

            for (int i = 0; i < origin.Count; i++)
            {
                result += (origin[i] - target[i]) * (origin[i] - target[i]);
            }

            return (float)Math.Round(Math.Sqrt(result), 2);
        }

        /// <summary>
        /// This function calculates the sum of the distance between one vector and all of the vectors of a set
        /// </summary>
        static public float GetDistanceToSet(List<List<float>> distanceMatrix, List<int> solutionList, int target)
        {
            float result = 0;
            for (int i = 0; i < solutionList.Count; i++)
            {
                result += distanceMatrix[target][solutionList[i]];
            }

            return (float)Math.Round(result, 2);
        }
    }
}