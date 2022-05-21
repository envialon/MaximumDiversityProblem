using System.Diagnostics;
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
                    if (activeSolutions[i].depth > bestSolution.depth)
                    {
                        bestSolution = activeSolutions[i];
                    }
                }
            }
            return bestSolution;
        }


        private static void InitializeActiveNodes(HashSet<PartialSolution> activeNodes, float lowerBound)
        {
            for (int i = 0; i < problem.numberOfVectors; i++)
            {

                HashSet<int> partialSolution = new HashSet<int> { i};
                PartialSolution pSol = new PartialSolution(problem, partialSolution, i, solutionSize);
                if (pSol.upperBound > lowerBound)
                {
                    activeNodes.Add(pSol);
                }
            }
        }
          

        private static void Prune(HashSet<PartialSolution> activeNodes, float lowerBound)
        {

            foreach (PartialSolution pSol in activeNodes)
            {
                if (pSol.upperBound < lowerBound)
                {
                    activeNodes.Remove(pSol);
                }
            }
        }
        

        public static Solution Solve(Problem problem, int solutionSize, BBType selectionType)
        {
            BranchAndBound.problem = problem;
            BranchAndBound.solutionSize = solutionSize;
            HashSet<PartialSolution> activeNodes = new HashSet<PartialSolution>();
            int generatedNodesCounter = 0;

            Solution initialSolution = Greedy.Solve(problem, solutionSize, 1);
            float lowerBound = initialSolution.totalDistance;
            PartialSolution bestSolution = new PartialSolution(problem, initialSolution.solution, -1, solutionSize);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            InitializeActiveNodes(activeNodes, lowerBound);
            generatedNodesCounter += activeNodes.Count;
            while (activeNodes.Count > 0)
            {
                PartialSolution currentSolution = SelectPartialSolution(activeNodes.ToList(), selectionType);
                activeNodes.Remove(currentSolution);

                if (currentSolution.upperBound > lowerBound)
                {
                    int numberOfCandidates = problem.numberOfVectors - (solutionSize - currentSolution.solution.Count) - currentSolution.id;
                    for (int candidate = currentSolution.id + 1; candidate < numberOfCandidates + 1 + currentSolution.id; candidate++)
                    {
                        if (!currentSolution.solution.Contains(candidate))
                        {
                            HashSet<int> added = new HashSet<int>(currentSolution.solution);
                            added.Add(candidate);
                            PartialSolution newSolution = new PartialSolution(problem, added, candidate, solutionSize);
                            if (newSolution.upperBound > lowerBound)
                            {
                                if (newSolution.depth == solutionSize)
                                {
                                    lowerBound = newSolution.upperBound;
                                    bestSolution = newSolution;
                                }
                                else
                                {
                                    activeNodes.Add(newSolution);
                                }
                                generatedNodesCounter++;
                            }

                        }
                    }
                    Prune(activeNodes, lowerBound);
                }

            }
            sw.Stop();
            Solution toReturn = new Solution(problem, bestSolution);
            toReturn.elapsedMilliseconds = sw.ElapsedMilliseconds;
            toReturn.generatedNodes = generatedNodesCounter;
            return toReturn;
        }
    }
}
