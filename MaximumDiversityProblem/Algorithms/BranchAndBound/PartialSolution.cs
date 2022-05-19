namespace MaximumDiversityProblem
{
    public class PartialSolution
    {
        public  HashSet<int> solution = new HashSet<int>();
        public int depth = -1;
        public float upperBound = -1;
    
        public PartialSolution(HashSet<int> solution, float upperBound)
        {
            this.solution = solution;
            this.upperBound = upperBound;
            this.depth = solution.Count;
        }
    }
}
