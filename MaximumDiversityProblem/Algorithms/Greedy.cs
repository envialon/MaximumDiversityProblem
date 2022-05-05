namespace MaximumDiversityProblem
{
    public class Greedy
    {             

        static private List<List<double>> MakeRCL(List<double> centroid, List<List<double>> availableNodes, int rclSize)
        {
            List<List<double>> rcl = new List<List<double>>();


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
                List<double> centroid = Utils.GetCentroid(availableVectors, solution.solution);
                List<List<double>> rcl = MakeRCL(centroid, availableVectors, rclSize);
                int index = rand.Next(0, rcl.Count);
                solution.solution.Add(index);
                availableVectors.RemoveAt(index);
            }

            return solution;
        }
    }
}
