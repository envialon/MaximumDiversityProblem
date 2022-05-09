﻿namespace MaximumDiversityProblem
{
    public class Problem
    {
        public string filename = "";
        public int numberOfVectors = -1;
        public int dimensionality = -1;
        public int solutionSize = -1;
        public List<List<double>> vectors = new List<List<double>>();
        public List<List<double>> distanceMatrix = new List<List<double>>();


        public Problem(string filename, int numberOfVectors, int dimensionality, int solutionSize, List<List<double>> vectors)
        {
            this.solutionSize = solutionSize;
            this.filename = filename;
            this.numberOfVectors = numberOfVectors;
            this.dimensionality = dimensionality;
            this.vectors = vectors;
        }

        private void buildDistanceMatrix()
        {
            for (int i = 0; i < numberOfVectors; i++)
            {
                distanceMatrix.Add(new List<double>());
                for (int j = 0; j < numberOfVectors; j++)
                {
                    
                    if(i == j) {
                        distanceMatrix[i].Add(0);
                    }
                    else
                    {
                        distanceMatrix[i].Add(Utils.GetDistance(vectors[i], vectors[j]));
                    }
                }
            }
        }

        public Problem(string filename)
        {
            buildFromFile(filename);
        }
        
        private void RetrieveVectors(List<string> lines)
        {
            for (int l = 0; l < numberOfVectors; l++)
            {
                string line = lines[l];
                string processed = line.Replace(",", ".");
                string[] splitted = processed.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                List<double> vector = new List<double>();
                for (int i = 0; i < splitted.Length; i++)
                {
                    vector.Add(double.Parse(splitted[i]));
                }
                vectors.Add(vector);
            }
        }

        public void buildFromFile(string filename)
        {
            this.filename = filename.Split("\\").Last();
            List<string> lines = new List<string>(File.ReadAllLines(filename));
            this.numberOfVectors = int.Parse(lines[0]);
            this.dimensionality = int.Parse(lines[1]);
            lines.RemoveRange(0, 2);
            RetrieveVectors(lines);
            buildDistanceMatrix();
        }
    }
}
