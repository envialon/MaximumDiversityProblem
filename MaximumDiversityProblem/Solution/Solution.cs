namespace MaximumDiversityProblem
{
    public class Solution
    {
        public List<List<double>> vectors = new List<List<double>>();
        public HashSet<int> solution = new HashSet<int>();
        int dimensionality = -1;

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
