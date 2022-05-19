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

        private static List<int> GetCandidates(List<int> indexList)
        {
            List<int> candidates = Enumerable.Range(0, problem.numberOfVectors).ToList();

            for (int k = 0; k < indexList.Count; k++)
            {
                candidates.Remove(indexList[k]);
            }
            return candidates;
        }

        private static float CalculateUpperBound(HashSet<int> partialSolution)
        {
            List<int> indexList = partialSolution.ToList();
            List<int> candidates = GetCandidates(indexList);

            float higherDistance = float.MinValue;
            int bestCandidate = -1;
            if (indexList.Count != solutionSize)
            {
                for (int c = 0; c < candidates.Count; c++)
                {
                    int candidate = candidates[c];
                    for (int i = 0; i < problem.numberOfVectors; i++)
                    {
                        if (!indexList.Contains(i))
                        {
                            float currentDistance = problem.distanceMatrix[i][candidate];
                            if (higherDistance < currentDistance)
                            {
                                higherDistance = currentDistance;
                                bestCandidate = candidate;
                            }
                        }
                    }
                }
            }

            float toBeSelected = higherDistance * (solutionSize - indexList.Count);
            float connectionsToSolution = Utils.GetDistanceToSet(problem.distanceMatrix, new HashSet<int>(indexList), bestCandidate);
            float solutionDistance = Utils.GetSolutionDistance(indexList, problem);

            return toBeSelected + connectionsToSolution + solutionDistance;

        }

        private static void InitializeActiveNodes(HashSet<PartialSolution> activeNodes)
        {
            for (int i = 0; i < problem.numberOfVectors; i++)
            {
                for (int j = i + 1; j < problem.numberOfVectors; j++)
                {
                    HashSet<int> partialSolution = new HashSet<int>{ i, j };

                    float upperBound = CalculateUpperBound(partialSolution);
                    activeNodes.Add(new PartialSolution(partialSolution, upperBound));
                }
            }
        }

        private static List<PartialSolution> GenerateChildren(PartialSolution partialSolution, int solutionSize)
        {
            List<int> candidates = GetCandidates(partialSolution.solution.ToList());
            List<List<int>> childrenSolutions = new List<List<int>>();
            List<PartialSolution> children = new List<PartialSolution>();



            return children;
        }

        public static Solution Solve(Problem problem, int solutionSize, BBType selectionType)
        {
            BranchAndBound.problem = problem;
            BranchAndBound.solutionSize = solutionSize;
            HashSet<PartialSolution> activeNodes = new HashSet<PartialSolution>();
            InitializeActiveNodes(activeNodes);

            Solution greedy = Greedy.Solve(problem, solutionSize, 1);

            float lowerBound = greedy.totalDistance;
            PartialSolution bestSolution = new PartialSolution(greedy.solution, lowerBound);

            while (activeNodes.Count > 0)
            {
                PartialSolution currentSolution = SelectPartialSolution(activeNodes.ToList(), selectionType);

                if(currentSolution.depth == solutionSize && currentSolution.upperBound > lowerBound)
                {
                    lowerBound = currentSolution.upperBound;
                    activeNodes.Remove(currentSolution);
                    bestSolution = currentSolution;
                    continue;
                }


                activeNodes.Remove(currentSolution);
                if (currentSolution.upperBound <= lowerBound)
                {
                    continue;
                }


                List<PartialSolution> children = GenerateChildren(currentSolution, solutionSize);

                for(int i = 0; i < children.Count; i++)
                {
                    activeNodes.Add(children[i]);
                }                
            }

            return new Solution(problem, bestSolution, GetCandidates(bestSolution.solution.ToList()));
            
        }

    }
}
