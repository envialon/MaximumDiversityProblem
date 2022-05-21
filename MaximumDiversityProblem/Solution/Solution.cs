namespace MaximumDiversityProblem
{
    public class Solution
    {
        public List<List<float>> vectors = new List<List<float>>();
        public List<List<float>> distanceMatrix = new List<List<float>>();
        public HashSet<int> solution = new HashSet<int>();
        public HashSet<int> discarted = new HashSet<int>();
        public long elapsedMilliseconds = -1;
        public float totalDistance = -1;
        public int dimensionality = -1;
        public int rclSize = -1;
        public string id = "undefined";

        public Solution(Problem problem, int rclSize = -1)
        {
            this.dimensionality = problem.dimensionality;
            this.vectors = new List<List<float>>(problem.vectors);
            this.rclSize = rclSize;
            this.distanceMatrix = new List<List<float>>(problem.distanceMatrix);
            this.id = problem.filename;
        }

        public Solution(Problem problem, PartialSolution solution, List<int> discarted)
        {
            this.dimensionality = problem.dimensionality;
            this.vectors = new List<List<float>>(problem.vectors);
            this.distanceMatrix = new List<List<float>>(problem.distanceMatrix);
            this.id = problem.filename;
            this.discarted = new HashSet<int>(discarted);
            this.solution = new HashSet<int>(solution.solution);
            this.totalDistance = solution.upperBound;            
        }
        public Solution(Problem problem, PartialSolution solution, HashSet<int> discarted)
        {
            this.dimensionality = problem.dimensionality;
            this.vectors = new List<List<float>>(problem.vectors);
            this.distanceMatrix = new List<List<float>>(problem.distanceMatrix);
            this.id = problem.filename;
            this.discarted = new HashSet<int>(discarted);
            this.solution = new HashSet<int>(solution.solution);
            this.totalDistance = solution.upperBound;
        }

        public Solution(Solution solution)
        {
            this.id = solution.id;
            this.dimensionality = solution.dimensionality;
            this.rclSize = solution.rclSize;
            this.vectors = new List<List<float>>(solution.vectors);
            this.solution = new HashSet<int>(solution.solution);
            this.distanceMatrix = new List<List<float>>(solution.distanceMatrix);
            this.discarted = new HashSet<int>(solution.discarted);
            this.totalDistance = solution.totalDistance;
        }

        public void add(int index)
        {
            solution.Add(index);
        }

        public void remove(int index)
        {
            solution.Remove(index);
        }

        public string GetTable()
        {
            return "";
        }

    }
}
