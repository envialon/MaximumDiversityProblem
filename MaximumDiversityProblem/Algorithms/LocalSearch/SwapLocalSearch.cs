namespace MaximumDiversityProblem
{
    public class SwapLocalSearch
    {
        public static Solution Search(Solution s)
        {
            List<List<double>> distanceMatrix = s.distanceMatrix;
            List<int> solution = new List<int>(s.solution);
            List<int> discarted = new List<int>(s.discarted);

            int sToSwap = -1;
            int dToSwap = -1;
            double bestDistance = s.totalDistance;


            bool foundSolution = true;
            while (foundSolution)
            {
                foundSolution = false;
                for (int sIndex = 0; sIndex < solution.Count; sIndex++)
                {
                    int sCandidate = solution[sIndex];
                    double distanceAfterRemove = bestDistance - Utils.GetDistanceToSet(distanceMatrix, solution, sCandidate);

                    for (int dIndex = 0; dIndex < discarted.Count; dIndex++)
                    {
                        int dCandidate = discarted[dIndex];
                        solution.Remove(sCandidate);
                        double currentDistance = distanceAfterRemove + Utils.GetDistanceToSet(distanceMatrix, solution, dCandidate);
                        solution.Add(sCandidate);
                        if (currentDistance > bestDistance)
                        {
                            bestDistance = currentDistance;
                            sToSwap = sCandidate;
                            dToSwap = dCandidate;
                            foundSolution = true;
                        }
                    }
                }

                if(foundSolution)
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
