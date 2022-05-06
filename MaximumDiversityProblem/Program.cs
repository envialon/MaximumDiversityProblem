
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

            List<Problem> problems = new List<Problem>();
            List<Solution> solutions = new List<Solution>();

            foreach (string filename in Directory.EnumerateFiles(path, "*.txt"))
            {
                problems.Add(new Problem(filename));
            }

            Solution solution = AlgorithmManager.SolveGreedy(problems[0]);
            Console.WriteLine("Greedy solution total distance = " + Utils.GetSolutionDistance(solution));
        }
    }
}