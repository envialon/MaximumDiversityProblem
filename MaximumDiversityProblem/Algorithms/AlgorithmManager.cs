namespace MaximumDiversityProblem
{
    public class AlgorithmManager
    {
        public static Solution SolveGreedy(Problem problem, int rclSize)
        {
            return Greedy.Solve(problem, rclSize);
        }
    }
}
