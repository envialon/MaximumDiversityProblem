namespace MaximumDiversityProblem
{
    public class Greedy : IAlgorithm
    {

        static private List<double> GetCentroid(List<List<double>> currentSolution)
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



        static private List<List<double>> MakeRCL(List<double> centroid, List<List<double>> availableNodes)
        {
            List<List<double>> rcl = new List<List<double>>();



            return rcl;
        }

        static public Solution Solve(Problem problem, int rclSize)
        {

            Solution solution = new Solution(problem); 
            List<List<double>> availableVectors = new List<List<double>>();
            int numberOfVectors = problem.numberOfVectors;
            int dimensionality = problem.dimensionality;


            
            for( int i = 0; i < availableVectors.Count; i++)
            {
                
            }
            
            throw new NotImplementedException();
        }
    }
}
