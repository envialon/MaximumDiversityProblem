namespace MaximumDiversityProblem
{
    public class Solution
    {
        public List<List<double>> vectors = new List<List<double>>();
        public List<List<double>> distanceMatrix = new List<List<double>>();
        public HashSet<int> solution = new HashSet<int>();
        public HashSet<int> discarted = new HashSet<int>();
        public long elapsedMilliseconds = -1;
        public double totalDistance = -1; 
        public int dimensionality = -1;
        public int rclSize = -1;
        public string id = "undefined";

        public Solution(Problem problem, int rclSize = -1)
        {
            dimensionality = problem.dimensionality;
            vectors = problem.vectors;
            this.rclSize = rclSize;
            this.distanceMatrix = problem.distanceMatrix;
            this.id = problem.filename;
        }

        public Solution(Solution solution)
        {
            this.id = solution.id;
            this.dimensionality = solution.dimensionality;
            this.rclSize = solution.rclSize;
            this.vectors = new List<List<double>>(solution.vectors);
            this.solution = new HashSet<int>(solution.solution);
            this.distanceMatrix = new List<List<double>>(solution.distanceMatrix);
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
