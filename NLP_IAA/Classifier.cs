using ExcelDataReader;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using WeCantSpell.Hunspell;

namespace NLP_IAA
{
    public class Solution
    {
        public string beginning;
        public double pProbability;
        public double nProbability;
        public string classification;

        public Solution(string beginning, double pProbability, double nProbability, string classification)
        {
            this.beginning = beginning;
            this.pProbability = pProbability;
            this.nProbability = nProbability;
            this.classification = classification;

        }
    }
    public class Classifier
    {
        static string[] punct = { ".", ",", "!", "?", ":", ";", "\"", "(", ")", "[", "]", "{", "}", "<", ">", "/", "\\", "|", "=", "+", "_" };

        public static void Classify(string testFile)
        {
            int nOfPositiveTweets = int.Parse(File.ReadLines("models\\modelo_lenguaje_P.txt").ToList()[0].Split(":")[1]);
            int nOfNegativeTweets = int.Parse(File.ReadLines("models\\modelo_lenguaje_N.txt").ToList()[0].Split(":")[1]);

            Dictionary<string, (int, double)> positiveDic = GetDictionaryFromModel("models\\modelo_lenguaje_P.txt");
            Dictionary<string, (int, double)> negativeDic = GetDictionaryFromModel("models\\modelo_lenguaje_N.txt");

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FileStream file = File.Open(testFile, FileMode.Open, FileAccess.Read);
            DataSet table = ExcelReaderFactory.CreateReader(file, new ExcelReaderConfiguration()).AsDataSet();

            List<string[]> processedTweets = new List<string[]>();
            using var dictionaryStream = File.OpenRead(@"English_American.dic");
            using var affixStream = File.OpenRead(@"English_American.aff");
            WordList dictionary = WordList.CreateFromStreams(dictionaryStream, affixStream);
            for (int i = 0; i < table.Tables[0].Rows.Count; i++)
                processedTweets.Add(Preprocess(table.Tables[0].Rows[i][0].ToString(), dictionary));

            file.Close();
            List<Solution> solutions = new List<Solution>();

            int j = 0;
            foreach (string[] tweet in processedTweets)
            {
                string beggining = table.Tables[0].Rows[j][0].ToString();
                
                beggining = beggining.Substring(0, 10);
                beggining = beggining.Replace('\n', ' ');
                
                double pProbability = 0;
                double nProbability = 0;
                string type = "";

                foreach (string word in tweet)
                {
                    if (word != null && !stopWords.Contains(word))
                    {
                        if (positiveDic.ContainsKey(word))
                        {
                            pProbability += positiveDic[word].Item2;
                        }
                        else
                        {
                           pProbability += positiveDic["<UNK>"].Item2;
                        }

                        if (negativeDic.ContainsKey(word))
                        {
                            nProbability += negativeDic[word].Item2;
                        }
                        else
                        {
                           nProbability += negativeDic["<UNK>"].Item2;
                        }
                    }
                }

                nProbability += Math.Log((double)nOfNegativeTweets / (double)(nOfNegativeTweets + nOfPositiveTweets));
                pProbability += Math.Log((double)nOfPositiveTweets / (double)(nOfNegativeTweets + nOfPositiveTweets));


                pProbability = Math.Round(pProbability, 2);
                nProbability = Math.Round(nProbability, 2);

                type = pProbability > nProbability ? "P" : "N";
                solutions.Add(new Solution(beggining, pProbability, nProbability, type));
                j++;
            }

            using (StreamWriter fileWriter = new StreamWriter("clasificacion_alu0101337760.txt"))
            {
                foreach (Solution solution in solutions)
                {
                    fileWriter.WriteLine(
                        solution.beginning + "," + Math.Round(solution.pProbability, 2) + "," + Math.Round(solution.nProbability, 2) + "," + solution.classification);

                }
            }

            using (StreamWriter fileWriter = new StreamWriter("resumen_alu0101337760.txt"))
            {
                foreach (Solution solution in solutions)
                {
                    fileWriter.WriteLine(solution.classification);
                }
            }

        }

