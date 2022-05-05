namespace MaximumDiversityProblem
{
    public class Greedy
    {

        static private List<double> GetCentroid(List<List<double>> availableVectors, HashSet<int> solutionSet)
        {
            List<int> solutionIndexes = solutionSet.ToList();
            List<double> result = availableVectors[solutionIndexes[0]];

            result = availableVectors[solutionIndexes[0]];

            for (int i = 0; i < solutionIndexes.Count; i++)
            {
                for (int j = 0; j < availableVectors.Count; j++)
                {
                    result[j] += availableVectors[solutionIndexes[i]][j];
                }
            }

            for (int i = 0; i < result.Count; i++)
            {
                result[i] /= solutionIndexes.Count;
            }

            return result;
        }

        static private double GetDistance(List<double> centroid, List<double> target)
        {
            double result = 0;

            for (int i = 0; i < centroid.Count; i++)
            {
                result += (centroid[i] - target[i]) * (centroid[i] - target[i]);
            }

            return Math.Sqrt(result);
        }

        static private List<List<double>> MakeRCL(List<double> centroid, List<List<double>> availableNodes, int rclSize)
        {
            List<List<double>> rcl = new List<List<double>>();
            List<(int, double)> indexDistanceList = new List<(int, double)>();

            for (int i = 0; i < availableNodes.Count; i++)
            {
                double distance = GetDistance(centroid, availableNodes[i]);
                
            }

            return rcl;
        }

        static public Solution Solve(Problem problem, int rclSize)
        {
            Random rand = new Random();
            Solution solution = new Solution(problem);
            List<List<double>> availableVectors = new List<List<double>>();
            int numberOfVectors = problem.numberOfVectors;

            for (int i = 0; i < numberOfVectors; i++)
            {
                List<double> centroid = GetCentroid(availableVectors, solution.solution);
                List<List<double>> rcl = MakeRCL(centroid, availableVectors, rclSize);
                int index = rand.Next(0, rcl.Count);
                solution.solution.Add(index);
                availableVectors.RemoveAt(index);
            }

            return solution;
        }
    }
}
