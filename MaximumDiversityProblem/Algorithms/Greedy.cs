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

            int randomToInsert = 0;//rand.Next(0, vectors.Count);
            solution.solution.Add(randomToInsert);
            availableVectors.Remove(randomToInsert);

            List<int> rcl;

            for (int i = 1; i < solutionSize; i++)
            {
                List<double> centroid = Utils.GetCentroid(vectors, solution.solution);
                 rcl = MakeRCL(vectors, availableVectors, centroid, rclSize);
                randomToInsert = rcl[rand.Next(0, rcl.Count)];
                solution.solution.Add(randomToInsert);
                availableVectors.Remove(randomToInsert);
            }

            return solution;
        }
    }
}
