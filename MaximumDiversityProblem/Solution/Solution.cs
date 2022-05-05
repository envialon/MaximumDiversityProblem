namespace MaximumDiversityProblem
{
    public class Solution
    {
        public List<List<double>> vectors = new List<List<double>>();
        public HashSet<int> solution = new HashSet<int>();
        int dimensionality = -1;
        int rclSize = -1;


        public Solution(Problem problem, int rclSize = -1)
        {
            dimensionality = problem.dimensionality;
            vectors = problem.vectors;
            this.rclSize = rclSize;
        }

        public Solution(List<List<double>> vectors, int dimensionality)
        {
            this.vectors = vectors;
            this.dimensionality = dimensionality;
        }

        public void add(int index)
        {
            solution.Add(index);
        }

        public void remove(int index)
        {
            solution.Remove(index);
        }
    }
}
