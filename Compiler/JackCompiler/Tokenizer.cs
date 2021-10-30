using System;
using System.IO;
using System.Collections.Generic;
using JackCompiler;

namespace JackCompiler
{
    public class Tokenizer
    {

        char[] symboles = { '{', '}', '(', ')', '[', ']', '.', ',', ';', '+', '-', '*', '/', '&', '|', '<', '>', '=', '~' };
        string[] keywords = {"class","constructor","function" ,
"method","field", "static", "var", "int","char","boolean","void","true","false",
            "null","this","let","do","if","else","while","return"};

        public List<Token> TokenizeFile(string fileNameFullNAme)
        {
            List<Token> allTokens = new List<Token>();
            if (File.Exists(fileNameFullNAme))
            {
                Console.WriteLine("found file");
                List<string> instructions = new List<string>();
                using (StreamReader streamReaderOfFile = File.OpenText(fileNameFullNAme))
                {
                    string currentLine = "";
                    bool wideComment = false;
                    while ((currentLine = streamReaderOfFile.ReadLine()) != null)
                    {
                       
                        string currentToken = "";
                        bool expectedTokenIsStringConst = false;

                        for (int i = 0; i < currentLine.Length; i++)
                        {
                            if (currentLine[i] == '*'&& i+1< currentLine.Length && currentLine[i+1]=='/' && wideComment)
                            {
                                 i++;
                                 wideComment = false;
                            }
                            else
                            if (currentLine[i] == '/' && i + 1 < currentLine.Length && currentLine[i + 1] == '/')
                            { 
                                break;
                            }
                            else
                            if (currentLine[i] == '/' && i + 1 < currentLine.Length && currentLine[i + 1] == '*')
                            {
                                i++;
                                expectedTokenIsStringConst = false;
                                wideComment = true;
                            }
                            else
                            if (!wideComment)
                            {
                                if (currentLine[i] == '"')
                                {
                                    if (expectedTokenIsStringConst)
                                    {
                                        expectedTokenIsStringConst = false;
                                        allTokens.Add(new Token(currentToken, Token.Type.StringConstant));
                                        currentToken = "";
                                    }
                                    else
                                    {
                                        expectedTokenIsStringConst = true;
                                    }
                                }
                                else
                                if (expectedTokenIsStringConst)
                                {
                                    currentToken += currentLine[i];
                                }
                                else
                                {
                                    if (currentLine[i] == '\n' || currentLine[i] == ' ' || currentLine[i] == '\t')
                                    {
                                        AddTokenFromWord();
                                    }
                                    else
                                    {
                                        bool foundSymbol = false;
                                        foreach (char currentSymbole in symboles)
                                        {
                                            if (foundSymbol==false && currentLine[i] == currentSymbole)
                                            {
                                                AddTokenFromWord();
                                                allTokens.Add(new Token("" + currentSymbole, Token.Type.symbol));
                                                foundSymbol = true;
                                                break;
                                            }
                                        }
                                        if (!foundSymbol)
                                        {
                                            currentToken += currentLine[i];
                                            if ((currentLine.Length - 1) == i)
                                            {
                                                AddTokenFromWord();
                                            }
                                        }
                                        
                                    }
                                }
                                void AddTokenFromWord()
                                {
                                    
                                    if (currentToken != "")
                                    {
                                        allTokens.Add(getTokenFromWord(currentToken));
                                        currentToken = "";
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
            
            return allTokens;
        }
        Token getTokenFromWord(string str)
        {
             foreach (string currentKeyWord in keywords)
             {
                 if (str == currentKeyWord)
                 {
                     return new Token(str, Token.Type.keyword);
                 }
             }
             if (MainRun.CheckIsNumeric(str))
             {
                 return new Token(str, Token.Type.integerConstant);
             }
             else
             {
                 return new Token(str, Token.Type.identifier);
             }
        }
    }
    public class Token
    {
        public string tokenValue;
        public Type tokenType;
        public enum Type
        {
          keyword,
          identifier,
          symbol,
          StringConstant,
          integerConstant
        }
        public Token(string tokenValueP, Type tokenTypeP)
        {
            tokenValue = tokenValueP;
            tokenType = tokenTypeP;

        }

    }
    
}