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
            JackSyntaxAnalyze("MathTest");
            int x = 18000;
            int y = 6;
            int XTimeY = x / y;
            Console.WriteLine(dividePositive(x, y) + "," + (XTimeY));
            x = 48;
            y = 3;
            XTimeY = x / y;
            Console.WriteLine(dividePositive(x, y) + "," + (XTimeY));
            x = 99;
            y = 33;
            XTimeY = x / y;
            Console.WriteLine(dividePositive(x, y) + "," + (XTimeY));
            x = 4521;
            y = 30;
            XTimeY = x / y;
            Console.WriteLine(dividePositive(x, y) + "," + (XTimeY));
        }
        public static void JackSyntaxAnalyze(string fileOrFolderShort)
        {
            //string folderSearching = @"C:\Users\Carte\Source\Repos\Nand2TetrisCourse\Compiler\JackCompiler\";
            string folderSearching = @"C:\Users\Carte\Source\Repos\Nand2TetrisCourse\JackOs\";
            string fullNameOfFileOrFolder = folderSearching + fileOrFolderShort;
            Tokenizer tokenizerUsing = new Tokenizer();
            string extension =Path.GetExtension(fileOrFolderShort);
            List<Token> tokenList = null;
            if (extension == ".jack")
            {
                tokenList =tokenizerUsing.TokenizeFile(fullNameOfFileOrFolder);
                Parser currentParser = new Parser();
                currentParser.ParseFile(tokenList,fullNameOfFileOrFolder);
            }
            else
            {
                foreach (string current in Directory.GetFiles(fullNameOfFileOrFolder))
                {
                    if(Path.GetExtension(current)  == ".jack")
                    {
                        tokenList=tokenizerUsing.TokenizeFile(current);
                        Parser currentParser = new Parser();
                        currentParser.ParseFile(tokenList,current);
                    }
                }
            }
            // if(tokenList!=null)
            // {
            //     foreach (Token current in tokenList)
            //     {
            //         Console.WriteLine(current.tokenType + ": " + current.tokenValue);
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("didn't find file "+ fullNameOfFileOrFolder);
            // }
        }
        public static bool CheckIsNumeric(string str)
        {
            int n;
            return int.TryParse(str, out n);
        }

        static int multiply(int x, int y)
        {
            int currentBit;
            int result =0;
            int twoX;
        
             twoX = x + x;
             currentBit = 1;
            while (currentBit < twoX)
            {
                int andOp = x&currentBit;
                //check if currentbit is 1;
                if (andOp != 0)
                {
                    //add y shifted
                     result = result + y;
                }
                //shift y
                 y = y + y;
                 currentBit = currentBit + currentBit;
            }
           
            return result;
        }
        public static int dividePositive(int x, int y)
        {
            int result = 0;
            int mult = 0;
            if (x < y)
            {
                return 0;
            }
            result = dividePositive(x, y + y);
            mult = multiply(result, y);
            if ((x - mult - mult) < y)
            {
                return result + result;
            }
            return result + result + 1;
        }
    }
    
}
