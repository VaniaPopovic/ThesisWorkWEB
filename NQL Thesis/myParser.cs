using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using com.sun.corba.se.impl.orb;
using ikvm.extensions;
using java.io;
using java.net;
using javax.swing;
using opennlp.tools.chunker;
using opennlp.tools.cmdline.parser;
using opennlp.tools.namefind;
using opennlp.tools.parser;
using opennlp.tools.postag;
using opennlp.tools.tokenize;
using opennlp.tools.util;
using Syn.WordNet;

using File = System.IO.File;
using IOException = java.io.IOException;
using StringReader = java.io.StringReader;
using Tokenizer = opennlp.tools.tokenize.Tokenizer;




namespace NQL_Thesis
{
    public class myParser
    {

        //  WordNetEngine 
        static HashSet<string> nounPhrases = new HashSet<string>();
        static HashSet<string> nouns = new HashSet<string>();
        static HashSet<string> adjectivePhrases = new HashSet<string>();
        static HashSet<string> verbPhrases = new HashSet<string>();
        static HashSet<string> dates;
        private static Dictionary<string, List<string>> matchedWords = new Dictionary<string, List<string>>();
      
        //      private static String line = "Display the sales of coca cola 5ltr";
        private string[] stopWordsofwordnet = { "from", "to", "me", "the", "of", "a", "a ", "as",".","between","and" };

        private static void FindMatchingNodesFromXml(XmlNode node, HashSet<string> set)
        {
            if (node.NodeType == XmlNodeType.Text)
            {
                foreach (var variable in set)
                {
                    if ((variable.ToLower().Trim()).Equals(node.Value.ToLower().Trim()))
                    {
                        string a = node.ParentNode.ParentNode.FirstChild.FirstChild.Value;
                        if (!matchedWords.ContainsKey(variable))
                        {
                            var newList = new List<string>();
                            newList.Add(a);
                            matchedWords.Add(variable,newList);
                        }
                       matchedWords[variable].Add(a);
                    }


                }
            }




            //Print individual children of the node, gets only direct children of the node
            XmlNodeList children = node.ChildNodes;
            foreach (XmlNode child in children)
            {
                FindMatchingNodesFromXml(child, set);
            }
        }


        public void PrintSets()
        {

            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.Write("\nList of Noun Phrases : [");
            foreach (var b in nounPhrases)
            {
                System.Diagnostics.Debug.Write(b);
                System.Diagnostics.Debug.Write(",");
            }
            System.Diagnostics.Debug.Write("]");

            System.Diagnostics.Debug.Write("\nList of Noun Parse [:");
            foreach (var a in nouns)
            {
                System.Diagnostics.Debug.Write(a);
                System.Diagnostics.Debug.Write(",");
            }
            System.Diagnostics.Debug.Write("]");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.Write("List of Adjective Parse : [");
            foreach (var c in adjectivePhrases)
            {
                System.Diagnostics.Debug.Write(c);
                System.Diagnostics.Debug.Write(",");
            }
            System.Diagnostics.Debug.Write("]");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.Write("List of Verb Parse : [");
            foreach (var h in verbPhrases)
            {
                System.Diagnostics.Debug.Write(h);
                System.Diagnostics.Debug.Write(",");
            }

            System.Diagnostics.Debug.Write("]");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.Write("List of date Parse : [");
            foreach (var z in dates)
            {
                System.Diagnostics.Debug.Write(z);
                System.Diagnostics.Debug.Write(",");
            }
        }

