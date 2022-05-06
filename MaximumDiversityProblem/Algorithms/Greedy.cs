namespace MaximumDiversityProblem
{
    public class Greedy
    {

        static private List<int> MakeRCL(List<List<double>> vectors, HashSet<int> vectorHash, List<double> centroid, int rclSize)
        {
            List<int> rcl = Enumerable.Repeat(0, rclSize).ToList();
            List<double> rclDistanceToCentroid = Enumerable.Repeat(double.MaxValue, rclSize).ToList();
            List<int> availableVectors = vectorHash.ToList();

            for (int i = 0; i < availableVectors.Count; i++)
            {
                List<double> candidate = vectors[availableVectors[i]];
                int candidateIndex = availableVectors[i];
                double candidateDistance = Utils.GetDistance(centroid, candidate);

                for (int j = 0; j < rclSize; j++)
                {
                    if (candidateDistance > rclDistanceToCentroid[j])
                    { break; }
                    int temp = rcl[j];
                    double distTemp = rclDistanceToCentroid[j];
                    rcl.Insert(j, candidateIndex);
                    rclDistanceToCentroid.Insert(j, candidateDistance);
                    candidateIndex = temp;
                    candidateDistance = distTemp;

                }
            }
            return rcl;
        }

        static public Solution Solve(Problem problem, int rclSize)
        {
            Random rand = new Random();
            Solution solution = new Solution(problem);
            List<List<double>> vectors = problem.vectors;
            HashSet<int> availableVectors = new HashSet<int>(Enumerable.Range(0, vectors.Count).ToList());
            int numberOfVectors = problem.numberOfVectors;

            int randomToInsert = rand.Next(0, vectors.Count);
            solution.solution.Add(randomToInsert);
            availableVectors.Remove(randomToInsert);

            for (int i = 0; i < numberOfVectors; i++)
            {
                List<double> centroid = Utils.GetCentroid(vectors, solution.solution);
                List<int> rcl = MakeRCL(vectors, availableVectors, centroid, rclSize);
                if (rcl.Count <= 0)
                {
                    break;
                }
                int indexOfVector = rcl[rand.Next(0, rcl.Count)];
                solution.solution.Add(indexOfVector);
                availableVectors.Remove(indexOfVector);
            }

            return solution;
        }
    }
}
