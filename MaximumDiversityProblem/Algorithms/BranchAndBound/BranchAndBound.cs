namespace MaximumDiversityProblem
{
    public enum BBType
    {
        SMALLEST_UPPER_BOUND,
        DEPTH_FIRST_SEARCH,
    }

    internal class BranchAndBound
    {

        private static List<List<float>> distanceMatrix = new List<List<float>>();
        private static int numberOfVectors = 0;


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

        private static List<int> GetCandidates(List<int> partialSolution)
        {
            List<int> candidates = Enumerable.Range(0, numberOfVectors).ToList();

            for (int k = 0; k < partialSolution.Count; k++)
            {
                candidates.Remove(partialSolution[k]);
            }
            return candidates;
        }

        private static float CalculateUpperBound(List<int> partialSolution)
        {
            List<int> candidates = GetCandidates(partialSolution);

            float higherDistance = float.MinValue;
            for (int c = 0; c < candidates.Count; c++)
            {
                int candidate = candidates[c];
                for (int i = 0; i < numberOfVectors; i++)
                {
                    if (!partialSolution.Contains(i))
                    {
                        float currentDistance = distanceMatrix[i][candidate];
                        if (higherDistance < currentDistance)
                        {
                            higherDistance = currentDistance;
                        }
                    }
                }
            }
            return higherDistance * (numberOfVectors - partialSolution.Count);
        }

        private static void InitializeActiveNodes(HashSet<PartialSolution> activeNodes)
        {
            for (int i = 0; i < numberOfVectors; i++)
            {
                for (int j = i + 1; j < numberOfVectors; j++)
                {
                    List<int> partialSolution = new List<int> { i, j };

                    float upperBound = CalculateUpperBound(partialSolution);
                    activeNodes.Add(new PartialSolution(partialSolution, upperBound));
                }
            }
        }

        private static List<PartialSolution> GenerateChildren(PartialSolution partialSolution, int solutionSize)
        {
            List<int> candidates = GetCandidates(partialSolution.solution);
            List<List<int>> childrenSolutions = new List<List<int>>();
            List<PartialSolution> children = new List<PartialSolution>();



            return children;
        }

        public static Solution Solve(Problem problem, BBType selectionType, int solutionSize, double LowerBound)
        {
            distanceMatrix = problem.distanceMatrix;
            numberOfVectors = problem.vectors.Count;

            HashSet<PartialSolution> activeNodes = new HashSet<PartialSolution>();
            InitializeActiveNodes(activeNodes);

            float lowerBound = Greedy.Solve(problem, solutionSize).totalDistance;
            PartialSolution bestSolution;

            while (activeNodes.Count > 0)
            {
                PartialSolution currentSolution = SelectPartialSolution(activeNodes.ToList(), selectionType);

                if (currentSolution.upperBound <= LowerBound)
                {
                    activeNodes.Remove(currentSolution);
                }

                List<PartialSolution> children = GenerateChildren(currentSolution, problem.solutionSize);

            }


            throw new NotImplementedException();
        }

    }
}
