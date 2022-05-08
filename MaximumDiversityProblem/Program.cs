
namespace MaximumDiversityProblem
{
    class Program
    {
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
                Console.WriteLine("Greedy solution total distance = " + greedySolutions.Last().totalDistance);

            }

        }
    }
}