        public void TruncateLists(ref HashSet<string> set)
        {
            var newList = new List<string>();
            var tempList = new List<string>();
            foreach (var str in set)
            {
                foreach (var word in matchedWords)
                {
                    var s = str.toLowerCase();
                    var a = word.Key.toLowerCase().Trim();
                    if (s.contains(a))
                    {

                        newList = tempList.Select(x => x.Replace(a, string.Empty)).ToList();


                    }

                    tempList = newList;
                }
            }
            set = new HashSet<string>(tempList);
        }
        public void GetSentenceParts(Parse p)
        {
            if (p.getType().Equals("NN") || p.getType().Equals("NNS") || p.getType().Equals("NNP") ||
                p.getType().Equals("NNPS"))
            {
                nouns.Add(p.getCoveredText());
            }

            if (p.getType().Equals("JJ") || p.getType().Equals("JJR") || p.getType().Equals("JJS"))
            {
                adjectivePhrases.Add(p.getCoveredText());
            }

            if (p.getType().Equals("VB") || p.getType().Equals("VBP") || p.getType().Equals("VBG") ||
                p.getType().Equals("VBD") || p.getType().Equals("VBN"))
            {
                verbPhrases.Add(p.getCoveredText());
            }

            if (p.getType().Equals("NP"))
            {
                //NP=noun phrase
                nounPhrases.Add(p.getCoveredText());
            }

            foreach (Parse child in p.getChildren())
            {
                GetSentenceParts(child);
            }
        }
        public Dictionary<string,List<string>> Main(string line)
        {
            //debug sentence
           // line = "Show me the sales of Kean Cola .25ltr Bottle in Nicosia from January 2017 to October 2017 as a line chart.";
            matchedWords?.Clear();
            nounPhrases?.Clear();
            nouns?.Clear();
            adjectivePhrases?.Clear();
            verbPhrases?.Clear();
            InputStream modelIn = new FileInputStream(HttpRuntime.AppDomainAppPath + "\\Models\\en-parser-chunking.bin");

            InputStream modelIn1 = new FileInputStream(HttpRuntime.AppDomainAppPath + "\\Models\\en-ner-date.bin");
            InputStream modelIn2 = new FileInputStream(HttpRuntime.AppDomainAppPath + "\\Models\\en-token.bin");
            ParserModel model = new ParserModel(modelIn);
            var myParser = ParserFactory.create(model);
            var topParses = ParserTool.parseLine(line, myParser, 1);
            foreach (var p in topParses)
            {
                GetSentenceParts(p);
            }


            try
            {
                TokenizerModel model1 = new TokenizerModel(modelIn2);
                TokenNameFinderModel model2 = new TokenNameFinderModel(modelIn1);

                Tokenizer tokenizer = new TokenizerME(model1);
                var nameFinder = new NameFinderME(model2);

                var tokens = tokenizer.tokenize(line);
                var nameSpans = nameFinder.find(tokens);

                var array = Span.spansToStrings(nameSpans, tokens);

                //
                //                foreach (var v in array)
                //                {
                //                    System.Diagnostics.Debug.WriteLine(v);
                //                }

                dates = new HashSet<string>(array);



                PrintSets();
//                System.Diagnostics.Debug.WriteLine("\nProcessing Presentation type");
//
//                if (nouns.Contains("table"))
//                {
//                    matchedWords.Add(new Tuple<string, string>("PRESENTATION_TYPE", "table"));
//                }
//                if (nounPhrases.Contains("bar chart"))
//                {
//                    matchedWords.Add(new Tuple<string, string>("PRESENTATION_TYPE", "bar chart"));
//                }
//                if (nounPhrases.Contains("line chart"))
//                {
//                    matchedWords.Add(new Tuple<string, string>("PRESENTATION_TYPE", "line chart"));
//                }
                //TODO IF NO OPTION IS FOUND ASK THE USER TO GIVE YOU ONE. IMPLEMENT IT IN THE WEB VERSION SOON

                System.Diagnostics.Debug.WriteLine("\nProcessing Dates");

                if (dates.Count == 2)
                {
                    if (dates.ElementAt(0).contains("from"))
                    {
                        var a = dates.ElementAt(0).replace("from", "");
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
                        matchedWords.Add(a,newList);
                         newList = new List<string>();
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dates.ElementAt(1),newList);

                    }
                    else
                    {
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
                        matchedWords.Add(dates.ElementAt(0), newList);
                        newList = new List<string>();
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dates.ElementAt(1), newList);
                    }

                }

