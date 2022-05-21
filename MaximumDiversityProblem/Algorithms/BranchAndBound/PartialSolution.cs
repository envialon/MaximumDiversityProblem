namespace MaximumDiversityProblem
{
    public class PartialSolution
    {
        public HashSet<int> solution = new HashSet<int>();
        public int depth = -1;
        public float upperBound = -1;
        public int maxSolutionSize;
        public int id;


        public PartialSolution(Problem problem, HashSet<int> solution, int id, int maxSolutionSize)
        {
            this.id = id;
            this.solution = new HashSet<int>(solution);
            this.depth = solution.Count;
            this.maxSolutionSize = maxSolutionSize;
            this.upperBound = CalculateUpperBound(problem);
        }


        private float CalculateUpperBound(Problem problem)
        {
            List<int> indexList = solution.ToList();

            float highestDistance = float.MinValue;

            for (int candidate = 0; candidate < problem.vectors.Count; candidate++)
            {
                if (!solution.Contains(candidate))
                {
                    for (int target = 0; target < problem.vectors.Count; target++)
                    {
                        if (candidate != target)
                        {
                            float currentDistance = problem.distanceMatrix[candidate][target];
                            if (highestDistance < currentDistance)
                            {
                                highestDistance = currentDistance;
                            }
                        }
                    }
                }
            }
            int rest = maxSolutionSize - depth;
            int coef = maxSolutionSize * rest + rest * rest - (rest * (rest + 1) / 2);
            return Utils.GetSolutionDistance(indexList, problem) + highestDistance * coef;
        }

        public override string ToString()
        {
            List<int> list = solution.ToList();
            return String.Join(", ", list);
        }

    }
}
