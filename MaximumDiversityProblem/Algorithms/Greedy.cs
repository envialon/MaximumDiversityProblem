using System.Diagnostics;
namespace MaximumDiversityProblem
{
    public class Greedy
    {

        static private List<int> MakeRCL(List<List<float>> vectors, HashSet<int> vectorHash, List<float> centroid, int rclSize)
        {

            List<int> availableVectors = vectorHash.ToList();
            List<int> rcl = Enumerable.Repeat(availableVectors[0], rclSize).ToList();
            List<float> rclDistanceToCentroid = Enumerable.Repeat(float.MinValue, rclSize).ToList();

            for (int i = 0; i < availableVectors.Count; i++)
            {

                int candidateIndex = availableVectors[i];
                List<float> candidate = vectors[candidateIndex];
                float candidateDistance = Utils.GetDistance(centroid, candidate);

                for (int j = 0; j < rclSize; j++)
                {
                    if (candidateDistance <= rclDistanceToCentroid[j])
                    { continue; }
                    int temp = rcl[j];
                    float distTemp = rclDistanceToCentroid[j];
                    rcl[j] = candidateIndex;
                    rclDistanceToCentroid[j] = candidateDistance;
                    candidateIndex = temp;
                    candidateDistance = distTemp;
                }
            }
            return rcl;
        }

        static public Solution Solve(Problem problem, int solutionSize, int rclSize)
        {
            Stopwatch sw = new Stopwatch();
            Random rand = new Random();
            Solution solution = new Solution(problem);
            List<List<float>> vectors = problem.vectors;
            HashSet<int> availableVectors = new HashSet<int>(Enumerable.Range(0, vectors.Count).ToList());

            List<float> centroid = Utils.GetCentroid(vectors, availableVectors);
            List<int> rcl;


            sw.Start();

            for (int i = 0; i < solutionSize; i++)
            {
                rcl = MakeRCL(vectors, availableVectors, centroid, rclSize);
                int indexToInsert = rcl[rand.Next(0, rcl.Count)];
                solution.solution.Add(indexToInsert);
                availableVectors.Remove(indexToInsert);
                centroid = Utils.GetCentroid(vectors, solution.solution);
            }
            sw.Stop();

            solution.totalDistance = Utils.GetSolutionDistance(solution);
            solution.elapsedMilliseconds = sw.ElapsedMilliseconds;
            solution.rclSize = rclSize;
            solution.discarted = new HashSet<int>(availableVectors);
            return solution;
        }
    }
}
