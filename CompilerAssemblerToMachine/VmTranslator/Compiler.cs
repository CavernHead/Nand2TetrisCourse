using System;
using System.IO;
using System.Collections.Generic;

namespace VmTranslator
{
    class Compiler
    {
        const string TRUE_VALUE_ASSEMBLY = "-1";
        static void Main(string[] args)
        {
            Compiler complier = new Compiler();
            complier.TranslateAssemblerToMachine("StackTest");
            complier.TranslateAssemblerToMachine("SimpleAdd");
            complier.TranslateAssemblerToMachine("BasicTest");
            complier.TranslateAssemblerToMachine("PointerTest");
            complier.TranslateAssemblerToMachine("StaticTest");
        }
        void TranslateAssemblerToMachine(string FileName)
        {
            TranslateVmToAssembler(FileName + ".vm", FileName + ".asm");
        }
        void TranslateVmToAssembler(string VMFileName, string AssemblerFileName)
        {
            string parentFileName = @"C:\Users\Carte\Source\Repos\Nand2TetrisCourse\CompilerAssemblerToMachine\VmTranslator\";
            TranslateVmToAssembler(parentFileName, VMFileName, AssemblerFileName);
        }
        void TranslateVmToAssembler(string parentFileName, string VmFileName, string assemblerFileName)
        {
            Console.WriteLine("did something");
            string VmFullfileName = parentFileName + VmFileName;
            List<VmCommand> commands = GetVmCommandsFromFile(VmFullfileName);
            List<string> assemblerCommands = new List<string>();
            for(int i = 0;i < commands.Count;i++)
            {
                assemblerCommands.Add("//////////"+i+": " + commands[i].instruction + " " + commands[i].memoryType + " " + commands[i].variable+"");
                if (commands[i].instruction == "push")
                {
                    if(commands[i].memoryType=="constant")
                    {
                        assemblerCommands.Add("@"+commands[i].variable);
                        assemblerCommands.Add("D=A");
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    if(commands[i].memoryType == "static")
                    {
                        assemblerCommands.Add("@" + "static"+commands[i].variable);
                        assemblerCommands.Add("D=M");
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    if(commands[i].memoryType == "temp")
                    {
                        int tempLoc = 5 + int.Parse(commands[i].variable);
                        assemblerCommands.Add("@"+ tempLoc);
                        assemblerCommands.Add("D=M");
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    if(commands[i].memoryType == "pointer")
                    {
                        if(commands[i].variable=="0")
                        {
                            assemblerCommands.Add("@THIS");
                            assemblerCommands.Add("D=M");
                        }
                        else
                        {
                            assemblerCommands.Add("@THAT");
                            assemblerCommands.Add("D=M");
                        }
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    {
                        assemblerCommands.AddRange(PlaceValueFromMemoryLocationOnD(commands[i].memoryType, commands[i].variable));
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    
                }
                else
                if(commands[i].instruction == "pop")
                {
                    if(commands[i].memoryType == "constant")
                    {
                        assemblerCommands.Add("//we poped a constant wierd");
                    }
                    else
                    if (commands[i].memoryType == "static")
                    {
                        assemblerCommands.AddRange(GetTopStackOnD());
                        assemblerCommands.Add("@" + "static" + commands[i].variable);
                        assemblerCommands.Add("M=D");
                    }
                    else
                    if (commands[i].memoryType == "temp")
                    {
                        assemblerCommands.AddRange(GetTopStackOnD());
                        int tempLoc = 5 + int.Parse(commands[i].variable);
                        assemblerCommands.Add("@" + tempLoc);
                        assemblerCommands.Add("M=D");
                    }
                    else
                    if (commands[i].memoryType == "pointer")
                    {
                        if (commands[i].variable == "0")
                        {
                            assemblerCommands.AddRange(GetTopStackOnD());
                            assemblerCommands.Add("@THIS");
                            assemblerCommands.Add("M=D");
                        }
                        else
                        {
                            assemblerCommands.AddRange(GetTopStackOnD());
                            assemblerCommands.Add("@THAT");
                            assemblerCommands.Add("M=D");                        }
                    }
                    else
                    {
                        assemblerCommands.Add("@" + commands[i].variable);
                        assemblerCommands.Add("D=A");
                        assemblerCommands.Add("@" + GetMemoryShort(commands[i].memoryType));
                        assemblerCommands.Add("D=M+D");
                        assemblerCommands.Add("@SP");
                        assemblerCommands.Add("A=M");
                        assemblerCommands.Add("M=D");
                        assemblerCommands.Add("A=A-1");
                        assemblerCommands.Add("D=M");
                        assemblerCommands.Add("A=A+1");
                        assemblerCommands.Add("A=M");
                        assemblerCommands.Add("M=D");
                        assemblerCommands.Add("@SP");
                        assemblerCommands.Add("M=M-1");
                    }
                }
                else
                if (commands[i].instruction == "add")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D+M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");

                    assemblerCommands.AddRange(PlaceDOnStack());

                }
                else
                if (commands[i].instruction == "sub")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D-M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                   

                    assemblerCommands.AddRange(PlaceDOnStack());
                }
                else
                if (commands[i].instruction == "neg")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("M=-M");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M+1");
                }
                else
                if (commands[i].instruction == "eq")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D-M");
                    assemblerCommands.Add("@EQTRUE"+i);
                    assemblerCommands.Add("D;JEQ");
                    assemblerCommands.Add("D=0");
                    assemblerCommands.Add("@EQEND" + i);
                    assemblerCommands.Add("0;JMP");
                    assemblerCommands.Add("(EQTRUE" + i+")");
                    assemblerCommands.Add("D="+ TRUE_VALUE_ASSEMBLY);
                    assemblerCommands.Add("(EQEND" + i + ")");


                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.AddRange(PlaceDOnStack());
                }
                else
                if (commands[i].instruction == "gt")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D-M");
                    assemblerCommands.Add("@GTTRUE" + i); //this will need an idi
                    assemblerCommands.Add("D;JGT");
                    assemblerCommands.Add("D=0");
                    assemblerCommands.Add("@GTEND" + i);
                    assemblerCommands.Add("0;JMP");
                    assemblerCommands.Add("(GTTRUE" + i + ")");
                    assemblerCommands.Add("D="+ TRUE_VALUE_ASSEMBLY);
                    assemblerCommands.Add("(GTEND" + i + ")");


                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.AddRange(PlaceDOnStack());
                }
                else
                if (commands[i].instruction == "lt")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D-M");
                    assemblerCommands.Add("@LTTRUE" + i);
                    assemblerCommands.Add("D;JLT");
                    assemblerCommands.Add("D=0");
                    assemblerCommands.Add("@LTEND" + i);
                    assemblerCommands.Add("0;JMP");
                    assemblerCommands.Add("(LTTRUE" + i + ")");
                    assemblerCommands.Add("D=" + TRUE_VALUE_ASSEMBLY);
                    assemblerCommands.Add("(LTEND" + i + ")");


                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.AddRange(PlaceDOnStack());
                }
                else
                if (commands[i].instruction == "and")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D&M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.AddRange(PlaceDOnStack());
                }
                else
                if (commands[i].instruction == "or")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=D|M");
                    

                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.AddRange(PlaceDOnStack());
                }
                else
                if (commands[i].instruction == "not")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("M=!M");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M+1");
                }

            }

            foreach (String current in assemblerCommands)
            {
                Console.WriteLine("" + current);
            }
            using (StreamWriter sw = File.CreateText(parentFileName + assemblerFileName))
            {
                foreach (String current in assemblerCommands)
                {
                    sw.WriteLine(current);
                }
            }


            }
   
        List<String> PlaceValueFromMemoryLocationOnD(string memorylocation,string variable)
        {
            List<String> commandList = new List<string>();
            commandList.Add("@" + variable);
            commandList.Add("D=A");
            commandList.Add("@" + GetMemoryShort(memorylocation));
            commandList.Add("A=M+D");
            commandList.Add("D=M");
            return commandList;
        }
        List<String> PlaceDOnStack()
        {
            List<String> commandList= new List<string>();
            commandList.Add("@SP");
            commandList.Add("A=M");
            commandList.Add("M=D");
            commandList.Add("@SP");
            commandList.Add("M=M+1");
            return commandList;
        }
        List<String> GetTopStackOnD()
        {
            List<String> commandList = new List<string>();
            commandList.Add("@SP");
            commandList.Add("M=M-1");
            commandList.Add("A=M");
            commandList.Add("D=M");
            commandList.Add("@SP");
            return commandList;
        }
       
        

        

        List<VmCommand> GetVmCommandsFromFile(string fileName)
        {

            List<VmCommand> vmCommandList = new List<VmCommand>();
            if (File.Exists(fileName))
            {
                Console.WriteLine("found file");
                List<string> instructions = new List<string>();
                using (StreamReader streamReaderOfFile = File.OpenText(fileName))
                {
                    string currentLine = "";
                   
                    while ((currentLine = streamReaderOfFile.ReadLine()) != null)
                    {
                        string currentKeyWord = "";
                        VmCommand currentVmCommand = null;
                        for (int i = 0; i < currentLine.Length; i++)
                        {
                            if (currentLine[i] == '/' && i + 1 < currentLine.Length && currentLine[i + 1] == '/')
                            {
                                break;
                            }
                            else
                            {
                                if (currentLine[i] != ' ')
                                {
                                    currentKeyWord += currentLine[i];
                                }
                                if (currentLine[i] == ' ' || currentLine.Length <= i + 1)
                                {
                                    if (currentKeyWord == "push" || currentKeyWord == "pop"
                                         || currentKeyWord == "add" || currentKeyWord == "sub"
                                         || currentKeyWord == "neg" || currentKeyWord == "eq"
                                         || currentKeyWord == "gt" || currentKeyWord == "lt"
                                         || currentKeyWord == "and" || currentKeyWord == "or"
                                         || currentKeyWord == "not")
                                    {
                                        currentVmCommand = new VmCommand(currentKeyWord, "", "");
                                        vmCommandList.Add(currentVmCommand);
                                    }
                                    else
                                    if ((currentKeyWord == "local" || currentKeyWord == "arg"
                                      || currentKeyWord == "this" || currentKeyWord == "that"
                                      || currentKeyWord == "constant" || currentKeyWord == "pointer"
                                      || currentKeyWord == "static" || currentKeyWord == "temp"
                                      || currentKeyWord == "argument") &&
                                      currentVmCommand != null)
                                    {
                                        currentVmCommand.memoryType = currentKeyWord;
                                    }
                                    else
                                    if (currentVmCommand != null)
                                    {
                                        int nV;
                                        if (int.TryParse(currentKeyWord, out nV))
                                        {
                                            currentVmCommand.variable = currentKeyWord;
                                        }
                                    }
                                    currentKeyWord = "";
                                }
                            }



                        }
                    }
                    for (int i = 0; i< vmCommandList.Count;i++)
                    {
                        Console.WriteLine(i+") command: " + vmCommandList[i].instruction + " " + vmCommandList[i].memoryType + " " + vmCommandList[i].variable);
                    }
                }
            }
            return vmCommandList;
        }
        string GetMemoryShort(string memoryLocationlong)
        {
            if(memoryLocationlong=="argument")
            {
                return "ARG";
            }
            else
            if (memoryLocationlong == "local")
            {
                return "LCL";
            }
            else
            if (memoryLocationlong == "this")
            {
                return "THIS";
            }
            else
            if (memoryLocationlong == "that")
            {
                return "THAT";
            }
            return "NON EXISTANT";
        }
        class VmCommand
        {
            public string instruction;
            public string memoryType;
            public string variable;
            public VmCommand(string insP,string memtypeP, string variableP)
            {
                 instruction = insP;
                 memoryType = memtypeP;
                 variable= variableP;
            }
        }
    }

    
}
