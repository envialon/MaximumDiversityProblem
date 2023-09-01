# MaximumDiversityProblem

# Introduction

The Maximum Diversity Problem aims to find the subset of elements with maximum diversity from a given set of elements. 
Given a set $S = \{s_1, s_2, s_3, \ldots, s_n\}$, where each element is a vector of dimension $k$, and $d_{ij}$
represents the distance between elements $i$ and $j$. With $m < n$ being the size of the solution subset of the problem, 
the objective is to find:

Maximize: $$z = \sum_{i=1}^{n-1} \sum_{j=i+1}^{n} d_{ij} x_i x_j$$

Subject to:
$$\sum_{i=1}^{n} x_i = m$$
$$x_i \in \{0, 1\} \) for all \( i = 1, 2, \ldots, n$$

Where:
- $x_i$:
  - $1$ if it belongs to the solution
  - $0$ otherwise

The distance $d_{ij}$ depends on the specific application context, on this case, we will use the euclid distance.

## Code Description

In this implementation, object-oriented programming with C# has been used without the use of any external libraries.

### 2.1. Problem
We have created a 'Problem' class responsible for extracting information stored in data files. It stores the filename, each of the vectors contained in the file, and constructs a distance matrix between each pair of vectors.

### 2.2. Solution
In this implementation, we use an index-based representation for solutions. Specifically, we have a `HashSet<int> solution` for the indices of the solution set. The 'Solution' class also stores the list of total vectors in the problem.

### 2.3. Developed Algorithms
Three algorithms and a local search have been implemented, detailed below:

#### 2.3.1. Greedy
The 'Greedy' class implements a greedy algorithm with a Restricted Candidate List. To solve the problem purely greedily, the RCL size is assigned to 1 by default, making the algorithm behave like a traditional greedy algorithm.

#### 2.3.2. Grasp
The 'Grasp' class implements a GRASP algorithm. The 'Solve' method takes the RCL size for the constructive phase and the solution size to be found as parameters. The implemented local searches are:

- Swap: The performed move is swapping an element that is in the solution with one that is not.

#### 2.3.3. Branch and Bound
The 'BranchAndBound' class implements the Branch and Bound algorithm. During the algorithm's development, we use a 'PartialSolution' class to store the partial solutions generated as we explore the solution tree.

## 3. Results
Below are the best solutions found by the implemented algorithms:

## 3.1. Greedy Algorithm Results

| Filename          | n   | dim | s_size | cost  | milliseconds |
|-------------------|-----|-----|--------|-------|--------------|
| max div 15 2.txt  | 15  | 2   | 2      | 11.86 | 1            |
| max div 15 3.txt  | 15  | 3   | 2      | 13.27 | 0            |
| max div 20 2.txt  | 20  | 2   | 2      | 8.51  | 0            |
| max div 20 3.txt  | 20  | 3   | 2      | 11.80 | 0            |
| max div 30 2.txt  | 30  | 2   | 2      | 11.66 | 0            |
| max div 30 3.txt  | 30  | 3   | 2      | 13.07 | 0            |
| max div 15 2.txt  | 15  | 2   | 3      | 25.72 | 0            |
| max div 15 3.txt  | 15  | 3   | 3      | 30.32 | 0            |
| max div 20 2.txt  | 20  | 2   | 3      | 21.99 | 0            |
| max div 20 3.txt  | 20  | 3   | 3      | 30.87 | 0            |
| max div 30 2.txt  | 30  | 2   | 3      | 28.95 | 0            |
| max div 30 3.txt  | 30  | 3   | 3      | 33.83 | 0            |
| max div 15 2.txt  | 15  | 2   | 4      | 48.41 | 0            |
| max div 15 3.txt  | 15  | 3   | 4      | 59.76 | 0            |
| max div 20 2.txt  | 20  | 2   | 4      | 39.56 | 0            |
| max div 20 3.txt  | 20  | 3   | 4      | 56.52 | 0            |
| max div 30 2.txt  | 30  | 2   | 4      | 52.77 | 0            |
| max div 30 3.txt  | 30  | 3   | 4      | 63.51 | 0            |
| max div 15 2.txt  | 15  | 2   | 5      | 73.56 | 0            |
| max div 15 3.txt  | 15  | 3   | 5      | 94.75 | 0            |
| max div 20 2.txt  | 20  | 2   | 5      | 61.22 | 0            |
| max div 20 3.txt  | 20  | 3   | 5      | 92.81 | 0            |
| max div 30 2.txt  | 30  | 2   | 5      | 80.90 | 0            |
| max div 30 3.txt  | 30  | 3   | 5      | 99.50 | 0            |

*Table 1: Greedy Algorithm Results.*

## 3.2. GRASP Algorithms

The GRASP algorithm was run with various parameters, specifically with 10 and 20 maximum iterations and with RCL sizes of 2 and 3. However, despite parameter variations, the results are identical for each of the problems. The following table shows simplified results, eliminating repetitions. GRASP algorithm results:

| Filename          | n   | dim | rcl | s_size | cost  | milliseconds |
|-------------------|-----|-----|-----|--------|-------|--------------|
| max div 15 2.txt  | 15  | 2   | 2   | 2      | 11.86 | 10           |
| max div 15 3.txt  | 15  | 3   | 2   | 2      | 13.27 | 7            |
| max div 20 2.txt  | 20  | 2   | 2   | 2      | 8.51  | 9            |
| max div 20 3.txt  | 20  | 3   | 2   | 2      | 11.80 | 9            |
| max div 30 2.txt  | 30  | 2   | 2   | 2      | 11.66 | 9            |
| max div 30 3.txt  | 30  | 3   | 2   | 2      | 13.07 | 11           |
| max div 15 2.txt  | 15  | 2   | 2   | 3      | 27.38 | 11           |
| max div 15 3.txt  | 15  | 3   | 2   | 3      | 31.87 | 11           |
| max div 20 2.txt  | 20  | 2   | 2   | 3      | 21.99 | 12           |
| max div 20 3.txt  | 20  | 3   | 2   | 3      | 30.87 | 13           |
| max div 30 2.txt  | 30  | 2   | 2   | 3      | 28.95 | 16           |
| max div 30 3.txt  | 30  | 3   | 2   | 3      | 34.28 | 19           |
| max div 15 2.txt  | 15  | 2   | 2   | 4      | 49.84 | 14           |
| max div 15 3.txt  | 15  | 3   | 2   | 4      | 59.76 | 12           |
| max div 20 2.txt  | 20  | 2   | 2   | 4      | 40.00 | 17           |
| max div 20 3.txt  | 20  | 3   | 2   | 4      | 56.68 | 16           |
| max div 30 2.txt  | 30  | 2   | 2   | 4      | 52.77 | 51           |
| max div 30 3.txt  | 30  | 3   | 2   | 4      | 63.69 | 27           |
| max div 15 2.txt  | 15  | 2   | 2   | 5      | 79.13 | 16           |
| max div 15 3.txt  | 15  | 3   | 2   | 5      | 96.10 | 13           |
| max div 20 2.txt  | 20  | 2   | 2   | 5      | 63.65 | 19           |
| max div 20 3.txt  | 20  | 3   | 2   | 5      | 92.81 | 19           |
| max div 30 2.txt  | 30  | 2   | 2   | 5      | 80.90 | 51           |
| max div 30 3.txt  | 30  | 3   | 2   | 5      | 99.58 | 79           |

*Table 2: GRASP Algorithm Results with multiroute reinsertion as local search.*

## 3.3. Branch and Bound Algorithm with DFS

Results of the Branch and Bound Algorithm with Depth-First Search:

| Filename          | n   | dim | s_size | cost  | milliseconds | generated |
|-------------------|-----|-----|--------|-------|--------------|-----------|
| max div 15 2.txt  | 15  | 2   | 2      | 11.86 | 2            | 15        |
| max div 15 3.txt  | 15  | 3   | 2      | 13.27 | 1            | 15        |
| max div 20 2.txt  | 20  | 2   | 2      | 8.51  | 2            | 20        |
| max div 20 3.txt  | 20  | 3   | 2      | 11.80 | 2            | 20        |
| max div 30 2.txt  | 30  | 2   | 2      | 11.66 | 10           | 30        |
| max div 30 3.txt  | 30  | 3   | 2      | 13.07 | 10           | 30        |
| max div 15 2.txt  | 15  | 2   | 3      | 27.38 | 4            | 108       |
| max div 15 3.txt  | 15  | 3   | 3      | 31.87 | 4            | 109       |
| max div 20 2.txt  | 20  | 2   | 3      | 21.99 | 14           | 191       |
| max div 20 3.txt  | 20  | 3   | 3      | 30.87 | 15           | 191       |
| max div 30 2.txt  | 30  | 2   | 3      | 28.95 | 62           | 436       |
| max div 30 3.txt  | 30  | 3   | 3      | 34.28 | 34           | 437       |
| max div 15 2.txt  | 15  | 2   | 4      | 49.84 | 3            | 459       |
| max div 15 3.txt  | 15  | 3   | 4      | 59.76 | 4            | 456       |
| max div 20 2.txt  | 20  | 2   | 4      | 40.00 | 28           | 1109      |
| max div 20 3.txt  | 20  | 3   | 4      | 56.68 | 32           | 1123      |
| max div 30 2.txt  | 30  | 2   | 4      | 52.77 | 209          | 4005      |
| max div 30 3.txt  | 30  | 3   | 4      | 63.69 | 228          | 3936      |
| max div 15 2.txt  | 15  | 2   | 5      | 79.13 | 9            | 1341      |
| max div 15 3.txt  | 15  | 3   | 5      | 96.10 | 10           | 1341      |
| max div 20 2.txt  | 20  | 2   | 5      | 63.65 | 60           | 4235      |
| max div 20 3.txt  | 20  | 3   | 5      | 92.81 | 61           | 3698      |
| max div 30 2.txt  | 30  | 2   | 5      | 80.90 | 1245         | 24924     |
| max div 30 3.txt  | 30  | 3   | 5      | 99.58 | 1172         | 23232     |

*Table 3: Branch and Bound Algorithm Results with Depth-First Search.*
## 3.4. Branch and Bound Algorithm with SUF

Results of the Branch and Bound Algorithm with Smallest Upperbound First (SUF):

| Filename          | n   | dim | s_size | cost  | milliseconds | generated |
|-------------------|-----|-----|--------|-------|--------------|-----------|
| max div 15 2.txt  | 15  | 2   | 2      | 11.86 | 0            | 15        |
| max div 15 3.txt  | 15  | 3   | 2      | 13.27 | 0            | 15        |
| max div 20 2.txt  | 20  | 2   | 2      | 8.51  | 0            | 20        |
| max div 20 3.txt  | 20  | 3   | 2      | 11.80 | 0            | 20        |
| max div 30 2.txt  | 30  | 2   | 2      | 11.66 | 3            | 30        |
| max div 30 3.txt  | 30  | 3   | 2      | 13.07 | 3            | 30        |
| max div 15 2.txt  | 15  | 2   | 3      | 27.38 | 1            | 109       |
| max div 15 3.txt  | 15  | 3   | 3      | 31.87 | 1            | 108       |
| max div 20 2.txt  | 20  | 2   | 3      | 21.99 | 4            | 191       |
| max div 20 3.txt  | 20  | 3   | 3      | 30.87 | 4            | 191       |
| max div 30 2.txt  | 30  | 2   | 3      | 28.95 | 47           | 436       |
| max div 30 3.txt  | 30  | 3   | 3      | 34.28 | 58           | 437       |
| max div 15 2.txt  | 15  | 2   | 4      | 49.84 | 6            | 461       |
| max div 15 3.txt  | 15  | 3   | 4      | 59.76 | 6            | 456       |
| max div 20 2.txt  | 20  | 2   | 4      | 40.00 | 51           | 1110      |
| max div 20 3.txt  | 20  | 3   | 4      | 56.68 | 52           | 1123      |
| max div 30 2.txt  | 30  | 2   | 4      | 52.77 | 228          | 4005      |
| max div 30 3.txt  | 30  | 3   | 4      | 63.69 | 317          | 3936      |
| max div 15 2.txt  | 15  | 2   | 5      | 79.13 | 9            | 1342      |
| max div 15 3.txt  | 15  | 3   | 5      | 96.10 | 9            | 1343      |
| max div 20 2.txt  | 20  | 2   | 5      | 63.65 | 59           | 4243      |
| max div 20 3.txt  | 20  | 3   | 5      | 92.81 | 53           | 3698      |
| max div 30 2.txt  | 30  | 2   | 5      | 80.90 | 1309         | 24924     |
| max div 30 3.txt  | 30  | 3   | 5      | 99.58 | 1165         | 23232     |

*Table 4: Branch and Bound Algorithm Results with Smallest Upperbound First (SUF).*

## 4. Conclusions

Comparing the obtained results, we can observe that despite Branch and Bound being an exact algorithm, it is not capable of finding solutions of higher quality than GRASP, and it is also more than 10 times slower.

This is due to the small number of vectors in the problem as well as the small solution sizes. In this context, GRASP can find the optimal solution. However, by increasing the number of vectors to evaluate, we should notice a difference in the quality of the objective function values of the solutions found by the algorithms.
