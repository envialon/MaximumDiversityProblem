namespace MaximumDiversityProblem
{
    public class Greedy
    {             

        static private List<List<double>> MakeRCL(List<List<double>> distanceMatrix, List<List<double>> availableNodes, List<double> centroid, int rclSize)
        {
            List<List<double>> rcl = new List<List<double>>();

            for (int i = 0; i < availableNodes.Count; i++)
            {
                double distance = 0;
               
            }

            return rcl;
        }

        static public Solution Solve(Problem problem, int rclSize)
        {
            Random rand = new Random();
            Solution solution = new Solution(problem);
            List<List<double>> availableVectors = new List<List<double>>();
            List<List<double>> distanceMatrix = problem.distanceMatrix;
            int numberOfVectors = problem.numberOfVectors;

            for (int i = 0; i < numberOfVectors; i++)
            {
                List<double> centroid = Utils.GetCentroid(availableVectors, solution.solution);
                List<List<double>> rcl = MakeRCL(distanceMatrix, availableVectors, centroid, rclSize);
                if (rcl.Count > 0)
                {
                    break;
                }
                int index = rand.Next(0, rcl.Count);
                solution.solution.Add(index);
                availableVectors.RemoveAt(index);
            }

            return solution;
        }
    }
}
