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

        public static Solution SolveBranchAndBound(Problem problem, int problemSize,  BBType searchType = BBType.DEPTH_FIRST_SEARCH)
        {
            return BranchAndBound.Solve(problem,problemSize,  searchType);
        }

    }
}