        public static string Validate(string predictedFile, string realFile)
        {

            List<string> predicted = new  List<string>(File.ReadAllLines(predictedFile));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FileStream file = File.Open(realFile, FileMode.Open, FileAccess.Read);
            DataSet table = ExcelReaderFactory.CreateReader(file, new ExcelReaderConfiguration()).AsDataSet();
            file.Close();

            List<string> real = new List<string>();
            List<string> toPredict = new List<string>();

            for (int i = 0; i < table.Tables[0].Rows.Count; i++)
            {
                int index = int.Parse(table.Tables[0].Rows[i][0].ToString()) -1;
                string value = table.Tables[0].Rows[i][2].ToString();
                real.Add(value);
                toPredict.Add(predicted[index]);
            }

            return Report(toPredict, real);
        }

        private static string Report(List<string> classification, List<string> realClass)
        {
            int correct = 0;
            int Positive = 0;
            int realPositive = 0;
            int Negative = 0;
            int realNegative = 0;
            int[,] confusionMatrix = new int[2, 2];
            for (int i = 0; i < classification.Count; i++)
            {
                if (realClass[i] == "Positive")
                {
                    realPositive++;
                    if (classification[i] == "P")
                    {
                        confusionMatrix[0, 0]++;
                        correct++;
                        Positive++;
                    }
                    else
                    {
                        confusionMatrix[1, 0]++;
                        Negative++;
                    }
                }
                else
                {
                    realNegative++;
                    if (classification[i] == "P")
                    {
                        confusionMatrix[0, 1]++;
                        Positive++;
                    }
                    else
                    {
                        confusionMatrix[1, 1]++;
                        correct++;
                        Negative++;
                    }

                }
            }
            return $" Positive:{Positive}/{realPositive}\n Negative:{Negative}/{realNegative}\n Correct:{correct}/{classification.Count}\n" +
                $" Confusion Matrix: \n   {confusionMatrix[0, 0],5}   {confusionMatrix[0, 1],5}\n   {confusionMatrix[1, 0],5}   {confusionMatrix[1, 1],5}\n" +
                $" Accuracy: {((double)correct / classification.Count * 100).ToString("00.000")} %";
        }

        public static Dictionary<string, (int, double)> GetDictionaryFromModel(string modelFile)
        {
            List<string> lines = new List<string>(File.ReadAllLines(modelFile));
            lines.RemoveRange(0, 2);

            Dictionary<string, (int, double)> dictionary = new Dictionary<string, (int, double)>();

            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                string word = split[0].Split(":")[1];
                string freq = split[1].Split(":")[1];
                string prob = split[2].Split(":")[1];

                prob = prob.Replace(",", ".");

                dictionary.Add(word, (int.Parse(freq), double.Parse(prob)));

            }

            return dictionary;
        }

        private static string[] Preprocess(string data, WordList dictionary)
        {
            string[] punct = { ".", ",", "!", "?", ":", ";", "\"", "(", ")", "[", "]", "{", "}", "<", ">", "/", "\\", "|", "=", "+", "_", "-", "\u0080", "\u0091", "\u0092", "\u0093", "\u0094", "\u0095", "\u0096", "\u0097", "\u0099", "$", "~", "^", "%", "&", "*", "'", "«", "»", "£", "¥", "`", "®", "©" };
            string[] result = ReplaceAll(punct, data.ToLower(), " ").Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            List<string> filtered = new();
            for (int i = 0; i < result.Length; i++)
                if (dictionary.Check(result[i]) && result[i][0] != '#' && result[i].Length > 2 && result[i][0] != '@' && !result[i].StartsWith("http") && !int.TryParse($"{result[i][0]}", out _) && !stopWords.Contains(result[i]))
                    filtered.Add(result[i]);
            return filtered.ToArray();

        }

        private static string ReplaceAll(string[] pattern, string str, string replacement = "")
        {
            {
                for (int c = 0; c < pattern.Length; c++)
                {
                    str = str.Replace(pattern[c], replacement);
                }
                return str;
            }
        }

