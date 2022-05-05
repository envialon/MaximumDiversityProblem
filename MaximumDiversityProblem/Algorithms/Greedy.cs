namespace MaximumDiversityProblem
{
    public class Greedy : IAlgorithm
    {

        private List<double> GetCentroid(List<List<double>> currentSolution)
        {
            List<double> result = currentSolution[0];
            for(int i = 1; i < currentSolution.Count; i++)
            {
                for(int j = 0; j < currentSolution[i].Count; j++)
                {
                    result[j] += currentSolution[i][j];
                }
            }

            for(int i = 0; i < result.Count; i++)
            {
                result[i] = result[i] / currentSolution.Count;
            }

            return result;
        } 



        private List<List<double>> MakeRCL(List<double> centroid, List<List<double>> availableNodes)
        {
            List<List<double>> rcl = new List<List<double>>();



            return rcl;
        }

        public Solution Solve(Problem problem)
        {
            List<List<double>> availableVectors = new List<List<double>>();
            int numberOfVectors = problem.numberOfVectors;
            int dimensionality = problem.dimensionality;

            for( int i = 0; i < availableVectors)
            
            throw new NotImplementedException();
        }
    }
}
