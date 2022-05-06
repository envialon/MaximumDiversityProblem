namespace MaximumDiversityProblem
{
    public class AlgorithmManager
    {
        public static Solution SolveGreedy(Problem problem, int rclSize = 1)
        {
            return Greedy.Solve(problem, rclSize);
        }
    }
}
