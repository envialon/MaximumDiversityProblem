namespace MaximumDiversityProblem
{
    public enum BBType
    {
        SMALLEST_UPPER_BOUND,
        DEPTH_FIRST_SEARCH,
    }

    internal class BranchAndBound
    {
        private static Problem problem;
        private static int solutionSize;

        private static PartialSolution SelectPartialSolution(List<PartialSolution> activeSolutions, BBType selectionType)
        {
            PartialSolution bestSolution = activeSolutions.First();
            if (selectionType == BBType.SMALLEST_UPPER_BOUND)
            {
                for (int i = 1; i < activeSolutions.Count; i++)
                {
                    if (activeSolutions[i].upperBound < bestSolution.upperBound)
                    {
                        bestSolution = activeSolutions[i];
                    }
                }
            }
            else if (selectionType == BBType.DEPTH_FIRST_SEARCH)
            {
                for (int i = 1; i < activeSolutions.Count; i++)
                {
                    if (activeSolutions[i].depth < bestSolution.depth)
                    {
                        bestSolution = activeSolutions[i];
                    }
                }
            }
            return bestSolution;
        }


        private static void InitializeActiveNodes(HashSet<PartialSolution> activeNodes)
        {
            for (int i = 0; i < problem.numberOfVectors; i++)
            {
                for (int j = i + 1; j < problem.numberOfVectors; j++)
                {
                    HashSet<int> partialSolution = new HashSet<int> { i, j };
                    PartialSolution pSol = new PartialSolution(problem, partialSolution, solutionSize);
                    activeNodes.Add(pSol);
                }
            }
        }

        private static void GenerateChildren(HashSet<PartialSolution> activeNodes, PartialSolution partialSolution, float lowerBound, int solutionSize)
        {
            List<int> candidates = partialSolution.candidates.ToList();

            for (int c = 0; c < candidates.Count; c++)
            {
                int candidate = candidates[c];
                HashSet<int> newSolution = new HashSet<int>(partialSolution.solution);
                newSolution.Add(candidate);
                PartialSolution newPSol = new PartialSolution(problem, newSolution, solutionSize);

                if (newPSol.upperBound >= lowerBound)
                {
                    activeNodes.Add(newPSol);
                }
            }
        }

        public static Solution Solve(Problem problem, int solutionSize, BBType selectionType)
        {
            BranchAndBound.problem = problem;
            BranchAndBound.solutionSize = solutionSize;
            HashSet<PartialSolution> activeNodes = new HashSet<PartialSolution>();
            InitializeActiveNodes(activeNodes);

            Solution initialSolution = Greedy.Solve(problem, solutionSize, 1);

            float lowerBound = initialSolution.totalDistance;         
            PartialSolution bestSolution = new PartialSolution(problem, initialSolution.solution, solutionSize);

            while (activeNodes.Count > 0)
            {
                PartialSolution currentSolution = SelectPartialSolution(activeNodes.ToList(), selectionType);

                activeNodes.Remove(currentSolution);

                if (currentSolution.upperBound >= lowerBound)
                {
                    if (currentSolution.depth == solutionSize)
                    {
                        lowerBound = currentSolution.upperBound;
                        bestSolution = currentSolution;
                        continue;
                    }
                    GenerateChildren(activeNodes, currentSolution, lowerBound, solutionSize);
                }
            }
            return new Solution(problem, bestSolution, bestSolution.candidates);

        }

    }
}
