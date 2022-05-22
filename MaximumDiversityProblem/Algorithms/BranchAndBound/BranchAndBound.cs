using System.Diagnostics;
namespace MaximumDiversityProblem
{
    /// <summary>
    /// Enum with the selection methods for the branch and bound algorithm.s
    /// </summary>
    public enum BBType
    {
        SMALLEST_UPPER_BOUND,
        DEPTH_FIRST_SEARCH,
    }

    /// <summary>
    /// Class with static methods to solve the given problem using the Branch and bound algorithm
    /// </summary>
    internal class BranchAndBound
    {
        private static Problem problem;
        private static int solutionSize;

        /// <summary>
        /// Helper funciton that choses the next node to evaluate 
        /// depending on the selection type
        /// </summary>
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

        /// <summary>
        /// Helper function to initialize the nodes for the branch and bound algorithm
        /// </summary>
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
          
        /// <summary>
        /// Helper function that prunes the activeNodes given a lower bound
        /// </summary>
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

        /// <summary>
        /// Solves the given problem with the given solution size 
        /// using the branch and bound algorithm, the selection type is DFS by default
        /// </summary>
        public static Solution Solve(Problem problem, int solutionSize, BBType selectionType = BBType.DEPTH_FIRST_SEARCH)
        {
            BranchAndBound.problem = problem;
            BranchAndBound.solutionSize = solutionSize;
            HashSet<PartialSolution> activeNodes = new HashSet<PartialSolution>();
            int generatedNodesCounter = 0;

            Solution initialSolution = Grasp.Solve(problem, solutionSize, 1);
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
