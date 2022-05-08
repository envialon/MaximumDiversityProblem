namespace MaximumDiversityProblem
{
    public class Greedy
    {

        static private List<int> MakeRCL(List<List<double>> vectors, HashSet<int> vectorHash, List<double> centroid, int rclSize)
        {

            List<int> availableVectors = vectorHash.ToList();
            List<int> rcl = Enumerable.Repeat(availableVectors[0], rclSize).ToList();
            List<double> rclDistanceToCentroid = Enumerable.Repeat(double.MaxValue, rclSize).ToList();

            for (int i = 0; i < availableVectors.Count; i++)
            {

                int candidateIndex = availableVectors[i];
                List<double> candidate = vectors[candidateIndex];
                double candidateDistance = Utils.GetDistance(centroid, candidate);

                for (int j = 0; j < rclSize; j++)
                {
                    if (candidateDistance > rclDistanceToCentroid[j])
                    { break; }
                    int temp = rcl[j];
                    double distTemp = rclDistanceToCentroid[j];
                    rcl[j] = candidateIndex;
                    rclDistanceToCentroid[j] =  candidateDistance;
                    candidateIndex = temp;
                    candidateDistance = distTemp;
                }
            }
            return rcl;
        }

        static public Solution Solve(Problem problem, int solutionSize, int rclSize)
        {
            Random rand = new Random();
            Solution solution = new Solution(problem);            
            List<List<double>> vectors = problem.vectors;
            HashSet<int> availableVectors = new HashSet<int>(Enumerable.Range(0, vectors.Count).ToList());

            List<double> centroid ;
            List<int> rcl;
            
            int indexToInsert = 0;
            solution.solution.Add(indexToInsert);
            availableVectors.Remove(indexToInsert);         

            for (int i = 1; i < solutionSize; i++)
            {
                centroid = Utils.GetCentroid(vectors, solution.solution);
                rcl = MakeRCL(vectors, availableVectors, centroid, rclSize);
                indexToInsert = rcl[rand.Next(0, rcl.Count)];
                solution.solution.Add(indexToInsert);
                availableVectors.Remove(indexToInsert);
            }

            solution.rclSize = rclSize;
            solution.discarted = new HashSet<int>(availableVectors);
            solution.totalDistance = Utils.GetSolutionDistance(solution);
            return solution;
        }
    }
}
