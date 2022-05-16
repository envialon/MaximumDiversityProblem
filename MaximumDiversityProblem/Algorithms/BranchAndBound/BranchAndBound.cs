namespace MaximumDiversityProblem
{
    internal class BranchAndBound
    {

        private static List<int> SelectPartialSolution( List<List<int>> activeSolutions)
        {
            throw new NotImplementedException();
        }

        public static Solution Solve(Problem problem, int solutionSize, double LowerBound)
        {
            HashSet<List<int>> activeSolutions = new HashSet<List<int>>();
            activeSolutions.Add(new List<int>());

            while(activeSolutions.Count > 0)
            {
                List<int> currentSolution = SelectPartialSolution(activeSolutions.ToList());

                if (currentSolution.Count == solutionSize)
                {
                    float currentCost = Utils.GetSolutionDistance(currentSolution, problem);
                    LowerBound = (currentCost > LowerBound) ? currentCost : LowerBound;
                    activeSolutions.Remove(currentSolution);
                }
                else
                {
                }

            }

            throw new NotImplementedException();
        }
        
    }
}
