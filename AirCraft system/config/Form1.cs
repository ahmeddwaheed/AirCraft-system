using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Project101
{
    public partial class Form1 : Form
    {
        public static Trie koko;

        public Dictionary<string, List<Tuple<long, string>>> prefixToSentences;
        
        public List<string> dictionary;

        private static List<Tuple<long, string>> curList;

        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load_1(object sender, EventArgs e)
        {
            prefixToSentences = new Dictionary<string, List<Tuple<long, string>>>();
            dictionary = new List<string>();
            AddAllPrefixes(@"C:\Users\Waheed\Downloads\Project101\Project101\Project101\Search Links (Small).txt");
            AddDicionatryToTrie(@"C:\Users\Waheed\Downloads\Project101\Project101\Project101\Dictionary\dictionary.txt");
        }

       public class Trie
        {
            private TrieNode head;

            public Trie()
            {
                head = new TrieNode();
            }
            public void AddWord(string word)
            {
                TrieNode curr = head;
                for (int i = 0; i < word.Length; i++)
                {
                    curr = curr.GetChild(word[i], true);
                }
                curr.AddCount();
            }
            public int GetCount(string word)
            {
                TrieNode curr = head;
                foreach (char c in word)
                {
                    curr = curr.GetChild(c);
                    if (curr == null)
                    {
                        return 0;
                    }
                }
                return curr.count;
            }
            public string[] GetAllSuggestions(string word)
            {
                TrieNode cur = head;
                string[] ret = { "", "" };
                int i = 0;
                foreach (char c in word)
                {
                    cur = cur.GetChild(c,false);
                    if (cur == null)
                    {
                        break;
                    }
                    i++;
                }
                return ret;
            }
            internal class TrieNode
            {
                private LinkedList<TrieNode> children;
                public int count { private set; get; }
                public char data { private set; get; }
                public TrieNode(char data = ' ')
                {
                    this.data = data;
                    count = 0;
                    children = new LinkedList<TrieNode>();
                }
                public TrieNode GetChild(char c, bool check = false)
                {
                    foreach (var child in children)
                    {
                        if (child.data == c)
                        {
                            return child;
                        }
                    }
                    if (check)
                    {
                        return CreateChild(c);
                    }

                    return null;
                }
                public void AddCount()
                {
                    count++;
                }

                public TrieNode CreateChild(char c)
                {
                    var child = new TrieNode(c);
                    children.AddLast(child);
                    return child;
                }
            }
        } //length of word

        public void AddAllPrefixes(string file)
        {
            var lines = File.ReadLines(file);
            int lineNumber = 0;
            foreach (var line in lines)
            {
                if (lineNumber == 0)
                {
                    lineNumber++;
                    continue;
                }
                string[] costAndSentence = line.Split(',');
                long cost = long.Parse(costAndSentence[0]);
                string sentenceBeforeParsing = costAndSentence[1];
                string[] sentenceAfterParsing = sentenceBeforeParsing.Split(' ');
                Tuple<long, string> curTuple = new Tuple<long, string>(cost, sentenceBeforeParsing);
                for (int i = 0; i < sentenceAfterParsing.Length; i++)
                {
                    string curSentence = "";
                    for (int j = i; j < sentenceAfterParsing.Length; j++)
                    {
                        string curPrefix = "";
                        for(int k = 0; k<sentenceAfterParsing[j].Length; k++)
                        {
                            curPrefix += sentenceAfterParsing[j][k];
                            if (prefixToSentences.ContainsKey(curPrefix.ToLower()) != true)
                            {
                                prefixToSentences[curPrefix.ToLower()] = new List<Tuple<long, string>>();
                            }
                            if(prefixToSentences[curPrefix.ToLower()].Contains(curTuple)!=true)
                            {
                                prefixToSentences[curPrefix.ToLower()].Add(curTuple);
                            }
                        }

                    }
                    if(curSentence.Length!=0)
                    {
                        curSentence += " ";
                    }
                    if(prefixToSentences.ContainsKey(curSentence.ToLower())!=true)
                    {
                        prefixToSentences[curSentence.ToLower()] = new List<Tuple<long, string>>();
                    }
                    if(prefixToSentences[curSentence.ToLower()].Contains(curTuple)!=true)
                    {
                        prefixToSentences[curSentence.ToLower()].Add(curTuple);
                    }
                }
            }
        } //n^3

        public void AddDicionatryToTrie(string file)
        {
            koko = new Trie();
            var lines = File.ReadAllLines(file);
            {
                foreach (string line in lines)
                {
                    dictionary.Add(line);
                    koko.AddWord(line);
                }
            }
        }   //number of words * length of each word

        public void BubbleSort()
        {
            foreach (var curDictionaryElement in prefixToSentences)
            {
                for (int i = 0; i < curDictionaryElement.Value.Count; i++)
                {
                    for (int j = 0; j < curDictionaryElement.Value.Count - 1; j++)
                    {
                        if (curDictionaryElement.Value[j].Item1 < curDictionaryElement.Value[j + 1].Item1)
                        {
                            Tuple<long, string> tmp = curDictionaryElement.Value[j];
                            curDictionaryElement.Value[j] = curDictionaryElement.Value[j + 1];
                            curDictionaryElement.Value[j + 1] = tmp;
                        }
                    }
                }
            }
        } //n^2

        public void SelectionSort()
        {
            foreach (var curDictionaryElement in prefixToSentences)
            {
                for (int i = 0; i < curDictionaryElement.Value.Count; i++)
                {
                    for (int j = i + 1; j < curDictionaryElement.Value.Count; j++)
                    {
                        if (curDictionaryElement.Value[i].Item1 < curDictionaryElement.Value[j].Item1)
                        {
                            Tuple<long, string> tmp = curDictionaryElement.Value[i];
                            curDictionaryElement.Value[i] = curDictionaryElement.Value[j];
                            curDictionaryElement.Value[j] = tmp;
                        }
                    }
                }
            }
        } //n^2 - n

        public void MergeSort()
        {
            foreach (var curDictionaryElement in prefixToSentences)
            {
                curList = curDictionaryElement.Value;
                Merge(0, curDictionaryElement.Value.Count - 1);
            }
        } // n log(n)

        private void Merge(int l, int r)
        {
            if (l == r)
            {
                return;
            }
            List<Tuple<long, string>> tmp = new List<Tuple<long, string>>();
            int m = (l + r) / 2;
            Merge(l, m);
            Merge(m + 1, r);
            int i = l, j = m + 1;
            while (i <= m && j <= r)
            {
                if (curList[i].Item1 >= curList[j].Item1)
                {
                    tmp.Add(curList[i]);
                    i++;
                }
                else
                {
                    tmp.Add(curList[j]);
                    j++;
                }
            }
            while (i <= m)
            {
                tmp.Add(curList[i++]);
            }
            while (j <= r)
            {
                tmp.Add(curList[j++]);
            }
            for (int k = 0; k + l <= r; k++)
            {
                curList[k + l] = tmp[k];
            }
        }
        private int editdistance(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++)
            {
                for (int j = 0; j <= b.Length; j++)
                {
                    if (i == 0)
                    {
                        dp[i, j] = j;
                    }
                    else if (j == 0)
                    {
                        dp[i, j] = i;
                    }
                    else if (a[i - 1] == b[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1];
                    }
                    else
                    {
                        dp[i, j] = 1 + Math.Min(dp[i, j - 1], Math.Min(dp[i - 1, j], dp[i - 1, j - 1]));
                    }
                }
            }
            return dp[a.Length, b.Length];
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            listBox1.Hide();
            string text = textBox1.Text.ToString().ToLower();
            if (prefixToSentences.ContainsKey(text) == true && text.Length != 0)
            {
                listBox1.Show();
                listBox1.Items.Clear();
                List<Tuple<long, string>> suggestions = prefixToSentences[text];
                string[] tmp = new string[suggestions.Count];
                for (int i = 0; i < suggestions.Count; i++)
                {
                    tmp[i] = (suggestions[i].Item2);
                }
                listBox1.Items.AddRange(tmp);
            }
            else
            {
                string[] tmp = text.Split(' ');
                string[] err = new string[tmp.Length];
                int c = 0;
                string sss=textBox1.Text.ToString();
                if (sss[sss.Length-1] == ' ')
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        listBox1.Items.Clear();
                        if (koko.GetCount(tmp[i]) == 0 && tmp[i].Length > 0)
                        {
                            err[c++] = "Word '" + tmp[i] + "' is incorrectly spelt";
                            int cur = int.MaxValue;
                            string ans = "";
                            for (int j = 0; j < dictionary.Count; j++)
                            {
                                if (cur > editdistance(tmp[i], dictionary[j]))
                                {
                                    ans = dictionary[j];
                                    cur = editdistance(tmp[i], dictionary[j]);
                                }
                            }
                            DialogResult dialog = MessageBox.Show("Did you mean " + ans + '?', "Query", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                textBox1.Text = ans;
                            }
                        }
                    }
                    if (c > 0 && text.Length > 0)
                    {
                        listBox1.Show();
                        for (int i = 0; i < c; i++)
                        {
                            listBox1.Items.Add(err[i]);
                        }
                    }
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MergeSort();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            SelectionSort();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            BubbleSort();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