        static string[] stopWords =
        {
            "a",
            "able",
            "about",
            "above",
            "according",
            "accordingly",
            "across",
            "actually",
            "after",
            "afterwards",
            "again",
            "against",
            "ain't",
            "all",
            "allow",
            "allows",
            "almost",
            "alone",
            "along",
            "already",
            "also",
            "although",
            "always",
            "am",
            "among",
            "amongst",
            "an",
            "and",
            "another",
            "any",
            "anybody",
            "anyhow",
            "anyone",
            "anything",
            "anyway",
            "anyways",
            "anywhere",
            "apart",
            "appear",
            "appreciate",
            "appropriate",
            "are",
            "aren't",
            "around",
            "as",
            "a's",
            "aside",
            "ask",
            "asking",
            "associated",
            "at",
            "available",
            "away",
            "awfully",
            "be",
            "became",
            "because",
            "become",
            "becomes",
            "becoming",
            "been",
            "before",
            "beforehand",
            "behind",
            "being",
            "believe",
            "below",
            "beside",
            "besides",
            "best",
            "better",
            "between",
            "beyond",
            "both",
            "brief",
            "but",
            "by",
            "came",
            "can",
            "cannot",
            "cant",
            "can't",
            "cause",
            "causes",
            "certain",
            "certainly",
            "changes",
            "clearly",
            "c'mon",
            "co",
            "com",
            "come",
            "comes",
            "concerning",
            "consequently",
            "consider",
            "considering",
            "contain",
            "containing",
            "contains",
            "corresponding",
            "could",
            "couldn't",
            "course",
            "c's",
            "currently",
            "definitely",
            "described",
            "despite",
            "did",
            "didn't",
            "different",
            "do",
            "does",
            "doesn't",
            "doing",
            "done",
            "don't",
            "down",
            "downwards",
            "during",
            "each",
            "edu",
            "eg",
            "eight",
            "either",
            "else",
            "elsewhere",
            "enough",
            "entirely",
            "especially",
            "et",
            "etc",
            "even",
            "ever",
            "every",
            "everybody",
            "everyone",
            "everything",
            "everywhere",
            "ex",
            "exactly",
            "example",
            "except",
            "far",
            "few",
            "fifth",
            "first",
            "five",
            "followed",
            "following",
            "follows",
            "for",
            "former",
            "formerly",
            "forth",
            "four",
            "from",
            "further",
            "furthermore",
            "get",
            "gets",
            "getting",
            "given",
            "gives",
            "go",
            "goes",
            "going",
            "gone",
            "got",
            "gotten",
            "greetings",
            "had",
            "hadn't",
            "happens",
            "hardly",
            "has",
            "hasn't",
            "have",
            "haven't",
            "having",
            "he",
            "he'd",
            "he'll",
            "hello",
            "help",
            "hence",
            "her",
            "here",
            "hereafter",
            "hereby",
            "herein",
            "here's",
            "hereupon",
            "hers",
            "herself",
            "he's",
            "hi",
            "him",
            "himself",
            "his",
            "hither",
            "hopefully",
            "how",
            "howbeit",
            "however",
            "how's",
            "i",
            "i'd",
            "ie",
            "if",
            "ignored",
            "i'll",
            "i'm",
            "immediate",
            "in",
            "inasmuch",
            "inc",
            "indeed",
            "indicate",
            "indicated",
            "indicates",
            "inner",
            "insofar",
            "instead",
            "into",
            "inward",
            "is",
            "isn't",
            "it",
            "it'd",
            "it'll",
            "its",
            "it's",
            "itself",
            "i've",
            "just",
            "keep",
            "keeps",
            "kept",
            "know",
            "known",
            "knows",
            "last",
            "lately",
            "later",
            "latter",
            "latterly",
            "least",
            "less",
            "lest",
            "let",
            "let's",
            "like",
            "liked",
            "likely",
            "little",
            "look",
            "looking",
            "looks",
            "ltd",
            "mainly",
            "many",
            "may",
            "maybe",
            "me",
            "mean",
            "meanwhile",
            "merely",
            "might",
            "more",
            "moreover",
            "most",
            "mostly",
            "much",
            "must",
            "mustn't",
            "my",
            "myself",
            "name",
            "namely",
            "nd",
            "near",
            "nearly",
            "necessary",
            "need",
            "needs",
            "neither",
            "never",
            "nevertheless",
            "new",
            "next",
            "nine",
            "no",
            "nobody",
            "non",
            "none",
            "noone",
            "nor",
            "normally",
            "not",
            "nothing",
            "novel",
            "now",
            "nowhere",
            "obviously",
            "of",
            "off",
            "often",
            "oh",
            "ok",
            "okay",
            "old",
            "on",
            "once",
            "one",
            "ones",
            "only",
            "onto",
            "or",
            "other",
            "others",
            "otherwise",
            "ought",
            "our",
            "ours",
            "ourselves",
            "out",
            "outside",
            "over",
            "overall",
            "own",
            "particular",
            "particularly",
            "per",
            "perhaps",
            "placed",
            "please",
            "plus",
            "possible",
            "presumably",
            "probably",
            "provides",
            "que",
            "quite",
            "qv",
            "rather",
            "rd",
            "re",
            "really",
            "reasonably",
            "regarding",
            "regardless",
            "regards",
            "relatively",
            "respectively",
            "right",
            "said",
            "same",
            "saw",
            "say",
            "saying",
            "says",
            "second",
            "secondly",
            "see",
            "seeing",
            "seem",
            "seemed",
            "seeming",
            "seems",
            "seen",
            "self",
            "selves",
            "sensible",
            "sent",
            "serious",
            "seriously",
            "seven",
            "several",
            "shall",
            "shan't",
            "she",
            "she'd",
            "she'll",
            "she's",
            "should",
            "shouldn't",
            "since",
            "six",
            "so",
            "some",
            "somebody",
            "somehow",
            "someone",
            "something",
            "sometime",
            "sometimes",
            "somewhat",
            "somewhere",
            "soon",
            "sorry",
            "specified",
            "specify",
            "specifying",
            "still",
            "sub",
            "such",
            "sup",
            "sure",
            "take",
            "taken",
            "tell",
            "tends",
            "th",
            "than",
            "thank",
            "thanks",
            "thanx",
            "that",
            "thats",
            "that's",
            "the",
            "their",
            "theirs",
            "them",
            "themselves",
            "then",
            "thence",
            "there",
            "thereafter",
            "thereby",
            "therefore",
            "therein",
            "theres",
            "there's",
            "thereupon",
            "these",
            "they",
            "they'd",
            "they'll",
            "they're",
            "they've",
            "think",
            "third",
            "this",
            "thorough",
            "thoroughly",
            "those",
            "though",
            "three",
            "through",
            "throughout",
            "thru",
            "thus",
            "to",
            "together",
            "too",
            "took",
            "toward",
            "towards",
            "tried",
            "tries",
            "truly",
            "try",
            "trying",
            "t's",
            "twice",
            "two",
            "un",
            "under",
            "unfortunately",
            "unless",
            "unlikely",
            "until",
            "unto",
            "up",
            "upon",
            "us",
            "use",
            "used",
            "useful",
            "uses",
            "using",
            "usually",
            "value",
            "various",
            "very",
            "via",
            "viz",
            "vs",
            "want",
            "wants",
            "was",
            "wasn't",
            "way",
            "we",
            "we'd",
            "welcome",
            "well",
            "we'll",
            "went",
            "were",
            "we're",
            "weren't",
            "we've",
            "what",
            "whatever",
            "what's",
            "when",
            "whence",
            "whenever",
            "when's",
            "where",
            "whereafter",
            "whereas",
            "whereby",
            "wherein",
            "where's",
            "whereupon",
            "wherever",
            "whether",
            "which",
            "while",
            "whither",
            "who",
            "whoever",
            "whole",
            "whom",
            "who's",
            "whose",
            "why",
            "why's",
            "will",
            "willing",
            "wish",
            "with",
            "within",
            "without",
            "wonder",
            "won't",
            "would",
            "wouldn't",
            "y'all",
            "yes",
            "yet",
            "you",
            "you'd",
            "you'll",
            "your",
            "you're",
            "yours",
            "yourself",
            "yourselves",
            "you've",
            "zero"
        };

    }
}
