using ExcelDataReader;
using System.Data;
using System.Text;
using WeCantSpell.Hunspell;
namespace NLP_IAA
{
    public class ProbabiltyEstimation
    {
        static string[] punct = { ".", ",", "!", "?", ":", ";", "\"", "(", ")", "[", "]", "{", "}", "<", ">", "/", "\\", "|", "=", "+", "_" };

        public static void GoodModel(string corpus, string vocabulary)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            FileStream file = File.Open(corpus, FileMode.Open, FileAccess.Read);
            DataSet table = ExcelReaderFactory.CreateReader(file, new ExcelReaderConfiguration()).AsDataSet();
            file.Close();
            List<string[]> processedTweets = new List<string[]>();
            using var dictionaryStream = File.OpenRead(@"English_American.dic");
            using var affixStream = File.OpenRead(@"English_American.aff");
            WordList dictionary = WordList.CreateFromStreams(dictionaryStream, affixStream);
            for (int i = 0; i < table.Tables[0].Rows.Count; i++)
                processedTweets.Add(Preprocess(table.Tables[0].Rows[i][0].ToString(), dictionary));

            List<string> vocabList = new List<string>(File.ReadAllLines(vocabulary));
            vocabList.RemoveAt(0);

            List<string> positiveWords = new List<string>();
            List<string> negativeWords = new List<string>();

            int positiveTweets = 0;
            int negativeTweets = 0;

            int j = 0;
            foreach (string[] tweet in processedTweets)
            {
                string value = table.Tables[0].Rows[j][1].ToString();

                if (value == "Positive")
                {
                    positiveTweets++;
                }
                else if (value == "Negative")
                {
                    negativeTweets++;
                }

                foreach (string word in tweet)
                {
                    if (word != null && !stopWords.Contains(word))
                    {
                        if (value == "Positive")
                        {
                            positiveWords.Add(word);
                        }
                        else if (value == "Negative")
                        {
                            negativeWords.Add(word);
                        }
                    }
                }
                j++;
            }

            positiveWords.Sort();
            negativeWords.Sort();


            Dictionary<string, int> nWordFreq = new Dictionary<string, int>();
            nWordFreq.Add("<UNK>", 0);
            Dictionary<string, int> pWordFreq = new Dictionary<string, int>();
            pWordFreq.Add("<UNK>", 0);

            foreach (string word in positiveWords)
            {
                if (!pWordFreq.ContainsKey(word))
                {
                    pWordFreq.Add(word, 0);
                }
                pWordFreq[word]++;
            }

            foreach (string word in negativeWords)
            {
                if (!nWordFreq.ContainsKey(word))
                {
                    nWordFreq.Add(word, 0);
                }
                nWordFreq[word]++;
            }

            foreach (string word in vocabList)
            {
                int pFreq;
                if (pWordFreq.ContainsKey(word))
                {
                    pFreq = pWordFreq[word];
                }
                else {
                    pFreq = 0;
                }

                int nFreq;
                if (nWordFreq.ContainsKey(word))
                {
                    nFreq = nWordFreq[word];
                }
                else
                {
                    nFreq = 0;
                }

                if (pFreq + nFreq < 5)
                {
                    if (pWordFreq.ContainsKey(word))
                    {
                        int pfreq = pWordFreq[word];
                        pWordFreq["<UNK>"] += pfreq;
                        pWordFreq.Remove(word);
                    }

                    if (nWordFreq.ContainsKey(word))
                    {
                        int nfreq = nWordFreq[word];
                        nWordFreq["<UNK>"] += nfreq;
                        nWordFreq.Remove(word);
                    }
                }
            }


            using (StreamWriter pfile = new StreamWriter("modelo_lenguaje_P.txt"))
            {
                List<string> wordList = new List<string>(pWordFreq.Keys);
                wordList.Sort();

                pfile.WriteLine("Numero de documentos (tweets) del corpus :" + positiveTweets);
                pfile.WriteLine("Número de palabras del corpus:" + positiveWords.Count);

                foreach (string word in wordList)
                {
                    double logProb = Math.Log((pWordFreq[word] + 1)) - Math.Log((double)positiveWords.Count + vocabList.Count);
                    pfile.WriteLine("Palabra:" + word + " Frec:" + pWordFreq[word] + " LogProb:" + logProb);
                }
            }


            using (StreamWriter nfile = new StreamWriter("modelo_lenguaje_N.txt"))
            {
                List<string> wordList = new List<string>(nWordFreq.Keys);
                wordList.Sort();

                nfile.WriteLine("Numero de documentos (tweets) del corpus :" + negativeTweets);
                nfile.WriteLine("Número de palabras del corpus:" + negativeWords.Count);

                foreach (string word in wordList)
                {
                    double logProb = Math.Log((nWordFreq[word] + 1)) - Math.Log((double)negativeWords.Count + vocabList.Count);
                    nfile.WriteLine("Palabra:" + word + " Frec:" + nWordFreq[word] + " LogProb:" + logProb);
                }
            }
        }

        public static void Model(int corpusTweets, string corpus, string vocuabulary, bool postive)
        {

            List<string> corpusList = new List<string>(File.ReadAllLines(corpus));
            List<string> vocabloList = new List<string>(File.ReadAllLines(vocuabulary));
            vocabloList.RemoveAt(0);

            HashSet<string> vocabloSet = new HashSet<string>(vocabloList);

            Dictionary<string, int> wordFreq = new Dictionary<string, int>();
            wordFreq.Add("<UNK>", 0);

            foreach (string word in corpusList)
            {
                if (!wordFreq.ContainsKey(word))
                {
                    wordFreq.Add(word, 0);
                }
                wordFreq[word]++;
            }

            List<string> wordList = new List<string>(wordFreq.Keys);

            foreach (string word in wordList)
            {
                if (wordFreq[word] < 2 && word != "<UNK>")
                {
                    int freq = wordFreq[word];
                    wordFreq["<UNK>"] += freq;
                    wordFreq.Remove(word);
                }
            }

            string outFilename;

            if (postive)
            {
                outFilename = "modelo_lenguaje_P.txt";
            }
            else
            {
                outFilename = "modelo_lenguaje_N.txt";
            }

            using (StreamWriter file = new StreamWriter(outFilename))
            {
                wordList = new List<string>(wordFreq.Keys);
                wordList.Sort();

                file.WriteLine("Numero de documentos (tweets) del corpus :" + corpusTweets);
                file.WriteLine("Número de palabras del corpus:" + corpusList.Count);

                foreach (string word in wordList)
                {
                    double logProb = Math.Log((wordFreq[word] + 1)) - Math.Log((double)corpusList.Count + vocabloSet.Count);
                    file.WriteLine("Palabra:" + word + " Frec:" + wordFreq[word] + " LogProb:" + logProb);
                }
            }
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
