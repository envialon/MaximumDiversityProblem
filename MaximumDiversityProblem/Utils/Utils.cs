namespace MaximumDiversityProblem
{
    public class Utils
    {
        static public List<double> GetCentroid(List<List<double>> vectors, HashSet<int> solutionSet)
        {
            List<int> solutionIndexes = solutionSet.ToList();
            List<double> result = vectors[solutionIndexes[0]];

            result = vectors[solutionIndexes[0]];

            for (int i = 1; i < solutionIndexes.Count; i++)
            {
                List<double> currentVector = vectors[solutionIndexes[i]];
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

        static public double GetSolutionDistance(Solution solution)
        {
            double totalDistance = 0;
            List<int> indexList = solution.solution.ToList();
            List<List<double>> vectors = solution.vectors;
            for (int origin = 0; origin < indexList.Count; origin++)
            {
                for (int destination = 0; destination < indexList.Count; destination++)
                {
                    if (origin != destination)
                    {
                        totalDistance += GetDistance(vectors[indexList[origin]], vectors[indexList[destination]]);
                    }
                }
            }

            return totalDistance;
        }

        static public double GetDistance(List<double> origin, List<double> target)
        {
            double result = 0;

            for (int i = 0; i < origin.Count; i++)
            {
                result += (origin[i] - target[i]) * (origin[i] - target[i]);
            }

            return Math.Sqrt(result);
        }
    }
}