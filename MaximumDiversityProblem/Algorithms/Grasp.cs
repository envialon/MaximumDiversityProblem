﻿using System.Diagnostics;
namespace MaximumDiversityProblem
{
    public class Grasp
    {
        public static Solution Solve(Problem problem, int solutionSize, int maxIter,  int rclSize)
        {
            Stopwatch sw = new Stopwatch();
            Solution bestSolution = new Solution(problem);
            bestSolution.totalDistance = float.MinValue;
            int iterationsWithoutImprovement = 1000;

            sw.Start();

            for (int i = 0; i < iterationsWithoutImprovement; i++)
            {
                Solution processed = SwapLocalSearch.Search(Greedy.Solve(problem, solutionSize, rclSize));
                if(processed.totalDistance > bestSolution.totalDistance)
                {
                    bestSolution = processed;
                    i = 0;
                }
            }
            sw.Stop();
            bestSolution.elapsedMilliseconds = sw.ElapsedMilliseconds;
            bestSolution.iterations = maxIter;
            return bestSolution;
        }
    }
}
