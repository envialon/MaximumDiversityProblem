namespace MaximumDiversityProblem
{
    public class SwapLocalSearch
    {
        public static Solution Search(Solution s)
        {
            List<List<double>> distanceMatrix = s.distanceMatrix;
            List<int> solution = new List<int>(s.solution);
            List<int> discarted = new List<int>(s.discarted);
            
            bool foundSolution = true;
            while (foundSolution)
            {
                foundSolution = false;
                for (int sIndex = 0; sIndex < solution.Count; sIndex++)
                {
                    int sCandidate = solution[sIndex];
                                        


                    for (int dIndex = 0; dIndex < discarted.Count; dIndex++)
                    {
                        int dCandidate = discarted[dIndex];

                    }
                }
            }

            Solution bestSolution = new Solution(s);
            bestSolution.solution = new HashSet<int>(solution);
            bestSolution.discarted = new HashSet<int>(discarted);

            return bestSolution;
        }
    }
}
