namespace NLP_IAA
{
    class Program
    {
        static void Main(string[] args)
        {
            //int[] corpusSize = ProbabiltyEstimation.PreProcess("COV_train.xlsx", "vocabulario.txt");
            //ProbabiltyEstimation.Model(corpusSize[0], "CorpusP.txt", "vocabulario.txt", true);
            //ProbabiltyEstimation.Model(corpusSize[1], "CorpusN.txt", "vocabulario.txt", false);



            Console.WriteLine("Introduce the test filename:");
            //string testFile = Console.ReadLine();
            string testFile = "COV_train.xlsx";
            Classifier.Classify(testFile);
            Console.WriteLine(Classifier.Validate());
        }
    }
}