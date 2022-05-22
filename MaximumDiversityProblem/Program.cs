
namespace MaximumDiversityProblem
{
    class Program
    {

        /// <summary>
        /// Helper function to print the Grasp information
        /// </summary>
        private static void PrintGraspInfo(List<Solution> solutions, int numberOfProblems)
        {
            Console.WriteLine(String.Format("\tfilename\tn\tdim\ts_size\trcl\tcost\tmilliseconds"));
            int count = 1;
            foreach (Solution solution in solutions)
            {
                Console.WriteLine(solution.id + "\t" + solution.vectors.Count + "\t" + solution.dimensionality + "\t" + solution.rclSize + "\t"+
                    + solution.solution.Count + "\t" + solution.totalDistance.ToString("0.00") + "\t" + solution.elapsedMilliseconds);
                if (count == numberOfProblems)
                {
                    count = 0;
                    Console.WriteLine();
                }

                count++;
            }
        }

        /// <summary>
        /// Helper function to print the Branch and bound info
        /// </summary>
        private static void PrintBranchAndBoundInfo(List<Solution> solutions, int numberOfProblems)
        {
            Console.WriteLine(String.Format("\tfilename\tn\tdim\ts_size\tcost\tmilliseconds\tgenerated"));
            int count = 1;
            foreach (Solution solution in solutions)
            {
                Console.WriteLine(solution.id + "\t" + solution.vectors.Count + "\t" + solution.dimensionality + "\t" 
                    + solution.solution.Count + "\t" + solution.totalDistance.ToString("0.00") + "\t" + solution.elapsedMilliseconds + "\t\t" + solution.generatedNodes);
                if (count == numberOfProblems)
                {
                    count = 0;
                    Console.WriteLine();
                }

                count++;
            }
        }


        /// <summary>
        /// Helper fucntion to print basic solution Info
        /// </summary>
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
                    if (solution.totalDistance.ToString("0.00") != Utils.GetSolutionDistance(solution).ToString("0.00"))
                    {
                        Console.WriteLine("ERROR: correct distance is " + Utils.GetSolutionDistance(solution));
                    }
                }

                count++;
            }
        }

        /// <summary>
        /// Used to print the solution vectors
        /// </summary>
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
    
        /// <summary>
        /// Main body of the program
        /// </summary>
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
                foreach (Problem problem in problems)
                {
                    greedySolutions.Add(AlgorithmManager.SolveGreedy(problem, i));
                }
            }
            Console.WriteLine("GREEDY SOLUTIONS:");
            PrintSolutionInfo(greedySolutions, problems.Count);


            for (int i = SOLUTION_SIZE; i <= 5; i++)
            {
                foreach (Problem problem in problems)
                {
                    graspSolutions.Add(AlgorithmManager.SolveGrasp(problem, i, RCL_SIZE));
                }
            }
            Console.WriteLine();
            Console.WriteLine("GRASP SOLUTIONS:");
            PrintGraspInfo(graspSolutions, problems.Count);


            for (int i = SOLUTION_SIZE; i <= 5; i++)
            {
                foreach (Problem problem in problems)
                {
                    branchAndBoundDFSSolutions.Add(AlgorithmManager.SolveBranchAndBound(problem, i, BBType.DEPTH_FIRST_SEARCH)); 
                }
            }
            Console.WriteLine();
            Console.WriteLine("BRANCH AND BOUND DFS SOLUTIONS:");
            PrintBranchAndBoundInfo(branchAndBoundDFSSolutions, problems.Count);


            for (int i = SOLUTION_SIZE; i <= 5; i++)
            {
                foreach (Problem problem in problems)
                {
                    branchAndBoundSmallestUpperBoundSolution.Add(AlgorithmManager.SolveBranchAndBound(problem, i, BBType.SMALLEST_UPPER_BOUND));
                }
            }
            Console.WriteLine();
            Console.WriteLine("BRANCH AND BOUND SMALLEST UPPERBOUND FIRST SOLUTIONS:");
            PrintBranchAndBoundInfo(branchAndBoundSmallestUpperBoundSolution, problems.Count);
        }
    }
}