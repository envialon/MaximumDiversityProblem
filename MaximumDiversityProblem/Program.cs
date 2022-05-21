
namespace MaximumDiversityProblem
{
    class Program
    {

        private static void PrintSolutionInfo(List<Solution> solutions, int numberOfProblems)
        {
            Console.WriteLine(String.Format("\tfilename\tn\tdim\ts_size\tcost\tmilliseconds"));
            int count = 1;
            foreach (Solution solution in solutions)
            {
                Console.WriteLine(solution.id + "\t" + solution.vectors.Count + "\t" + solution.dimensionality + "\t"
                    + solution.solution.Count + "\t" + solution.totalDistance.ToString("0.00") + "\t" + solution.elapsedMilliseconds);
                if (count == numberOfProblems)
                {
                    count = 0;
                    Console.WriteLine();
                }

                count++;
            }
        }


        private static void PrintSolutionVectors(Solution solution)
        {
            Console.WriteLine("{ ");
            foreach (int index in solution.solution.ToList())
            {
                Console.Write("( ");
                foreach (float coord in solution.vectors[index])
                {
                    Console.Write(coord.ToString("0.00") + " ");
                }
                Console.WriteLine(")");
            }
            Console.WriteLine(" }");
        }

        private static void PrintSolutionInfo(List<Solution> solutions)
        {
            float solutionTest = Utils.GetSolutionDistance(solutions[0]);
            Console.WriteLine(String.Format("\tfilename\tn\tdim\ts_size\tcost\tmilliseconds"));
            foreach (Solution solution in solutions)
            {
                Console.WriteLine(solution.id + "\t" + solution.vectors.Count + "\t" + solution.dimensionality + "\t"
                    + solution.solution.Count + "\t" + solution.totalDistance.ToString("0.00") + "\t" + solution.elapsedMilliseconds);

                float solutionDist = Utils.GetSolutionDistance(solution);

                if (solution.totalDistance.ToString("0.00") != Utils.GetSolutionDistance(solution).ToString("0.00"))
                {
                    Console.WriteLine("ERROR: correct distance is " + Utils.GetSolutionDistance(solution));
                }
            }
        }

        public static void Main(string[] args)
        {
            string path;
            if (args.Length > 1)
            {
                path = args[0];
            }
            else
            {
                path = "input_files\\";
            }

            int SOLUTION_SIZE = 2;
            int RCL_SIZE = 2;

            List<Problem> problems = new List<Problem>();
            List<Solution> greedySolutions = new List<Solution>();
            List<Solution> graspSolutions = new List<Solution>();
            List<Solution> branchAndBoundDFSSolutions = new List<Solution>();
            List<Solution> branchAndBoundSmallestUpperBoundSolution = new List<Solution>();


            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                problems.Add(new Problem(filename));
            }

            for (int i = SOLUTION_SIZE; i <= 5; i++)
            {
                SOLUTION_SIZE = i;
                foreach (Problem problem in problems)
                {
                    greedySolutions.Add(AlgorithmManager.SolveGreedy(problem, SOLUTION_SIZE));
                    graspSolutions.Add(AlgorithmManager.SolveGrasp(problem, SOLUTION_SIZE, RCL_SIZE));
                    branchAndBoundDFSSolutions.Add(AlgorithmManager.SolveBranchAndBound(problem, SOLUTION_SIZE, BBType.DEPTH_FIRST_SEARCH));  ;
                    branchAndBoundSmallestUpperBoundSolution.Add(AlgorithmManager.SolveBranchAndBound(problem, SOLUTION_SIZE, BBType.SMALLEST_UPPER_BOUND));
                }
            }

            Console.WriteLine("GREEDY SOLUTIONS:");
            PrintSolutionInfo(greedySolutions, problems.Count);
            
            Console.WriteLine();
            Console.WriteLine("GRASP SOLUTIONS:");
            PrintSolutionInfo(graspSolutions, problems.Count);
            
            Console.WriteLine();
            Console.WriteLine("BRANCH AND BOUND DFS SOLUTIONS:");
            PrintSolutionInfo(branchAndBoundDFSSolutions, problems.Count);

            Console.WriteLine();
            Console.WriteLine("BRANCH AND BOUND SMALLEST UPPERBOUND FIRST SOLUTIONS:");
            PrintSolutionInfo(branchAndBoundSmallestUpperBoundSolution, problems.Count);
            

        }
    }
}