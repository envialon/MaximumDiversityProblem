namespace NLP_IAA
{
    class Program
    {
        static void Main(string[] args)
        {
            //ProbabiltyEstimation.GoodModel("COV_train.xlsx", "vocabulario.txt");

            Console.WriteLine("Introduce the test filename:");
            string testFile = Console.ReadLine();
            //string testFile = "COV_test_g1.xlsx";
            Classifier.Classify(testFile);

            Console.WriteLine("Introduce the validation filename:");
            string validationFile = Console.ReadLine();
            //string validationFile =  "COV_test_g1_debug.xlsx";
            Console.WriteLine(Classifier.Validate("resumen_alu0101337760.txt", validationFile));
        }
    }
}