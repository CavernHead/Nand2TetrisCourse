using System;
using System.IO;
using System.Collections.Generic;
using JackCompiler;

namespace JackCompiler
{
    class SymbolTable
    {
        List<Symbol> symbolList;

       public int qVar;
       public int qFields;
       public int qArg;
       public int qStatics;
        
        public SymbolTable()
        {
            symbolList = new List<Symbol>();
            qVar = 0;
            qFields = 0;
            qArg = 0;
        }
        public void AddSymbol(string nameP, string typeP, Symbol.kind kindP)
        {
            AddSymbol(new Symbol(nameP, typeP, kindP,0));
        }
        public void PrintAllVariables()
        {
            Console.WriteLine("showing all symboles in symbol table");
            foreach(Symbol current in symbolList)
            {
                Console.WriteLine(current.GetSring());
            }
        }
        public void AddSymbol(Symbol symp)
        {
            Symbol checkSym = GetSymbol(symp.name);
           
            if (checkSym == null)
            {
                symbolList.Add(symp);
                
                if (symp.kindE== Symbol.kind.field)
                {
                    symp.pos = qFields;
                    qFields++;
                }

                if (symp.kindE == Symbol.kind.var)
                {
                    symp.pos = qVar;
                    qVar++;
                }
                if (symp.kindE == Symbol.kind.arg)
                {
                    symp.pos = qArg;
                    qArg++;
                }
                if(symp.kindE == Symbol.kind.staticE)
                {
                    symp.pos = qStatics;
                    qStatics++;
                }
                //Console.WriteLine(symp.GetSring());

            }
            else
            {
                Console.WriteLine("compile error two times the same variable name used");
            }

        }
        public Symbol GetSymbol(string nameP)
        { 

     //   Console.WriteLine("looking for variable:"+ nameP);
            foreach(Symbol current in symbolList)
            {
                if(current.name == nameP)
                {
                    return current;
                }
            }
            return null;
        }
    }
    class Symbol
    {
        public enum kind
        {
            staticE,
            field,
            var,
            arg,
        }
        public string name;
        public string type;
        public kind kindE;
        public int pos;
        public Symbol()
        {
        }
        public string GetSring()
        {
            return "name:"+name + " type:" + type + " kind:" + kindE.ToString() + " pos:" + pos;
        }
        public Symbol(string nameP, string typeP, kind kindP, int posP)
        {
                name = nameP;
                type = typeP;
                kindE = kindP;
                 pos = posP;
        }
    }
}
