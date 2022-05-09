
namespace MaximumDiversityProblem
{
    class Program
    {

        private static void Test2(Solution solution)
        {
            float cost = solution.totalDistance;
            List<int> slist = solution.solution.ToList();

            float afterRemoval = solution.totalDistance - (float)Utils.GetDistanceToSet(solution.distanceMatrix, solution.solution, 3);
            solution.solution.Remove(3);

            float real = Utils.GetSolutionDistance(solution);


        }



        private static void PrintSolutionVectors(Solution solution)
        {
            Console.WriteLine("{ ");
            foreach (int index in solution.solution.ToList())
            {
                Console.Write("( ");
                foreach ( float coord in solution.vectors[index])
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

            int SOLUTION_SIZE = 5;
            int RCL_SIZE = 2;

            List<Problem> problems = new List<Problem>();
            List<Solution> greedySolutions = new List<Solution>();
            List<Solution> graspSolutions = new List<Solution>();

            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                problems.Add(new Problem(filename));
            }

            
            Solution s = AlgorithmManager.SolveGreedy(problems[0], SOLUTION_SIZE);
            Test2(s);
            
            
            foreach (Problem problem in problems)
            {
                greedySolutions.Add(AlgorithmManager.SolveGreedy(problem, SOLUTION_SIZE));
                graspSolutions.Add(AlgorithmManager.SolveGrasp(problem, SOLUTION_SIZE, RCL_SIZE));
            }

            Console.WriteLine("GREEDY SOLUTIONS:");
            PrintSolutionInfo(greedySolutions);
            Console.WriteLine("GRASP SOLUTIONS:");
            PrintSolutionInfo(graspSolutions);
        }
    }
}