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
            complier.TranslateFileOrFolder("StaticsTest", "StaticsTest");
        }

        public void TranslateFileOrFolder(string fileOrFolder,string asmFileNameShort)
        {
            string parentFileName = @"C:\Users\Carte\Source\Repos\Nand2TetrisCourse\CompilerAssemblerToMachine\VmTranslator\";
            List<string> filesToTranslate = new List<string>();
            bool containsSysFile =false;
            string SystemFileName = parentFileName + "Sys.vm";
            foreach (string current in Directory.GetFiles(parentFileName))
            {
                string fullFileNameSearching = parentFileName + fileOrFolder + ".vm";
                
                if (current == fullFileNameSearching)
                {
                    containsSysFile = (current == SystemFileName);
                    filesToTranslate.Add(current);
                    break;
                }
            }
            if (filesToTranslate.Count<=0)
            {
                foreach (string current in Directory.GetDirectories(parentFileName))
                {
                    string fullFolderNameSearching = parentFileName + fileOrFolder;
                    SystemFileName= parentFileName + fileOrFolder+ @"\Sys.vm";
                    if (current == fullFolderNameSearching)
                    {
                        
                        foreach (String currentfileName in Directory.GetFiles(fullFolderNameSearching, "*.vm"))
                        {
                                if(currentfileName == SystemFileName)
                                {
                                    containsSysFile = true;
                                }
                                Console.WriteLine(currentfileName);
                                filesToTranslate.Add(currentfileName);
                        }
                        break;
                    }
                }
            }
            List<VmCommand> allComands = ParseVmFiles(filesToTranslate);
            string fullAsmFileName = parentFileName + asmFileNameShort + ".asm";
            WriteCode(allComands, fullAsmFileName, containsSysFile);
        }

        public void WriteCode(List<VmCommand> commands,string asmFileNameFull,bool containsSystemP)
        {
            List<string> assemblerCommands = new List<string>();
            int commandCommandCounter =0;
            if(containsSystemP)
            {
                assemblerCommands.Add("@"+256);
                assemblerCommands.Add("D=A");
                assemblerCommands.Add("@SP");
                assemblerCommands.Add("M=D");
                assemblerCommands.AddRange(CallFunction("Sys.init", "0", commandCommandCounter));
                commandCommandCounter++;
            }
            
            //initial commands
            for (int i = 0; i < commands.Count; i++)
            {
                
                assemblerCommands.Add("//////////" + commandCommandCounter + ": " + commands[i].instruction + " " + commands[i].commandInfo1 + " " + commands[i].commandInfo2 + "");
                if (commands[i].instruction == "push")
                {
                    if (commands[i].commandInfo1 == "constant")
                    {
                        assemblerCommands.Add("@" + commands[i].commandInfo2);
                        assemblerCommands.Add("D=A");
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    if (commands[i].commandInfo1 == "static")
                    {
                        assemblerCommands.Add("@" +"static"+ commands[i].fileName+"." + commands[i].commandInfo2);
                        assemblerCommands.Add("D=M");
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    if (commands[i].commandInfo1 == "temp")
                    {
                        int tempLoc = 5 + int.Parse(commands[i].commandInfo2);
                        assemblerCommands.Add("@" + tempLoc);
                        assemblerCommands.Add("D=M");
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }
                    else
                    if (commands[i].commandInfo1 == "pointer")
                    {
                        if (commands[i].commandInfo2 == "0")
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
                        assemblerCommands.AddRange(PlaceValueFromMemoryLocationOnD(commands[i].commandInfo1, commands[i].commandInfo2));
                        assemblerCommands.AddRange(PlaceDOnStack());
                    }

                }
                else
                if (commands[i].instruction == "pop")
                {
                    if (commands[i].commandInfo1 == "constant")
                    {
                        assemblerCommands.Add("//we poped a constant wierd");
                    }
                    else
                    if (commands[i].commandInfo1 == "static")
                    {
                        assemblerCommands.AddRange(GetTopStackOnD());
                        assemblerCommands.Add("@" + "static" + commands[i].fileName + "." + commands[i].commandInfo2);
                        assemblerCommands.Add("M=D");
                    }
                    else
                    if (commands[i].commandInfo1 == "temp")
                    {
                        assemblerCommands.AddRange(GetTopStackOnD());
                        int tempLoc = 5 + int.Parse(commands[i].commandInfo2);
                        assemblerCommands.Add("@" + tempLoc);
                        assemblerCommands.Add("M=D");
                    }
                    else
                    if (commands[i].commandInfo1 == "pointer")
                    {
                        if (commands[i].commandInfo2 == "0")
                        {
                            assemblerCommands.AddRange(GetTopStackOnD());
                            assemblerCommands.Add("@THIS");
                            assemblerCommands.Add("M=D");
                        }
                        else
                        {
                            assemblerCommands.AddRange(GetTopStackOnD());
                            assemblerCommands.Add("@THAT");
                            assemblerCommands.Add("M=D");
                        }
                    }
                    else
                    {
                        assemblerCommands.Add("@" + commands[i].commandInfo2);
                        assemblerCommands.Add("D=A");
                        assemblerCommands.Add("@" + GetMemoryShort(commands[i].commandInfo1));
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
                    assemblerCommands.Add("@EQTRUE" + commandCommandCounter);
                    assemblerCommands.Add("D;JEQ");
                    assemblerCommands.Add("D=0");
                    assemblerCommands.Add("@EQEND" + commandCommandCounter);
                    assemblerCommands.Add("0;JMP");
                    assemblerCommands.Add("(EQTRUE" + commandCommandCounter + ")");
                    assemblerCommands.Add("D=" + TRUE_VALUE_ASSEMBLY);
                    assemblerCommands.Add("(EQEND" + commandCommandCounter + ")");


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
                    assemblerCommands.Add("@GTTRUE" + commandCommandCounter); //this will need an idi
                    assemblerCommands.Add("D;JGT");
                    assemblerCommands.Add("D=0");
                    assemblerCommands.Add("@GTEND" + commandCommandCounter);
                    assemblerCommands.Add("0;JMP");
                    assemblerCommands.Add("(GTTRUE" + commandCommandCounter + ")");
                    assemblerCommands.Add("D=" + TRUE_VALUE_ASSEMBLY);
                    assemblerCommands.Add("(GTEND" + commandCommandCounter + ")");


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
                    assemblerCommands.Add("@LTTRUE" + commandCommandCounter);
                    assemblerCommands.Add("D;JLT");
                    assemblerCommands.Add("D=0");
                    assemblerCommands.Add("@LTEND" + commandCommandCounter);
                    assemblerCommands.Add("0;JMP");
                    assemblerCommands.Add("(LTTRUE" + commandCommandCounter + ")");
                    assemblerCommands.Add("D=" + TRUE_VALUE_ASSEMBLY);
                    assemblerCommands.Add("(LTEND" + commandCommandCounter + ")");


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
                else
                if(commands[i].instruction == "label")
                {
                    assemblerCommands.Add("("+ commands[i].commandInfo1+ "$bar)");
                }
                else
                if(commands[i].instruction == "goto")
                {
                    assemblerCommands.Add("@"+ commands[i].commandInfo1+"$bar");
                    assemblerCommands.Add("0;JMP");
                }
                else
                if(commands[i].instruction == "if-goto")
                {
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=M-1");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@" + commands[i].commandInfo1 + "$bar");
                    assemblerCommands.Add("D;JNE");
                }
                else
                if (commands[i].instruction == "call")
                {
                    assemblerCommands.AddRange(CallFunction(commands[i].commandInfo1, commands[i].commandInfo2, commandCommandCounter));
                }
                else
                if (commands[i].instruction == "function")
                {
                    assemblerCommands.Add("("+commands[i].commandInfo1+")");

                    int localVariables = int.Parse(commands[i].commandInfo2);
                    assemblerCommands.Add("@SP");
                    for (int j =0;j < localVariables;j++)
                    {
                        assemblerCommands.Add("A=M");
                        assemblerCommands.Add("M=0");
                        assemblerCommands.Add("@SP");
                        assemblerCommands.Add("M=M+1");
                    }
                }
                else
                if(commands[i].instruction == "return")
                {
                   

                    //Setting momentary variable new stack pos old Arg
                    assemblerCommands.Add("@ARG");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M+1");
                    assemblerCommands.Add("M=D");

                    //Setting momentary variable end ofstored saved old LCL
                    assemblerCommands.Add("@LCL");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("M=D");
                    
                    //reseting ARG
                    assemblerCommands.Add("@3");
                    assemblerCommands.Add("D=A");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("A=M-D");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@ARG");
                    assemblerCommands.Add("M=D");

                    //reseting THIS
                    assemblerCommands.Add("@2");
                    assemblerCommands.Add("D=A");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("A=M-D");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@THIS");
                    assemblerCommands.Add("M=D");

                    //reseting THAT
                    assemblerCommands.Add("@1");
                    assemblerCommands.Add("D=A");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("A=M-D");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@THAT");
                    assemblerCommands.Add("M=D");

                    //reseting LCL
                    assemblerCommands.Add("@4");
                    assemblerCommands.Add("D=A");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("A=M-D");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@LCL");
                    assemblerCommands.Add("M=D");

                    //storing return adress top of stack
                    assemblerCommands.Add("@5");
                    assemblerCommands.Add("D=A");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("A=M-D");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M+1");
                    assemblerCommands.Add("A=M+1");
                    assemblerCommands.Add("M=D");

                    //returning value on stack
                    assemblerCommands.Add("@SP"); //still maney issues here nothings will work
                    assemblerCommands.Add("A=M-1");
                    assemblerCommands.Add("D=M");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M+1");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("M=D");

                    //repositioning SP removing all current function stuff still 
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("A=M+1");
                    assemblerCommands.Add("D=M+1");
                    assemblerCommands.Add("@SP");
                    assemblerCommands.Add("M=D");

                    //go back to the the return adress of the function code
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("A=M");
                    assemblerCommands.Add("0;JMP");
                }

                commandCommandCounter++;
            }

            foreach (String current in assemblerCommands)
            {
                Console.WriteLine("" + current);
            }
            using (StreamWriter sw = File.CreateText(asmFileNameFull))
            {
                foreach (String current in assemblerCommands)
                {
                    sw.WriteLine(current);
                }
                sw.Close();
            }
            
        }
        public List<String> CallFunction(string functionName,string argumentsP,int callIdi)
        {
            List<String> commandList = new List<string>();
            

            

            string returnAdressName = functionName + "$ret." + callIdi;
            //save adress of return command
            commandList.Add("@" + returnAdressName);
            commandList.Add("D=A");
            commandList.Add("@SP");
            commandList.Add("A=M");
            commandList.Add("M=D");

            commandList.Add("@SP");
            commandList.Add("M=M+1");//1

            //save LCL
            commandList.Add("@LCL");
            commandList.Add("D=M");
            commandList.Add("@SP");
            commandList.Add("A=M");
            commandList.Add("M=D");

            commandList.Add("@SP");
            commandList.Add("M=M+1");//2

            //save ARG
            commandList.Add("@ARG");
            commandList.Add("D=M");
            commandList.Add("@SP");
            commandList.Add("A=M");
            commandList.Add("M=D");

            commandList.Add("@SP");
            commandList.Add("M=M+1");//3

            //save THIS
            commandList.Add("@THIS");
            commandList.Add("D=M");
            commandList.Add("@SP");
            commandList.Add("A=M");
            commandList.Add("M=D");

            commandList.Add("@SP");
            commandList.Add("M=M+1");//4

            //save THAT
            commandList.Add("@THAT");
            commandList.Add("D=M");
            commandList.Add("@SP");
            commandList.Add("A=M");
            commandList.Add("M=D");

            commandList.Add("@SP");
            commandList.Add("M=M+1");//5

            //change ARG pointer to new location
            int arguments = int.Parse(argumentsP);
            int argPointerRelativeToNewLocation = arguments + 5;
            commandList.Add("@" + argPointerRelativeToNewLocation);
            commandList.Add("D=A");
            commandList.Add("@SP");
            commandList.Add("D=M-D");
            commandList.Add("@ARG");
            commandList.Add("M=D");

            //change LCL pointer to new location
            commandList.Add("@SP");
            commandList.Add("D=M");
            commandList.Add("@LCL");
            commandList.Add("M=D");

            //jump to function code
            commandList.Add("@" + functionName );
            commandList.Add("0;JMP");

            //return adress Lable
            commandList.Add("(" + returnAdressName + ")");
            return commandList;
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
       
        List<VmCommand> ParseVmFiles(List<String> allFileToParse)
        {
            List<VmCommand> vmCommandList = new List<VmCommand>();
            foreach (String currentFileParsing in allFileToParse)
            {
                if (File.Exists(currentFileParsing))
                {
                    Console.WriteLine("found file");
                    List<string> instructions = new List<string>();
                    using (StreamReader streamReaderOfFile = File.OpenText(currentFileParsing))
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
                                    if (currentLine[i] != ' '&&currentLine[i] !='\t')
                                    {
                                        currentKeyWord += currentLine[i];
                                    }
                                    if (currentLine[i] == ' ' || currentLine[i] == '\t' || currentLine.Length <= i + 1 || (i+2 <= currentLine.Length && currentLine[i+1] == '/' && currentLine[i + 2] == '/'))
                                    {
                                        if (currentKeyWord == "push" || currentKeyWord == "pop"
                                             || currentKeyWord == "add" || currentKeyWord == "sub"
                                             || currentKeyWord == "neg" || currentKeyWord == "eq"
                                             || currentKeyWord == "gt" || currentKeyWord == "lt"
                                             || currentKeyWord == "and" || currentKeyWord == "or"
                                             || currentKeyWord == "not" || currentKeyWord == "goto"
                                             || currentKeyWord == "if-goto" || currentKeyWord == "label"
                                             || currentKeyWord == "call" || currentKeyWord == "function"
                                             || currentKeyWord == "return" )
                                        {
                                            
                                            currentVmCommand = new VmCommand(currentKeyWord, "", "", Path.GetFileNameWithoutExtension(currentFileParsing));
                                            vmCommandList.Add(currentVmCommand);
                                        }
                                        else
                                        if (currentVmCommand != null&& currentVmCommand.instruction!=""&& currentVmCommand.commandInfo1=="")
                                        {
                                            currentVmCommand.commandInfo1 = currentKeyWord;
                                        }
                                        else
                                        if (currentVmCommand != null && currentVmCommand.instruction != "" && currentVmCommand.commandInfo1 != "" && currentVmCommand.commandInfo2 == "")
                                        {
                                            int nV;
                                            if (int.TryParse(currentKeyWord, out nV))
                                            {
                                                currentVmCommand.commandInfo2 = currentKeyWord;
                                            }
                                        }
                                        currentKeyWord = "";
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
            for (int i = 0; i < vmCommandList.Count; i++)
            {
                Console.WriteLine(i + ") command: " + vmCommandList[i].instruction + " " + vmCommandList[i].commandInfo1 + " " + vmCommandList[i].commandInfo2);
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
        public class VmCommand
        {
            public string instruction ="";
            public string commandInfo1="";
            public string commandInfo2="";
            public string fileName = "";
            public VmCommand(string insP,string commandInfo1P, string commandInfo2P,string fileNameP)
            {
                 instruction = insP;
                 commandInfo1 = commandInfo1P;
                 commandInfo2= commandInfo2P;
                 fileName = fileNameP;
            }
        }
    }

    
}
