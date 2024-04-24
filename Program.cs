using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchOption = Microsoft.VisualBasic.FileIO.SearchOption;

namespace Dictionary
{
    internal class Program
    {
        private static void Search(BinaryDictionaryTree tree, DictionaryNode dn)
        {
            Console.WriteLine("Какой термин вы бы хотели найти?");
            var term = Console.ReadLine();
            DictionaryNode answer = tree.Search(dn, term);
            
            var definitions = answer.term.First().Value;
            Console.WriteLine("Термин");
            Console.WriteLine(answer.term.First().Key);
            Console.WriteLine("Определения");
            foreach (var def in definitions)
            {
                Console.WriteLine(def);
            }
            Console.WriteLine();

        }
        static void Main(string[] args)
        {
            var binaryDictionaryTree = new BinaryDictionaryTree();

            FileManager fileterm = new("FileTerms.json");
            Dictionary<string, string> terms = fileterm.ReadFile();
            fileterm.CloseFile();
            FileManager filedef = new("FileDef.json");
            Dictionary<string, List<string>> definitions = filedef.ReadFileWithDefinitions();
            filedef.CloseFile();

            DictionaryNode dict = new DictionaryNode(new Dictionary<string, List<string>>()
            {
                {
                    terms.Values.First(), definitions[terms.Keys.First()]
                }
            });
            string keyFirst = terms.Keys.First();
            terms.Remove(keyFirst);
            definitions.Remove(keyFirst);
            foreach (var term in terms)
            {
                binaryDictionaryTree.Insert(dict, new Dictionary<string, List<string>>()
                {
                    {
                        term.Value, definitions[term.Key]
                        
                    }
                });
            }
            binaryDictionaryTree.PrintTree(dict);
            
            Search(binaryDictionaryTree, dict);
        }
    }
}
