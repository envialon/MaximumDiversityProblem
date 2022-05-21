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
                for (int j = i + 1; j < problem.numberOfVectors; j++)
                {
                    HashSet<int> partialSolution = new HashSet<int> { i, j };
                    PartialSolution pSol = new PartialSolution(problem, partialSolution, solutionSize);
                    if (pSol.upperBound > lowerBound)
                    {
                        activeNodes.Add(pSol);
                    }
                }
            }
        }

        private static void GenerateChildren(HashSet<PartialSolution> activeNodes, PartialSolution partialSolution, float lowerBound, int solutionSize)
        {
            for (int candidate = 0; candidate < problem.vectors.Count; candidate++)
            {
                if (!partialSolution.solution.Contains(candidate))
                {
                    HashSet<int> newSolution = new HashSet<int>(partialSolution.solution);
                    newSolution.Add(candidate);
                    PartialSolution newPSol = new PartialSolution(problem, newSolution, solutionSize);

                    if (newPSol.upperBound > lowerBound)
                    {
                        activeNodes.Add(newPSol);
                    }
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

            Solution initialSolution = Greedy.Solve(problem, solutionSize, 1);

            float lowerBound = initialSolution.totalDistance;
            PartialSolution bestSolution = new PartialSolution(problem, initialSolution.solution, solutionSize);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            InitializeActiveNodes(activeNodes, lowerBound);

            while (activeNodes.Count > 0)
            {
                PartialSolution currentSolution = SelectPartialSolution(activeNodes.ToList(), selectionType);
                activeNodes.Remove(currentSolution);

                if (currentSolution.upperBound > lowerBound)
                {
                    for (int candidate = 0; candidate < problem.vectors.Count; candidate++) {
                        if (!currentSolution.solution.Contains(candidate))
                        {
                            HashSet<int> added = new HashSet<int>(currentSolution.solution);
                            added.Add(candidate);
                            PartialSolution newSolution = new PartialSolution(problem, added, solutionSize);
                            if (newSolution.upperBound > lowerBound)
                            {
                                if (newSolution.depth == solutionSize)
                                {
                                    lowerBound = newSolution.upperBound;
                                    bestSolution = newSolution;
                                }
                                else {
                                    activeNodes.Add(newSolution);
                                }
                            }

                        }
                    }
                    Prune(activeNodes, lowerBound);
                }
                
            }
            sw.Stop();
            Solution toReturn = new Solution(problem, bestSolution);
            toReturn.elapsedMilliseconds = sw.ElapsedMilliseconds;
            return toReturn;
        }

    }
}
