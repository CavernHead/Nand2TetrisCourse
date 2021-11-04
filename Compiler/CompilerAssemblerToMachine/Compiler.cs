using System;
using System.IO;
using System.Collections.Generic;

namespace CompilerAssemblerToMachine
{
    class Compiler
    {
        string[,] possibleComputation = new string[18,3] { {"0", "0","101010" }, { "1", "1", "111111" }, { "-1", "-1", "111010" }, { "D", "D", "001100" }, { "A", "A", "110000" }, { "!D", "!D", "001101" }, { "!A", "!A", "110001" }, { "-D", "-D", "001111" }, { "-A", "-A", "110011" }, { "D+1", "1+D", "011111" }, { "A+1", "1+A", "110111" }, { "D-1", "-1+D", "001110" },{ "A-1", "-1+A", "110010" }, { "D+A", "A+D", "000010" }, { "D-A", "-A+D", "010011" }, { "A-D", "-D+A", "000111" }, { "D&A", "A&D", "000000" }, { "D|A", "A|D", "010101" } };
        string[,] possibleJumps = new string[8, 2] { { "null", "000" }, { "JGT", "001" }, { "JEQ", "010" }, { "JGE", "011" }, { "JLT", "100" }, { "JNE", "101" }, { "JLE", "110" }, { "JMP", "111" } };
        static void Main(string[] args)
        {
            Compiler compiler = new Compiler();
            Console.WriteLine("compiling FinalProg");
            compiler.TranslateAssemblerToMachine("FinalProg");
            // compiler.TranslateAssemblerToMachine("Max");
          // compiler.TranslateAssemblerToMachine("MaxL");
          // compiler.TranslateAssemblerToMachine("Mult"); 
          // compiler.TranslateAssemblerToMachine("Pong");
          // compiler.TranslateAssemblerToMachine("PongL");
          // compiler.TranslateAssemblerToMachine("Rect");
          // compiler.TranslateAssemblerToMachine("RectL");
        }

