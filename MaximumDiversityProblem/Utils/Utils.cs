namespace MaximumDiversityProblem
{
    public class Utils
    {

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

            return totalDistance;
        }

        static public float GetDistance(List<float> origin, List<float> target)
        {
            float result = 0;

            for (int i = 0; i < origin.Count; i++)
            {
                result += (origin[i] - target[i]) * (origin[i] - target[i]);
            }

            return (float)Math.Round(Math.Sqrt(result), 2);
        }

        static public float GetDistanceToSet(List<List<float>> distanceMatrix, HashSet<int> s, int target)
        {
            float result = 0;
            List<int> solutionList = s.ToList();
            for (int i = 0; i < solutionList.Count; i++)
            {

                result += distanceMatrix[target][solutionList[i]];

            }

            return result;
        }
        //static public float GetDistanceToSet(List<List<float>> vectors, HashSet<int> s, int target)
        //{
        //    float result = 0;
        //    List<int> solutionList = s.ToList();
        //    for (int i = 0; i < solutionList.Count; i++)
        //    {
        //        if (solutionList[i] != target)
        //        {
        //            result += GetDistance(vectors[target], vectors[solutionList[i]]);
        //        }
        //    }

        //    return result;
        //}

        static public float GetDistanceToSet(List<List<float>> distanceMatrix, List<int> solutionList, int target)
        {
            float result = 0;
            for (int i = 0; i < solutionList.Count; i++)
            {
                if (solutionList[i] != target)
                {
                    result += distanceMatrix[target][solutionList[i]];
                }
            }

            return result;
        }
    }
}