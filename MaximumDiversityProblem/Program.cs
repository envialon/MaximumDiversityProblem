
namespace MaximumDiversityProblem
{
    class Program
    {
        private static void PrintSolutionInfo(List<Solution> solutions)
        {
            Console.WriteLine(String.Format("\tfilename\tn\tdim\ts_size\tcost\tmilliseconds"));
            foreach (Solution solution in solutions)
            {
                Console.WriteLine(solution.id + "\t" + solution.vectors.Count + "\t" + solution.dimensionality + "\t" 
                    + solution.solution.Count + "\t" + solution.totalDistance.ToString("0.00") + "\t" + solution.elapsedMilliseconds);
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

            int SOLUTION_SIZE = 6;


            List<Problem> problems = new List<Problem>();
            List<Solution> greedySolutions = new List<Solution>();

            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                problems.Add(new Problem(filename));
            }

            foreach (Problem problem in problems)
            {
                greedySolutions.Add(AlgorithmManager.SolveGreedy(problem, SOLUTION_SIZE));
            }

            PrintSolutionInfo(greedySolutions);
        }
    }
}