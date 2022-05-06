namespace MaximumDiversityProblem
{
    public class Utils
    {
        static public List<double> GetCentroid(List<List<double>> vectors, HashSet<int> solutionSet)
        {
            List<int> solutionIndexes = solutionSet.ToList();
            List<double> result = vectors[solutionIndexes[0]];

            result = vectors[solutionIndexes[0]];

            for (int i = 0; i < solutionIndexes.Count; i++)
            {
                for (int j = 0; j < vectors.Count; j++)
                {
                    result[j] += vectors[solutionIndexes[i]][j];
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i] /= solutionIndexes.Count;
            }

            return result;
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