using System;
using System.IO;
using System.Collections.Generic;
using JackCompiler;

namespace JackCompiler
{
    class Parser
    {
        List<Token> allTokens;
        StreamWriter streamXml;

        bool xmlFile;
        int parseCount;
        Token currentToken;
        Token nextToken;
        string[] statments = { "let","do","if","while","return" };
        char[] operations = {'+','-','*','/','&','|','<','>','='};
        char[] unaryOperations = { '-', '~' };

        SymbolTable classLevelSymTable;
        SymbolTable subroutineLevelSymTable;
        VmFileManager vmFileMng;
        public int uniqueIdentifier;
        public void ParseFile(List<Token> allTokensP, string fileName)
        {
            xmlFile = false;
            allTokens = allTokensP;
            parseCount = 0;
            if(allTokens.Count>0)
            {
                currentToken = allTokens[parseCount];
                if(xmlFile)
                {
                    streamXml = File.CreateText(Path.ChangeExtension(fileName, ".xml"));
                }
                
                vmFileMng = new VmFileManager(fileName);
                if (currentToken.tokenValue == "class")
                {
                    CompileClass(0);
                }
                else
                {
                    Console.WriteLine("expected class keyword in file at start");
                }
                if(streamXml!=null)
                {
                    streamXml.Close();
                }
                
                vmFileMng.Close();
            }
            else
            {
                Console.WriteLine("not tokens found");
            }
           
        }
        void CompileClass(int spacing)
        {
           
            WriteToConsolXml("<class>", spacing);
            if (currentToken.tokenValue =="class"&& currentToken.tokenType == Token.Type.keyword)
            {
                uniqueIdentifier = 0;

                classLevelSymTable = new SymbolTable();
                Console.WriteLine("__________new class symbol table");
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.identifier)
                {
                    WriteTerminalToken(spacing);
                    string className = currentToken.tokenValue;
                    AdvanceParseCount();
                    if(currentToken.tokenValue == "{")
                    {
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        while (true)
                        {
                            if (currentToken.tokenValue=="field"|| currentToken.tokenValue == "static")
                            {
                                CompileVarDec(true, spacing);
                                AdvanceParseCount();
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (true)
                        {
                            if (currentToken.tokenValue == "function" || currentToken.tokenValue == "constructor" || currentToken.tokenValue == "method")
                            {
                                CompileSubroutineDec(spacing,className);
                                AdvanceParseCount();
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (currentToken.tokenValue == "}")
                        {
                            WriteTerminalToken(spacing);
                            WriteToConsolXml("</class>", spacing) ;
                        }
                        else
                        {
                            Console.WriteLine("CompileClass expected symbole }");
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompileClass expected symbole {");
                    }
                }
                else
                {
                    Console.WriteLine("CompileClass expected identifier ");
                }
            }
            else
            {
                Console.WriteLine("CompileClass expected class keyword");
            }
            
        }
        void CompileVarDec(bool classVar, int spacing)
        {
            spacing++;
            if (classVar) { WriteToConsolXml("<classVarDec>",spacing); }
            else { WriteToConsolXml("<varDec>",spacing); }
            
            if((classVar&&(currentToken.tokenValue == "field"||currentToken.tokenValue=="static"))||
                (!classVar&&currentToken.tokenValue=="var"))
            {
                string variableKind = currentToken.tokenValue;
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.keyword|| currentToken.tokenType == Token.Type.identifier)//type
                {
                    string variableType = currentToken.tokenValue;
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    
                    while(true)
                    {
                        Symbol variableSymbol = new Symbol();
                        variableSymbol.type = variableType;
                        if(variableKind == "field")
                        {
                            variableSymbol.kindE = Symbol.kind.field;
                        }
                        else
                        if(variableKind == "var")
                        {
                            variableSymbol.kindE = Symbol.kind.var;
                        }
                        else
                        if (variableKind == "static")
                        {
                            variableSymbol.kindE = Symbol.kind.staticE;
                        }

                        if (currentToken.tokenType == Token.Type.identifier) //variable name
                        {
                            WriteTerminalToken(spacing);
                            variableSymbol.name = currentToken.tokenValue;
                            AdvanceParseCount();
                            if (currentToken.tokenValue == ",")
                            {
                                if (classVar){ classLevelSymTable.AddSymbol(variableSymbol); }
                                else { subroutineLevelSymTable.AddSymbol(variableSymbol); }

                                WriteTerminalToken(spacing);
                                AdvanceParseCount();
                            }
                            else
                            if (currentToken.tokenValue == ";")
                            {

                                WriteTerminalToken(spacing);
                                if (classVar) { classLevelSymTable.AddSymbol(variableSymbol); }
                                else { subroutineLevelSymTable.AddSymbol(variableSymbol); }
                                if (classVar) { WriteToConsolXml("</classVarDec>",spacing); }
                                else { WriteToConsolXml("</varDec>",spacing); }
                                break;
                            }
                            else
                            {
                                Console.WriteLine("CompileVarDec expected ',' or ';'");
                                break; 
                            }
                        }
                        else
                        {
                            Console.WriteLine("CompileVarDec expected identifier");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("CompileVarDec expected type keyword");
                }
            }
            else
            {
                Console.WriteLine("CompileVarDec expected field or static keyword");
            }
            
        }
        void CompileSubroutineDec(int spacing,string className)
        {
            spacing++;
            WriteToConsolXml("<subroutineDec>",spacing);
            bool isConstructor = currentToken.tokenValue == "constructor";
            bool isMethode = currentToken.tokenValue == "method";
            if (currentToken.tokenValue == "function" || isConstructor || isMethode)
            {
               
               // string subroutineTyp = currentToken.tokenValue;
                subroutineLevelSymTable = new SymbolTable();
                Console.WriteLine("_____________new subroutine symbol table");

                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.keyword|| currentToken.tokenType == Token.Type.identifier)//return type
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenType == Token.Type.identifier)
                    {
                        WriteTerminalToken(spacing);
                        string subroutineName = currentToken.tokenValue;
                        AdvanceParseCount();
                        if(currentToken.tokenValue == "(")
                        {
                            if(isMethode)
                            {
                                subroutineLevelSymTable.AddSymbol("this", className,Symbol.kind.arg); //not sure if i should name the var this since it's a key word
                            }
                            
                            CompileParameterList(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue == "{")
                            {
                                WriteTerminalToken(spacing);
                                AdvanceParseCount();
                                CompileSubroutineBody(spacing,className,subroutineName, isMethode, isConstructor);
                                AdvanceParseCount();
                                if (currentToken.tokenValue == "}")
                                {
                                    WriteTerminalToken(spacing);
                                    WriteToConsolXml("</subroutineDec>",spacing);
                                }
                                else
                                {
                                    Console.WriteLine("CompileSubroutineDec expected '}'");
                                }
                            }
                            else
                            {
                                Console.WriteLine("CompileSubroutineDec expected '{'");
                            }
                        }
                        else
                        {
                            Console.WriteLine("CompileSubroutineDec expected '('");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("CompileSubroutineDec expected subroutine return type");
                }
            }
            else
            {
                Console.WriteLine("CompileSubroutineDec expected function or construtctor or method keyword");
            }
        }
        void CompileParameterList(int spacing)
        {
            spacing++;
            WriteToConsolXml("<parameterList>",spacing);
            if(currentToken.tokenValue=="(")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                while (true)
                {
                    if (currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                        WriteToConsolXml("</parameterList>",spacing);
                        break;
                    }
                    else
                    {
                        
                        if (currentToken.tokenType == Token.Type.keyword|| currentToken.tokenType == Token.Type.identifier) //type
                        {
                            Symbol currentParameter = new Symbol();
                            currentParameter.type = currentToken.tokenValue;
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenType == Token.Type.identifier)
                            {
                                WriteTerminalToken(spacing);
                                currentParameter.name = currentToken.tokenValue;
                                currentParameter.kindE = Symbol.kind.arg;
                                subroutineLevelSymTable.AddSymbol(currentParameter);
                                AdvanceParseCount();
                                if (currentToken.tokenValue == ","|| currentToken.tokenValue == ")")
                                {
                                    if (currentToken.tokenValue == ",")
                                    {
                                        WriteTerminalToken(spacing);
                                        AdvanceParseCount();
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("CompileParameterList expected ','or ')' ");
                                    break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("CompileParameterList expected parameter identifier");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("CompileParameterList expected parameter identifier");
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("CompileParameterList expected '('");
            }
           
        }
        void CompileSubroutineBody(int spacing,string className,string subroutineName,bool methode,bool constructor)
        {
            spacing++;
            WriteToConsolXml("<subroutineBody>",spacing);
            while(true)
            {
                if(currentToken.tokenValue =="var")
                {
                    CompileVarDec(false, spacing);
                    AdvanceParseCount();
                }
                else
                {
                   break;
                }
            }
            vmFileMng.DeclareSubroutine(className, subroutineName,subroutineLevelSymTable.qVar);
            if(methode|| constructor)
            {
                if(constructor)
                {
                    vmFileMng.Push(VmFileManager.MemoryE.constant, classLevelSymTable.qFields);
                    vmFileMng.CallSubroutine("Memory", "alloc", 1);
                }
                else
                {
                    vmFileMng.Push(VmFileManager.MemoryE.argument, 0);
                }
                vmFileMng.Pop(VmFileManager.MemoryE.pointer, 0);
            }
           

            CompileStatements(spacing,className);
            WriteToConsolXml("</subroutineBody>",spacing);
 
        }
        void CompileStatements(int spacing,string classFrom)
        {
            spacing++;
            WriteToConsolXml("<statements>",spacing);
            while (true)
            {
                if (currentToken.tokenValue == "let")
                {
                    CompileLetStatement(spacing,classFrom);
                    if(nextToken!=null&&GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                    
                }
                else
                if (currentToken.tokenValue == "do")
                {
                    CompileDoStatement(spacing, classFrom);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                if (currentToken.tokenValue == "if")
                {
                    CompileIfStatement(spacing,classFrom);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                if (currentToken.tokenValue == "while")
                {
                    CompileWhileStatement(spacing,classFrom);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                if (currentToken.tokenValue == "return")
                {
                    CompileReturnStatement(spacing,classFrom);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                {
                    WriteToConsolXml("</statements>", spacing);
                    break;
                }
                
            }
            
            
        }
        void CompileLetStatement(int spacing, string classFrom)
        {
            spacing++;
            WriteToConsolXml("<letStatement>",spacing);
            if(currentToken.tokenValue=="let")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.identifier)
                {
                    Symbol variableWorkingOn = FindVariableSymbol(currentToken.tokenValue);
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    bool isArray = false;
                    if(currentToken.tokenValue == "[")
                    {
                        isArray = true;
                        vmFileMng.Push(variableWorkingOn.kindE, variableWorkingOn.pos);
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        CompileExpression(spacing,classFrom);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == "]")
                        {
                            vmFileMng.BasicOp(VmFileManager.OpE.add);
                            vmFileMng.Pop(VmFileManager.MemoryE.temp,0);
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                        }
                        else
                        {
                            Console.WriteLine("expected ']'");
                        }
                    }
                    if(currentToken.tokenValue == "=")
                    {
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        CompileExpression(spacing,classFrom);
                        AdvanceParseCount();
                        if(currentToken.tokenValue == ";")
                        {
                            if(isArray)
                            {
                                vmFileMng.Push(VmFileManager.MemoryE.temp, 0);
                                vmFileMng.Pop(VmFileManager.MemoryE.pointer, 1);
                                vmFileMng.Pop(VmFileManager.MemoryE.that, 0);
                            }
                            else
                            {
                                vmFileMng.Pop(variableWorkingOn.kindE, variableWorkingOn.pos);
                            }
                           

                            WriteToConsolXml("</letStatement>",spacing);
                        }
                        else
                        {
                            Console.WriteLine("CompileLetStatement expected ';'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompileLetStatement expected '='");
                    }
                }
                else
                {
                    Console.WriteLine("CompileLetStatement expected identifier");
                }
            }
            else
            {
                Console.WriteLine("CompileLetStatement expected 'let' keyword");
            }
        }
        void CompileDoStatement(int spacing,string classFrom)
        {
            spacing++;
            WriteToConsolXml("<doStatement>",spacing);
            if(currentToken.tokenValue == "do")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                CompileSubroutineCall(spacing, classFrom);
                AdvanceParseCount();
                if(currentToken.tokenValue == ";")
                {
                    vmFileMng.Pop(VmFileManager.MemoryE.temp, 0);
                    WriteTerminalToken(spacing);
                    WriteToConsolXml("</doStatement>",spacing);
                }
                else
                {
                    Console.WriteLine("CompileDoStatement expected ';'");
                }
               
            }
            else
            {
                Console.WriteLine("CompileDoStatement expected 'do' keyword");
            }
        }
        void CompileIfStatement(int spacing,string classFrom)
        {
            spacing++;
            WriteToConsolXml("<ifStatement>",spacing);
            if (currentToken.tokenValue == "if")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
               
                if (currentToken.tokenValue == "(")
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    CompileExpression(spacing,classFrom);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ")")
                    {
                        int uniqueIfId = GetUniqueIdentifier();
                        vmFileMng.BasicOp(VmFileManager.OpE.not);
                        vmFileMng.IfGoto(classFrom+".else."+ uniqueIfId);
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        if(currentToken.tokenValue == "{")
                        {
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue != "}")//need to make sure there is at least one statement
                            {
                                CompileStatements(spacing,classFrom);
                                AdvanceParseCount();
                            }
                            vmFileMng.Goto(classFrom + ".end." + uniqueIfId);
                            if (currentToken.tokenValue == "}")
                            {
                                vmFileMng.Lable(classFrom + ".else." + uniqueIfId);
                                WriteTerminalToken(spacing);
                                if (nextToken != null&&nextToken.tokenValue == "else")
                                {
                                    AdvanceParseCount();
                                    WriteTerminalToken(spacing);
                                    AdvanceParseCount();
                                    if (currentToken.tokenValue == "{")
                                    {
                                        WriteTerminalToken(spacing);
                                        AdvanceParseCount();
                                        if (currentToken.tokenValue != "}")//need to make sure there is at least one statement
                                        {
                                            CompileStatements(spacing,classFrom);
                                            AdvanceParseCount();
                                        }
                                        if (currentToken.tokenValue == "}")
                                        {
                                            WriteTerminalToken(spacing);
                                            WriteToConsolXml("</ifStatement>",spacing);
                                        }
                                        else
                                        {
                                            Console.WriteLine("CompileIfStatement expected '}'");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("CompileIfStatement expected '{'");
                                    }
                                }
                                else
                                {
                                    WriteToConsolXml("</ifStatement>",spacing);
                                }
                                vmFileMng.Lable(classFrom + ".end." + uniqueIfId);
                            }
                            else
                            {
                                Console.WriteLine("CompileIfStatement expected '}'");
                            }
                        }
                        else
                        {
                            Console.WriteLine("CompileIfStatement expected '{'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompileIfStatement expected ')'");
                    }
                }
                else
                {
                    Console.WriteLine("CompileIfStatement expected '('");
                }
            }
            else
            {
                Console.WriteLine("CompileIfStatement expected 'if' keyword ");
            }
        }
        void CompileWhileStatement(int spacing, string classFrom)
        {
            spacing++;
            WriteToConsolXml("<whileStatement>",spacing);
            if (currentToken.tokenValue == "while")
            {
                
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                int uniqueIfId = GetUniqueIdentifier();
                vmFileMng.Lable(classFrom + ".whileStart." + uniqueIfId);
                if (currentToken.tokenValue == "(")
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    CompileExpression(spacing,classFrom);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ")")
                    {
                        
                        vmFileMng.BasicOp(VmFileManager.OpE.not);
                        vmFileMng.IfGoto(classFrom + ".end." + uniqueIfId);
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == "{")
                        {
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue != "}")
                            {
                                CompileStatements(spacing,classFrom);
                                AdvanceParseCount();
                            }
                            
                            if (currentToken.tokenValue == "}")
                            {
                                vmFileMng.Goto(classFrom + ".whileStart." + uniqueIfId);
                                vmFileMng.Lable(classFrom + ".end." + uniqueIfId);
                                WriteTerminalToken(spacing);
                                WriteToConsolXml("</whileStatement>",spacing);
                            }
                            else
                            {
                                Console.WriteLine("CompileWhileStatement expected '}'");
                            }
                        }
                        else
                        {
                            Console.WriteLine("CompileWhileStatement expected '{'");
                        }
                    }
                    else
                    {
                        Console.WriteLine("CompileWhileStatement expected ')'");
                    }
                }
                else
                {
                    Console.WriteLine("CompileWhileStatement expected '('");
                }
            }
            else
            {
                Console.WriteLine("CompileWhileStatement expected 'while' keyword ");
            }
        }
        void CompileReturnStatement(int spacing,string classFrom)
        {
            spacing++;
            WriteToConsolXml("<returnStatement>",spacing);
            if (currentToken.tokenValue == "return")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                if (currentToken.tokenValue == ";")
                {
                    WriteTerminalToken(spacing);
                    WriteToConsolXml("</returnStatement>",spacing);
                    vmFileMng.Push(VmFileManager.MemoryE.constant, 0);
                    vmFileMng.Return();
                }
                else
                {
                    CompileExpression(spacing,classFrom);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ";")
                    {
                        vmFileMng.Return();
                        WriteTerminalToken(spacing);
                        WriteToConsolXml("</returnStatement>",spacing);
                    }
                    else
                    {
                        Console.WriteLine("CompileReturnStatement expected ';' ");
                    }
                }
               
            }
            else
            {
                Console.WriteLine("CompileReturnStatement expected 'return' keyword ");
            }
        }
        void CompileSubroutineCall(int spacing,string classFrom)
        {
            spacing++;
            
            WriteToConsolXml("<subroutineCall>",spacing);
            if (currentToken.tokenType == Token.Type.identifier)
            {

                WriteTerminalToken(spacing);
                string firstIdentifierSubroutine = currentToken.tokenValue;
                string secondIdentifierSubroutine ="";
                if (nextToken != null && nextToken.tokenValue == ".") //first identifier is a class or object
                {
                   
                    AdvanceParseCount();
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenType == Token.Type.identifier)
                    {
                        secondIdentifierSubroutine = currentToken.tokenValue;
                        WriteTerminalToken(spacing);
                    }
                    else
                    {
                        Console.WriteLine("expected identifier ");
                    }
                }
                else
                {
                    //first identifier is methode
                }
                AdvanceParseCount();

                if (secondIdentifierSubroutine == "")
                {
                    vmFileMng.Push(VmFileManager.MemoryE.pointer, 0);
                }
                else
                {
                    Symbol representedVar = FindVariableSymbol(firstIdentifierSubroutine);
                    if (representedVar != null)
                    {
                        vmFileMng.Push(representedVar.kindE, representedVar.pos);
                    }
                }

                if (currentToken.tokenValue == "(")
                {
                    int qArguments = 0;
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                        qArguments = 0;
                        WriteToConsolXml("</subroutineCall>",spacing);
                    }
                    else
                    {
                         qArguments = CompileExpressionList(spacing,classFrom);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == ")")
                        {
                            WriteTerminalToken(spacing);
                            
                            WriteToConsolXml("</subroutineCall>",spacing);
                        }
                        else
                        {
                            Console.WriteLine("CompileSubroutineCall expected ')' ");
                        }
                    }
                    if(secondIdentifierSubroutine=="")
                    {
                        vmFileMng.CallSubroutine(classFrom, firstIdentifierSubroutine, qArguments+1);
                    }
                    else
                    {
                        Symbol representedVar = FindVariableSymbol(firstIdentifierSubroutine);
                        if (representedVar != null)
                        {
                            vmFileMng.CallSubroutine(representedVar.type, secondIdentifierSubroutine, qArguments+1);
                        }
                        else
                        {
                            vmFileMng.CallSubroutine(firstIdentifierSubroutine, secondIdentifierSubroutine, qArguments);
                        }
                       
                    }

                   
                }
                else
                {
                    Console.WriteLine("CompileSubroutineCall expected '(' ");
                }
            }
            else
            {
                Console.WriteLine("CompileSubroutineCall expected identifier ");
            }
        }
        int CompileExpressionList(int spacing, string classFrom)
        {
            spacing++;
            WriteToConsolXml("<expressionList>", spacing);
            int quantityOfExpressions = 0;
            while(true)
            {
                if(GetIsPotentialTerm(currentToken))//expression always start with termes
                {
                    CompileExpression(spacing,classFrom);
                    quantityOfExpressions++;
                    if (nextToken!=null&& nextToken.tokenValue==",")
                    {
                        AdvanceParseCount();
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                    }
                    else
                    {
                        WriteToConsolXml("</expressionList>", spacing);
                        break;
                    }
                }
                else
                {
                    WriteToConsolXml("</expressionList>", spacing);//empty expression list shouldn't have to be dealing with this?
                   // Console.WriteLine("expressionList expected term or expression");
                    break;
                }
            }
            return quantityOfExpressions;
        }
        void CompileExpression(int spacing, string classFrom)
        {
            spacing++;
            WriteToConsolXml("<expression>", spacing);
            string currentOperation ="";
            while (true)
            {
                if (GetIsPotentialTerm(currentToken))
                {
                    CompileTerme(spacing,classFrom);
                    if(currentOperation!="")
                    {

                        if (currentOperation == "/")
                        {
                            vmFileMng.CallSubroutine("Math", "divide",2);
                        }
                        else
                        if(currentOperation=="*")
                        {
                            vmFileMng.CallSubroutine("Math", "multiply",2);
                        }
                        else
                        {
                            vmFileMng.BasicOp(currentOperation, true);
                        }
                        
                    }
                    if (nextToken!=null&&IsOp(nextToken.tokenValue))
                    {
                        AdvanceParseCount();
                        currentOperation = currentToken.tokenValue;
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                    }
                    else
                    {
                        WriteToConsolXml("</expression>", spacing);
                        break;
                    }
                }
                else
                {
                    WriteToConsolXml("</expression>", spacing);
                    Console.WriteLine("CompileExpression expected identifier,const int or const string");
                    break;
                }
            }
        }
        void CompileTerme(int spacing, string classFrom)
        {
            spacing++;
            WriteToConsolXml("<term>", spacing);
            if (GetIsPotentialTerm(currentToken))
            {
                //term is affected by a operation
                string unarryOp = "";
                if (IsUnaryOp(currentToken.tokenValue))
                {
                    WriteTerminalToken(spacing);
                    unarryOp = currentToken.tokenValue;
                    AdvanceParseCount();
                }
                
                //Expression term
                if (currentToken.tokenValue =="(")
                {
                    AdvanceParseCount();
                    CompileExpression(spacing,classFrom);
                    AdvanceParseCount();
                    if(currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                    }
                    else
                    {
                        Console.WriteLine("CompileTerme expected ')'");
                    }
                    
                }
                else//Subroutine terme
                if (currentToken.tokenType== Token.Type.identifier&&nextToken!=null&&(nextToken.tokenValue=="."|| nextToken.tokenValue =="(") )
                {
                    CompileSubroutineCall(spacing,classFrom);
                    
                }
                else //identifier or constant 
                {
                    //Console.WriteLine("term dealing with: " + currentToken.tokenValue);
                    if(currentToken.tokenValue == "null")
                    {
                        vmFileMng.Push(VmFileManager.MemoryE.constant, 0);
                    }
                    else
                    if (currentToken.tokenValue == "this")
                    {
                        vmFileMng.Push(VmFileManager.MemoryE.pointer, 0);
                    }
                    else
                    if (currentToken.tokenValue == "true")
                    {
                        vmFileMng.Push(VmFileManager.MemoryE.constant, 1);
                        vmFileMng.BasicOp(VmFileManager.OpE.neg);
                    }
                    else
                    if (currentToken.tokenValue == "false")
                    {
                        vmFileMng.Push(VmFileManager.MemoryE.constant, 0);
                    }
                    else
                    if (currentToken.tokenType == Token.Type.identifier)
                    {
                        Symbol variableSym = FindVariableSymbol(currentToken.tokenValue);
                        vmFileMng.Push(variableSym.kindE, variableSym.pos);
                    }
                    else
                    if (currentToken.tokenType == Token.Type.integerConstant)
                    {
                        vmFileMng.Push(VmFileManager.MemoryE.constant, int.Parse(currentToken.tokenValue));
                    }
                    else
                    if (currentToken.tokenType == Token.Type.StringConstant)
                    {
                        vmFileMng.Push(VmFileManager.MemoryE.constant, currentToken.tokenValue.Length);
                        vmFileMng.CallSubroutine("String", "new", 1);
                    }
                    else
                    {
                        Console.WriteLine("CompileTerme expected identifier intiger constant or string constant ");
                    }
                    WriteTerminalToken(spacing);
                }

                //Term is array
                if (nextToken != null && nextToken.tokenValue == "[") 
                {
                        AdvanceParseCount();
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        CompileExpression(spacing,classFrom);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == "]")
                        {
                            vmFileMng.BasicOp(VmFileManager.OpE.add);
                            vmFileMng.Pop(VmFileManager.MemoryE.pointer, 1);
                            vmFileMng.Push(VmFileManager.MemoryE.that, 0);
                            WriteTerminalToken(spacing);
                        }
                }
                
                if (unarryOp != "")
                {
                    vmFileMng.BasicOp(unarryOp, false);
                }
                WriteToConsolXml("</term>", spacing);
            }
            else
            {
                Console.WriteLine("CompileTerme expected term");
            }
        }
        void WriteTerminalToken(int spacing)
        {
            if (streamXml != null)
            {
                spacing++;
                WriteToConsolXml("<" + currentToken.tokenType.ToString() + ">", spacing, false);
                if (currentToken.tokenValue == ">")
                {
                    WriteToConsolXml("_GT_", 0, false);
                }
                else
                if (currentToken.tokenValue == "<")
                {
                    WriteToConsolXml("_LT_", 0, false);
                }
                else
                if (currentToken.tokenValue == "&")
                {
                    WriteToConsolXml("_AND_", 0, false);
                }
                else
                {
                    WriteToConsolXml(currentToken.tokenValue, 0, false);
                }

                WriteToConsolXml("</" + currentToken.tokenType.ToString() + ">", 0, false);
                streamXml.WriteLine();
            }
        }
        string CreatStringWithXTabs(int x)
        {
            
            string str = "";
            for(int i =0; i < x;i++)
            {
                str += "\t";
            }
            return str;
        }
        void WriteToConsolXml(string content,int tabs,bool nextLine= true)
        {
            if (streamXml != null)
            {
                if (nextLine)
                {
                    streamXml.WriteLine(CreatStringWithXTabs(tabs) + content);
                }
                else
                {
                    streamXml.Write(CreatStringWithXTabs(tabs) + content);
                }
            }
           
        }
        bool IsUnaryOp(string str)
        {
            foreach (char chr in unaryOperations)
            {
                if ((chr + "") == str)
                {
                    return true;
                }
            }
            return false;
        }
        bool IsOp(string str)
        {
            foreach (char chr in operations)
            {
                if ((chr + "") == str)
                {
                    return true;
                }
            }
            return false;
        }
        bool GetIsPotentialTerm(Token tkn)
        {
            if(tkn.tokenValue=="this")
            {
                return true;
            }
            if (tkn.tokenValue == "true" || tkn.tokenValue == "false")
            {
                return true;
            }
            if (tkn.tokenValue == "(")
            {
                return true;
            }
            if (tkn.tokenValue=="null")
            {
                return true;
            }
            foreach (char chr in unaryOperations)
            {
                if((chr+"")== tkn.tokenValue)
                {
                    return true;
                }
            }
            return tkn.tokenType== Token.Type.identifier
                || tkn.tokenType == Token.Type.integerConstant
                || tkn.tokenType == Token.Type.StringConstant;
        }
        bool GetIsStatement(Token tkn)
        {
            foreach(string currentStatement in statments)
            {
                if(tkn.tokenValue== currentStatement)
                {
                    return true;
                }
            }
            return false;
        }
        void AdvanceParseCount()
        {
            parseCount++;
            if(parseCount < allTokens.Count)
            {
                currentToken = allTokens[parseCount];
            }
            else
            {
                currentToken = null;
            }
            if(parseCount+1 < allTokens.Count)
            {
                nextToken = allTokens[parseCount + 1];
            }
            else
            {
                nextToken = null;
            }
        }
        int GetUniqueIdentifier()
        {
            uniqueIdentifier++;
            return uniqueIdentifier-1;
        }
        Symbol FindVariableSymbol(string varName)
        {

            Symbol current = subroutineLevelSymTable.GetSymbol(varName);
            if (current == null)
            {
                current = classLevelSymTable.GetSymbol(varName);
            }
            if(current== null)
            {
               // subroutineLevelSymTable.PrintAllVariables();
            }
            return current;
        }
        int FindPositionOfVariable(string varName)
        {
            return FindVariableSymbol(varName).pos;
        }
       
    }
}
