using  System;
using System.IO;
using System.Collections.Generic;
using JackCompiler;


namespace JackCompiler
{
    class MainRun
    {
        static void Main(string[] args)
        {
            JackSyntaxAnalyze("StringTest");
        }
        public static void JackSyntaxAnalyze(string fileOrFolderShort)
        {
            //string folderSearching = @"C:\Users\Carte\Source\Repos\Nand2TetrisCourse\Compiler\JackCompiler\";
            string folderSearching = @"C:\Users\Carte\Source\Repos\Nand2TetrisCourse\JackOs\";
            string fullNameOfFileOrFolder = folderSearching + fileOrFolderShort;
            Tokenizer tokenizerUsing = new Tokenizer();
            string extension = Path.GetExtension(fileOrFolderShort);
            List<Token> tokenList = null;
            if (extension == ".jack")
            {
                tokenList = tokenizerUsing.TokenizeFile(fullNameOfFileOrFolder);
                Parser currentParser = new Parser();
                currentParser.ParseFile(tokenList, fullNameOfFileOrFolder);
            }
            else
            {
                foreach (string current in Directory.GetFiles(fullNameOfFileOrFolder))
                {
                    if (Path.GetExtension(current) == ".jack")
                    {
                        tokenList = tokenizerUsing.TokenizeFile(current);
                        Parser currentParser = new Parser();
                        currentParser.ParseFile(tokenList, current);
                    }
                }
            }
             //if(tokenList!=null)
             //{
             //    foreach (Token current in tokenList)
             //    {
             //        Console.WriteLine(current.tokenType + ": " + current.tokenValue);
             //    }
             //}
             //else
             //{
             //    Console.WriteLine("didn't find file "+ fullNameOfFileOrFolder);
             //}
        }
        public static bool CheckIsNumeric(string str)
        {
            int n;
            return int.TryParse(str, out n);
        }
    }

}