        void TranslateAssemblerToMachine(string FileName)
        {
            TranslateAssemblerToMachine(FileName+".asm", FileName+".hack");
        }
        void TranslateAssemblerToMachine(string assemberFileName, string machineCodeFileName)
        {
            string parentFileName= @"C:\Users\Carte\Desktop\nand2tetris\nand2tetris\projects\12\";
            TranslateAssemblerToMachine(parentFileName, assemberFileName, machineCodeFileName);
        }
        void TranslateAssemblerToMachine(string parentFileName, string assemberFileName, string machineCodeFileName)
        {
            Console.WriteLine("possibleComputation.Length: " + possibleComputation.GetLength(0));
            for(int i = 0; i<possibleComputation.GetLength(0);i++ )
            {
                Console.WriteLine(possibleComputation[i, 0] + "," + possibleComputation[i,1]+ ","+ possibleComputation[i, 2]);
            }
            //Console.WriteLine("possibleComputation[0].Length: " + possibleComputation[0].Length);
            string fullFileName = parentFileName + assemberFileName;
            if (File.Exists(fullFileName))
            {
                Console.WriteLine("fileExists");
                List<string> instructions = new List<string>();
                using (StreamReader sr = File.OpenText(fullFileName))
                {
                    string s = "";

                    //split instructions
                    while ((s = sr.ReadLine()) != null)
                    {
                        bool foundCommandInLine = false;
                        for (int i = 0; i < s.Length; i++)
                        {
                            if (s[i] == '/' && (i + 1) < s.Length && s[i + 1] == '/')
                            {
                                break;
                            }
                            else
                            {
                                if (s[i] != ' ')
                                {
                                    if (!foundCommandInLine)
                                    {
                                        
                                        foundCommandInLine = true;
                                        instructions.Add("");
                                    }
                                    instructions[instructions.Count - 1] += s[i];

                                }
                            }

                        }
                    }
                    

                    List<Symbol> allSymbols = new List<Symbol>();
                    //default symbols
                    for(int i =0;i < 16;i++)
                    {
                        allSymbols.Add(new Symbol("R" + i, i));
                    }
                    allSymbols.Add(new Symbol("SCREEN", 16384));
                    allSymbols.Add(new Symbol("KBD", 24576));

                    allSymbols.Add(new Symbol("SP", 0));
                    allSymbols.Add(new Symbol("LCL", 1));
                    allSymbols.Add(new Symbol("ARG", 2));
                    allSymbols.Add(new Symbol("THIS", 3));
                    allSymbols.Add(new Symbol("THAT", 4));

                    //find location symbols
                    int currentIntructionNumber = 0;
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        
                        string currentInstruction = instructions[i];
                        Console.WriteLine("instruction: " + currentInstruction);
                        if (currentInstruction[0] == '('&& 3<currentInstruction.Length)
                        {
                            
                            instructions.RemoveAt(i);
                            i--;
                            string nameFound =currentInstruction.Substring(1, currentInstruction.Length-2);
                            if (!CheckSymbolExists(nameFound))
                            {
                                allSymbols.Add(new Symbol(nameFound, currentIntructionNumber));
                            }
                        }
                        else
                        {
                            currentIntructionNumber++;
                        }
                    }

                    //find 'a' instruction symbols
                    int idiGiver = 15;
                    for (int i = 0; i < instructions.Count; i++)
                    {
                        string currentInstruction = instructions[i];
                        if (currentInstruction[0] == '@')
                        {
                            currentInstruction=currentInstruction.Substring(1, currentInstruction.Length - 1);

                            if (!CheckIsNumeric(currentInstruction) && !CheckSymbolExists(currentInstruction))
                            {
                                idiGiver++;
                                allSymbols.Add(new Symbol(currentInstruction, idiGiver));
                            }
                        }
                    }


                    //print out all symbols
                    for (int i = 0; i < allSymbols.Count; i++)
                    {
                        Console.WriteLine("symbole: name:" + allSymbols[i].name + " idi:" + allSymbols[i].idi);
                    }




                    using (StreamWriter sw = File.CreateText(parentFileName + machineCodeFileName))
                    {
                        //decrypt all instructions in to machine binary
                        for (int i = 0; i < instructions.Count; i++)
                        {
                            string currentInstruction = instructions[i];
                            //a instruction
                            if (currentInstruction[0] == '@')
                            {
                                string symbolName = currentInstruction.Substring(1, currentInstruction.Length - 1);
                                string binary = "";
                                int numericValue;
                                bool isNumNumeric = int.TryParse(symbolName, out numericValue);
                                if (!isNumNumeric)
                                {
                                    binary = Get16DigitBinary(GetSymbolIdi(symbolName)); ;
                                }
                                else
                                {

                                    binary = Get16DigitBinary(numericValue);
                                }
                                sw.WriteLine(binary);
                                Console.WriteLine("a; name:" + currentInstruction + " binary:" + binary);
                            }
                            else
                            {
                                //c instruction
                                string destinationInstruction="null";
                                string computationInstruction = "null";
                                string jumpInstruction = "null";
                                string currentInstructionPart ="";
                                bool jumpInstructionBool =false;
                                for(int j = 0; j< currentInstruction.Length;j++)
                                {
                                    if(currentInstruction[j]=='=')
                                    {
                                        destinationInstruction = currentInstructionPart;
                                        currentInstructionPart = "";
                                    }
                                    else
                                    if(currentInstruction.Length <= j + 1)
                                    {
                                        currentInstructionPart += currentInstruction[j];
                                        if (jumpInstructionBool)
                                        {
                                            jumpInstruction = currentInstructionPart;
                                        }
                                        else
                                        {
                                            computationInstruction = currentInstructionPart;
                                        }
                                        currentInstructionPart = "";
                                    }
                                    else
                                    if(currentInstruction[j] == ';')
                                    {
                                        jumpInstructionBool = true;
                                        computationInstruction = currentInstructionPart;
                                        currentInstructionPart = "";
                                    }
                                    else
                                    {
                                        currentInstructionPart += currentInstruction[j];
                                    }
                                    
                                }

                                string binary ="111";
                               
                                //computation
                                for (int j = 0; j < possibleComputation.GetLength(0); j++)
                                {
                                    
                                    if (computationInstruction == possibleComputation[j, 0] || computationInstruction == possibleComputation[j, 1])
                                    {
                                        binary+= "0"+ possibleComputation[j, 2];
                                        break;
                                    }
                                    else
                                    {
                                        string versionWithM1 = possibleComputation[j, 0];
                                        string versionWithM2 = possibleComputation[j, 1];
                                        versionWithM1 = versionWithM1.Replace("A", "M");
                                        versionWithM2 = versionWithM2.Replace("A", "M");
                                        //Console.WriteLine(versionWithM1);
                                        //Console.WriteLine(versionWithM2);


                                        if (computationInstruction == versionWithM1 || computationInstruction == versionWithM2)
                                        {
                                            binary += "1"+ possibleComputation[j, 2];
                                            break;
                                        }
                                       
                                    }
                                }
                                
                                //destination
                                if (0<=destinationInstruction.IndexOf("A"))
                                {
                                    binary += "1";
                                }
                                else
                                {
                                    binary += "0";
                                }
                                if (0 <= destinationInstruction.IndexOf("D"))
                                {
                                    binary += "1";
                                }
                                else
                                {
                                    binary += "0";
                                }
                                if (0 <= destinationInstruction.IndexOf("M"))
                                {
                                    binary += "1";
                                }
                                else
                                {
                                    binary += "0";
                                }

                                bool jumpCondition = false;
                                for (int j = 0; j < possibleJumps.GetLength(0); j++)
                                {
                                    if(possibleJumps[j,0]==jumpInstruction)
                                    {
                                        binary += possibleJumps[j, 1];
                                        jumpCondition = true;
                                        break;
                                    }
                                }
                                if(!jumpCondition)
                                {
                                    binary += possibleJumps[0, 1];
                                }
                                sw.WriteLine(binary);
                                Console.WriteLine("c; name: " + currentInstruction + " dest:" + destinationInstruction + " comp: " + computationInstruction + " jump: " + jumpInstruction+" binary:"+ binary);

                            }

                        }
                        sw.Close();
                    }
                    sr.Close();
                    
                    bool CheckSymbolExists(string name)
                    {
                        bool symbolExists = false;
                        foreach (Symbol current in allSymbols)
                        {
                            if (name == current.name)
                            {
                                symbolExists = true;
                            }
                        }
                        return symbolExists;
                    }
                    int GetSymbolIdi(string name)
                    {
                        foreach (Symbol current in allSymbols)
                        {
                            if (name == current.name)
                            {
                                return current.idi;
                            }
                        }
                        return -1;
                    }
                    
                }
            }
        }
               
            
        public static string Get16DigitBinary(int num)
        {
            string variableDigitBinary = Convert.ToString(num, 2);
            int amountOfZerosToAdd = 16 - variableDigitBinary.Length;
            string zeroString = "";
            for(int i = 0; i< amountOfZerosToAdd;i++)
            {
                zeroString += "0";
            }
            return zeroString + variableDigitBinary;
        }
        public static bool CheckIsNumeric(string str)
        {
            int n;
            return int.TryParse(str, out n);
        }
    }
        
    class Symbol
    {
        public string name;
        public int idi;
        public Symbol(string nameP,int idiP)
        {
            name = nameP;
            idi = idiP;
        }
    }



}
