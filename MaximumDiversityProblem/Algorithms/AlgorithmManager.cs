namespace MaximumDiversityProblem
{
    public class AlgorithmManager
    {
        public static Solution SolveGreedy(Problem problem, int problemSize, int rclSize = 1)
        {
            return Greedy.Solve(problem, problemSize, rclSize);
        }

        public static Solution SolveGrasp(Problem problem, int problemSize, int rclSize = 1)
        {
            return Grasp.Solve(problem, problemSize, rclSize);
        }

        public static Solution SolveBranchAndBound(Problem problem, Solution solution)
        {
            return BranchAndBound.Solve(problem, solution.solution.Count, solution.totalDistance);
        }

    }
}
