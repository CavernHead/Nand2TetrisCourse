using System;
using System.IO;
using System.Collections.Generic;
using JackCompiler;

namespace JackCompiler
{
    class Parser
    {
        List<Token> allTokens;
        StreamWriter stream;
        int parseCount;
        Token currentToken;
        Token nextToken;
        string[] statments = { "let","do","if","while","return" };
        char[] operations = {'+','-','*','/','&','|','<','>','='};
        char[] unaryOperations = { '-', '~' };
        public void ParseFile(List<Token> allTokensP, string fileName)
        {
            allTokens = allTokensP;
            parseCount = 0;
            if(allTokens.Count>0)
            {
                currentToken = allTokens[parseCount];
                stream = File.CreateText(fileName);
                if (currentToken.tokenValue == "class")
                {
                    CompileClass(0);
                }
                else
                {
                    Console.WriteLine("expected class keyword in file at start");
                }
                stream.Close();
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
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.identifier)
                {
                    WriteTerminalToken(spacing);
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
                                CompileSubroutineDec(spacing);
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
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.keyword|| currentToken.tokenType == Token.Type.identifier)//type
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    
                    while(true)
                    {
                        if (currentToken.tokenType == Token.Type.identifier) //variable name
                        {
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue == ",")
                            {
                                WriteTerminalToken(spacing);
                                AdvanceParseCount();
                            }
                            else
                            if (currentToken.tokenValue == ";")
                            {
                                WriteTerminalToken(spacing);
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
        void CompileSubroutineDec(int spacing)
        {
            spacing++;
            WriteToConsolXml("<subroutineDec>",spacing);
            if (currentToken.tokenValue == "function" || currentToken.tokenValue == "constructor" || currentToken.tokenValue == "method")
            {
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.keyword|| currentToken.tokenType == Token.Type.identifier)//return type
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenType == Token.Type.identifier)
                    {
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        if(currentToken.tokenValue == "(")
                        {
                            CompileParameterList(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue == "{")
                            {
                                WriteTerminalToken(spacing);
                                AdvanceParseCount();
                                CompileSubroutineBody(spacing);
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
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenType == Token.Type.identifier)
                            {
                                WriteTerminalToken(spacing);
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
        void CompileSubroutineBody(int spacing)
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
            CompileStatements(spacing);
            WriteToConsolXml("</subroutineBody>",spacing);

        }
        void CompileStatements(int spacing)
        {
            spacing++;
            WriteToConsolXml("<statements>",spacing);
            while (true)
            {
                if (currentToken.tokenValue == "let")
                {
                    CompileLetStatement(spacing);
                    if(nextToken!=null&&GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                    
                }
                else
                if (currentToken.tokenValue == "do")
                {
                    CompileDoStatement(spacing);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                if (currentToken.tokenValue == "if")
                {
                    CompileIfStatement(spacing);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                if (currentToken.tokenValue == "while")
                {
                    CompileWhileStatement(spacing);
                    if (nextToken != null && GetIsStatement(nextToken))
                    {
                        AdvanceParseCount();
                    }
                }
                else
                if (currentToken.tokenValue == "return")
                {
                    CompileReturnStatement(spacing);
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
        void CompileLetStatement(int spacing)
        {
            spacing++;
            WriteToConsolXml("<letStatement>",spacing);
            if(currentToken.tokenValue=="let")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                if(currentToken.tokenType == Token.Type.identifier)
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if(currentToken.tokenValue == "[")
                    {
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        CompileExpression(spacing);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == "]")
                        {
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
                        CompileExpression(spacing);
                        AdvanceParseCount();
                        if(currentToken.tokenValue == ";")
                        {
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
        void CompileDoStatement(int spacing)
        {
            spacing++;
            WriteToConsolXml("<doStatement>",spacing);
            if(currentToken.tokenValue == "do")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                CompileSubroutineCall(spacing);
                AdvanceParseCount();
                if(currentToken.tokenValue == ";")
                {
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
        void CompileIfStatement(int spacing)
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
                    CompileExpression(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        if(currentToken.tokenValue == "{")
                        {
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue != "}")//need to make sure there is at least one statement
                            {
                                CompileStatements(spacing);
                                AdvanceParseCount();
                            }
                            if (currentToken.tokenValue == "}")
                            {
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
                                            CompileStatements(spacing);
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
        void CompileWhileStatement(int spacing)
        {
            spacing++;
            WriteToConsolXml("<whileStatement>",spacing);
            if (currentToken.tokenValue == "while")
            {
                WriteTerminalToken(spacing);
                AdvanceParseCount();
                if (currentToken.tokenValue == "(")
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    CompileExpression(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == "{")
                        {
                            WriteTerminalToken(spacing);
                            AdvanceParseCount();
                            if (currentToken.tokenValue != "}")
                            {
                                CompileStatements(spacing);
                                AdvanceParseCount();
                            }
                            if (currentToken.tokenValue == "}")
                            {
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
        void CompileReturnStatement(int spacing)
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
                }
                else
                {
                    CompileExpression(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ";")
                    {
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
        void CompileSubroutineCall(int spacing)
        {
            spacing++;
            WriteToConsolXml("<subroutineCall>",spacing);
            if (currentToken.tokenType == Token.Type.identifier)
            {
                WriteTerminalToken(spacing);
                if (nextToken != null && nextToken.tokenValue == ".") //first identifier is a class 
                {
                    AdvanceParseCount();
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenType == Token.Type.identifier)
                    {
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
                if (currentToken.tokenValue == "(")
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                    if (currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                        WriteToConsolXml("</subroutineCall>",spacing);
                    }
                    else
                    {
                        CompileExpressionList(spacing);
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
        void CompileExpressionList(int spacing)
        {
            spacing++;
            WriteToConsolXml("<expressionList>", spacing);
            while(true)
            {
                if(GetIsPotentialTerm(currentToken))//expression always start with termes
                {
                    CompileExpression(spacing);
                    if(nextToken!=null&& nextToken.tokenValue==",")
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
                    Console.WriteLine("expressionList expected term or expression");
                    break;
                }
            }
        }
        void CompileExpression(int spacing)
        {
            spacing++;
            WriteToConsolXml("<expression>", spacing);
            while (true)
            {
                if (GetIsPotentialTerm(currentToken))
                {
                    CompileTerme(spacing);
                    if(nextToken!=null&&IsOp(nextToken.tokenValue))
                    {
                        AdvanceParseCount();
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
        void CompileTerme(int spacing)
        {
            spacing++;
            WriteToConsolXml("<term>", spacing);
            if (GetIsPotentialTerm(currentToken))
            {
                //term is affected by a operation
                if (IsUnaryOp(currentToken.tokenValue))
                {
                    WriteTerminalToken(spacing);
                    AdvanceParseCount();
                }
                bool isSubroutine =false;
                bool isExpression = false;
                //Expression term
                if (currentToken.tokenValue =="(")
                {
                    isExpression = true;
                    AdvanceParseCount();
                    CompileExpression(spacing);
                    AdvanceParseCount();
                    if(currentToken.tokenValue == ")")
                    {
                        WriteTerminalToken(spacing);
                    }
                    else
                    {
                        Console.WriteLine("CompileTerme expected ']'");
                    }
                    
                }
                else//Subroutine terme
                if (currentToken.tokenType== Token.Type.identifier&&nextToken!=null&&(nextToken.tokenValue=="."|| nextToken.tokenValue =="(") )
                {
                    CompileSubroutineCall(spacing);
                    isSubroutine = true;
                }
                //Term is array
                if (nextToken != null && nextToken.tokenValue == "[")
                {
                        AdvanceParseCount();
                        WriteTerminalToken(spacing);
                        AdvanceParseCount();
                        CompileExpression(spacing);
                        AdvanceParseCount();
                        if (currentToken.tokenValue == "]")
                        {
                            WriteTerminalToken(spacing);
                            WriteToConsolXml("</term>", spacing);
                        }
                        else
                        {
                            Console.WriteLine("CompileTerme expected ']'");
                        }
                }
                else
                {
                    if(!isSubroutine&&!isExpression)
                    {
                        WriteTerminalToken(spacing);
                    }
                    WriteToConsolXml("</term>", spacing);
                }
            }
            else
            {
                Console.WriteLine("CompileTerme expected term");
            }
        }
        void WriteTerminalToken(int spacing)
        {
            spacing++;
            WriteToConsolXml("<" + currentToken.tokenType.ToString() + ">", spacing,false);
            if (currentToken.tokenValue == ">")
            {
                WriteToConsolXml("_GT_",0, false);
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
            stream.WriteLine();
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
            if(nextLine)
            {
                stream.WriteLine(CreatStringWithXTabs(tabs) + content);
            }
            else
            {
                stream.Write(CreatStringWithXTabs(tabs) + content);
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
       
    }
}
