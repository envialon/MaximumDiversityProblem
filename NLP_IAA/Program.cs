namespace NLP_IAA
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProbabiltyEstimation.GoodModel("COV_train.xlsx", "vocabulario.txt");



            Console.WriteLine("Introduce the test filename:");
            //string testFile = Console.ReadLine();
            string testFile = "COV_train.xlsx";
            Classifier.Classify(testFile);
            Console.WriteLine(Classifier.Validate());
        }
    }
}