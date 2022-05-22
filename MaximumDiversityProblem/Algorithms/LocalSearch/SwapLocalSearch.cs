namespace MaximumDiversityProblem
{
    /// <summary>
    /// Class that implements a Swap local search
    /// </summary>
    public class SwapLocalSearch
    {
        public static Solution Search(Solution s)
        {
            List<List<float>> distanceMatrix = s.distanceMatrix;
            List<int> solution = new List<int>(s.solution);
            List<int> discarted = new List<int>(s.discarted);

            int sToSwap = -1;
            int dToSwap = -1;
            float bestDistance = s.totalDistance;


            bool foundSolution = true;
            while (foundSolution)
            {
                foundSolution = false;
                float initialDistance = bestDistance;
                for (int sIndex = 0; sIndex < solution.Count; sIndex++)
                {
                    int sCandidate = solution[sIndex];
                    float distanceAfterRemove = initialDistance - Utils.GetDistanceToSet(distanceMatrix, solution, sCandidate);
                    solution.Remove(sCandidate);

                    for (int dIndex = 0; dIndex < discarted.Count; dIndex++)
                    {
                        int dCandidate = discarted[dIndex];
                        float currentDistance = distanceAfterRemove + Utils.GetDistanceToSet(distanceMatrix, solution, dCandidate);

                        if (currentDistance > bestDistance)
                        {
                            bestDistance = currentDistance;
                            sToSwap = sCandidate;
                            dToSwap = dCandidate;
                            foundSolution = true;
                        }
                    }
                    solution.Add(sCandidate);
                }

                if (foundSolution)
                {
                    solution.Remove(sToSwap);
                    solution.Add(dToSwap);
                    discarted.Remove(dToSwap);
                    discarted.Add(sToSwap);
                }
            }

            Solution bestSolution = new Solution(s);
            bestSolution.solution = new HashSet<int>(solution);
            bestSolution.discarted = new HashSet<int>(discarted);
            bestSolution.totalDistance = bestDistance;

            return bestSolution;
        }
    }
}