                if (dates.Count == 1)
                {

                    if (dates.ElementAt(0).contains("from"))
                    {
                        var a = dates.ElementAt(0).replace("from", "");
                        var dts = a.Split(new[] { " to " }, StringSplitOptions.None);
                       
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
                        matchedWords.Add(dts[0], newList);
                        newList = new List<string>();
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dts[1], newList);
                    }
                    else
                    {
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
              
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dates.ElementAt(0), newList);

                    }

                }

                System.Diagnostics.Debug.WriteLine("\nProcessing noun phrases");

                //                var manager = new Manager();
                //                var serializer = new XmlSerializer(typeof(Manager.language));
                //                var loadStream = new FileStream("file2.xml", FileMode.Open, FileAccess.Read);
                //                var loadedObject = (Manager.language) serializer.Deserialize(loadStream);


                var doc = new XmlDocument();
                System.Diagnostics.Debug.WriteLine(HttpRuntime.AppDomainAppPath);
                System.Diagnostics.Debug.WriteLine(HttpRuntime.AppDomainAppPath);
                System.Diagnostics.Debug.WriteLine(HttpRuntime.AppDomainAppPath);
                System.Diagnostics.Debug.WriteLine(HttpRuntime.AppDomainAppPath);
                doc.Load(HttpRuntime.AppDomainAppPath+"\\file2.xml");
                var root = doc.SelectSingleNode("*");
                FindMatchingNodesFromXml(root, nounPhrases);
                FindMatchingNodesFromXml(root, verbPhrases);
                FindMatchingNodesFromXml(root, nouns);




                System.Diagnostics.Debug.WriteLine("\nProcessing verb phrases ");


                System.Diagnostics.Debug.WriteLine("\nProcessing nouns ");




                // construct the dictionary object and open it
                var directory = Directory.GetCurrentDirectory() + "\\wordnet\\";
                foreach (var variable in matchedWords)
                {
                    System.Diagnostics.Debug.WriteLine(variable.Value + "\t\t" + variable.Key);
                }

                foreach (var variable in matchedWords)
                {
                    string a = variable.Key;
                    if (line.Contains(a)) line = line.replace(a, "");
                }

                foreach (var variable in stopWordsofwordnet)
                {
                    string a = " " + variable.toLowerCase() + " ";
                    if (line.Contains(a)) line = line.replace(a, " ");
                }
                if(line.contains(".")) line=line.replace(".","");
                if (line.contains("-")) line = line.replace("-", " ");
                System.Diagnostics.Debug.WriteLine("/////////////");
                System.Diagnostics.Debug.WriteLine("SECOND PARSE STRING " + line);
                System.Diagnostics.Debug.WriteLine("/////////////");
                line = line.Trim();
                topParses = ParserTool.parseLine(line, myParser, 1);
                nounPhrases?.Clear();
                dates?.Clear();
                verbPhrases?.Clear();
                nouns?.Clear();
                foreach (var p in topParses)
                {
                    //p.show();
                    GetSentenceParts(p);

                }

                FindMatchingNodesFromXml(root, nounPhrases);
                FindMatchingNodesFromXml(root, verbPhrases);
                FindMatchingNodesFromXml(root, nouns);


                tokens = tokenizer.tokenize(line);
                nameSpans = nameFinder.find(tokens);

                array = Span.spansToStrings(nameSpans, tokens);
                dates = new HashSet<string>(array);


                
                PrintSets();

                System.Diagnostics.Debug.WriteLine("\nProcessing Dates");


                if (dates.Count == 2)
                {
                    if (dates.ElementAt(0).contains("from"))
                    {
                        var a = dates.ElementAt(0).replace("from", "");
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
                        matchedWords.Add(a, newList);
                        newList = new List<string>();
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dates.ElementAt(1), newList);

                    }
                    else
                    {
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
                        matchedWords.Add(dates.ElementAt(0), newList);
                        newList = new List<string>();
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dates.ElementAt(1), newList);
                    }

                }

                if (dates.Count == 1)
                {

                    if (dates.ElementAt(0).contains("from"))
                    {
                        var a = dates.ElementAt(0).replace("from", "");
                        var dts = a.Split(new[] { " to " }, StringSplitOptions.None);

                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");
                        matchedWords.Add(dts[0], newList);
                        newList = new List<string>();
                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dts[1], newList);
                    }
                    else
                    {
                        List<string> newList = new List<string>();
                        newList.Add("START_PERIOD");

                        newList.Add("END_PERIOD");
                        //todo fix when the date is the same here
                        matchedWords.Add(dates.ElementAt(0), newList);

                    }

                }

                System.Diagnostics.Debug.WriteLine("\nProcessing noun phrases");

                //                var manager = new Manager();
                //                var serializer = new XmlSerializer(typeof(Manager.language));
                //                var loadStream = new FileStream("file2.xml", FileMode.Open, FileAccess.Read);
                //                var loadedObject = (Manager.language) serializer.Deserialize(loadStream);



                FindMatchingNodesFromXml(root, nounPhrases);
                FindMatchingNodesFromXml(root, verbPhrases);
                FindMatchingNodesFromXml(root, nouns);

                foreach (var variable in matchedWords)
                {
                    System.Diagnostics.Debug.WriteLine(variable.Value + "\t\t" + variable.Key);
                }


                //MATCHING WITH WORD NET
                System.Diagnostics.Debug.WriteLine(directory);
                //                var wordNet = new WordNetEngine();
                //
                //                wordNet.AddDataSource(new StreamReader(Path.Combine(directory, "data.adj")), PartOfSpeech.Adjective);
                //                wordNet.AddDataSource(new StreamReader(Path.Combine(directory, "data.adv")), PartOfSpeech.Adverb);
                //                wordNet.AddDataSource(new StreamReader(Path.Combine(directory, "data.noun")), PartOfSpeech.Noun);
                //                wordNet.AddDataSource(new StreamReader(Path.Combine(directory, "data.verb")), PartOfSpeech.Verb);
                //
                //                wordNet.AddIndexSource(new StreamReader(Path.Combine(directory, "index.adj")), PartOfSpeech.Adjective);
                //                wordNet.AddIndexSource(new StreamReader(Path.Combine(directory, "index.adv")), PartOfSpeech.Adverb);
                //                wordNet.AddIndexSource(new StreamReader(Path.Combine(directory, "index.noun")), PartOfSpeech.Noun);
                //                wordNet.AddIndexSource(new StreamReader(Path.Combine(directory, "index.verb")), PartOfSpeech.Verb);
                //
                //                System.Diagnostics.Debug.WriteLine("Loading database...");
                //                wordNet.Load();
                //                System.Diagnostics.Debug.WriteLine("Load completed.");
                //                while (true)
                //                {
                //                    System.Diagnostics.Debug.WriteLine("\nType first word");
                //
                //                    var word = System.Diagnostics.Debug.ReadLine();
                //                    var synSetList = wordNet.GetSynSets(word);
                //
                //                    if (synSetList.Count == 0) System.Diagnostics.Debug.WriteLine($"No SynSet found for '{word}'");
                //
                //                    foreach (var synSet in synSetList)
                //                    {
                //                        var words = string.Join(", ", synSet.Words);
                //
                //                        System.Diagnostics.Debug.WriteLine($"\nWords: {words}");
                //                    }
                //                }



            }
            catch (IOException e)
            {
                e.printStackTrace();
            }
            finally
            {
                if (modelIn1 != null)
                {
                    try
                    {
                        modelIn1.close();
                    }
                    catch (IOException e)
                    {
                    }
                }

                if (modelIn2 != null)
                {
                    try
                    {
                        modelIn2.close();
                    }
                    catch (IOException e)
                    {
                    }
                }






                //            truncateLists(ref nounPhrases);
                //            truncateLists(ref nouns);
                //            truncateLists(ref dates);
                //            truncateLists(ref verbPhrases);


              

            }

            

            return matchedWords;
        }
    
}
}