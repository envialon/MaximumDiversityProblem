namespace MaximumDiversityProblem
{
    public class PartialSolution
    {
        public List<int> solution = new List<int>();
        public int depth = -1;
        public float upperBound = -1;
    
        public PartialSolution(List<int> solution, float upperBound)
        {
            this.solution = solution;
            this.upperBound = upperBound;
            this.depth = solution.Count;
        }
    }
}
