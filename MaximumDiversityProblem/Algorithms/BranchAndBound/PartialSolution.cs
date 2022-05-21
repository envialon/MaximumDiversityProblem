namespace MaximumDiversityProblem
{
    public class PartialSolution
    {
        public HashSet<int> solution = new HashSet<int>();
        public HashSet<int> candidates = new HashSet<int>();
        public int depth = -1;
        public float upperBound = -1;
        public int maxSolutionSize;



        public PartialSolution(Problem problem, HashSet<int> solution, int maxSolutionSize)
        {
            this.solution = new HashSet<int>(solution);
            this.depth = solution.Count;
            this.candidates = new HashSet<int>(GetCandidates(problem, solution.ToList()));
            this.maxSolutionSize = maxSolutionSize;
            this.upperBound = CalculateUpperBound(problem);
        }

        private List<int> GetCandidates(Problem problem, List<int> indexList)
        {
            List<int> candidates = Enumerable.Range(0, problem.numberOfVectors).ToList();

            for (int k = 0; k < indexList.Count; k++)
            {
                candidates.Remove(indexList[k]);
            }
            return candidates;
        }

        private float CalculateUpperBound(Problem problem)
        {
            List<int> indexList = solution.ToList();
            List<int> candidates = this.candidates.ToList();

            float highestDistance = float.MinValue;

            for (int c = 0; c < candidates.Count; c++)
            {
                int candidate = candidates[c];
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
