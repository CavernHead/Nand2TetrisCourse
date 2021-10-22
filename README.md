# Nand2TetrisCourse
Here is my code related to doing the exercises of the famous course nand2teris about making a computer from basic logic gates.
The logic gate part of the course is not here, here is mainly the compilitation code to transform the high level programming language jack into machine langauge.

/Compiler/CompilerAssemblerToMachine:
  C# .net code
  translates assembler code to the 16bit machine language of the so called "hack" computer
  main execution file: file:/Compiler/CompilerAssemblerToMachine/Compiler.cs
  
  other files are translation tests.
  
   special files:
  .hack: hack machine language
  .asm: assembler for hack computer

/Compiler/VmTranslator:
  C# .net code
  translates Virtual Machine code to Assembler.
  main execution file: file:/Compiler/VmTranslator/Compiler.cs
  
  other files and folders are translation tests.
  
  special files:
  .vm virtual machine languager

/Compiler/JackCompiler:
  C# .net code
  translates Jack language to Virtual Machine Language.
  main exection file: file:/Compiler/JackCompiler/MainRun.cs
  Tokenizer: file:/Compiler/JackCompiler/Tokenizer.cs, makes a token list.
  Parser: file:/Compiler/JackCompiler/Parser.cs, currently just creates xml file just to show if things have been parsed properly.
  
  Other files and folders are translation tests.
  
  special files:
  .jack high level simple language 
 
  
/JackTest:
  test program using the jack language that writes numbers on the screen of the hack computer
  
   special files:
   .jack high level simple language 
  
 Here is a link to the nan2tetris course by Noam Nisan and Shimon Schocken:
 https://www.nand2tetris.org/
 

  
