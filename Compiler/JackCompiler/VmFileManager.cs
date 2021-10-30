using System;
using System.IO;
using System.Collections.Generic;
using JackCompiler;


namespace JackCompiler
{
    class VmFileManager
    {
        StreamWriter streamVm;

        public enum MemoryE
        {
            local,
            argument,
            thisE,
            that,
            staticE,
            constant,
            temp,
            pointer,
        }
        public enum OpE
        {
            add,
            sub,
            neg,
            eq,
            gt,
            lt,
            and,
            or,
            not
        }
        public VmFileManager(string fileName, StreamWriter str)
        {
            streamVm = str;
            
        }
        void doWhenWriteToFile()
        {
            streamVm.Flush();
        }
        public void Push(MemoryE memtyp,int varPos)
        {
            streamVm.WriteLine("push " + getStringMem(memtyp) + " " + varPos);
            doWhenWriteToFile();
        }
        public void Pop(MemoryE memtyp, int varPos)
        {
            streamVm.WriteLine("pop " + getStringMem(memtyp) + " " + varPos);
            doWhenWriteToFile();
        }
        public void Pop(Symbol.kind varKind,int varPos)
        {
            if (varKind == Symbol.kind.field)
            {
               Pop(VmFileManager.MemoryE.thisE, varPos); //doesn't deal with arrays yet
            }
            else
            if (varKind == Symbol.kind.var)
            {
                Pop(VmFileManager.MemoryE.local, varPos);
            }
            else
            if (varKind == Symbol.kind.staticE)
            {
                Pop(VmFileManager.MemoryE.staticE, varPos);
            }
            else
            if (varKind == Symbol.kind.arg)
            {
                Pop(VmFileManager.MemoryE.argument, varPos);
            }
        }
        public void Push(Symbol.kind varKind, int varPos)
        {
            if (varKind == Symbol.kind.field)
            {
                Push(VmFileManager.MemoryE.thisE, varPos); //doesn't deal with arrays yet
            }
            else
            if (varKind == Symbol.kind.var)
            {
                Push(VmFileManager.MemoryE.local, varPos);
            }
            else
            if (varKind == Symbol.kind.staticE)
            {
                Push(VmFileManager.MemoryE.staticE, varPos);
            }
            else
            if (varKind == Symbol.kind.arg)
            {
                Push(VmFileManager.MemoryE.argument, varPos);
            }
        }
        public void BasicOp(OpE op)
        {
            streamVm.WriteLine(op.ToString());
        }
        public void BasicOp(string str,bool hBarIsSub)
        {
           
            switch(str)
            {
                   case "+":
                BasicOp(OpE.add);
                    break;
                case "-":
                    {
                        if(hBarIsSub)
                        {
                            BasicOp(OpE.sub);
                        }
                        else
                        {
                            BasicOp(OpE.neg);
                        }
                    }
                    break;
                case "&":
                 BasicOp(OpE.and);
                    break;
                case "|":
                    BasicOp(OpE.or);
                    break;
                case "<":
                    BasicOp(OpE.lt);
                    break;
                case ">":
                    BasicOp(OpE.gt);
                    break;
                case "=":
                    BasicOp(OpE.eq);
                    break;
                case "~":
                    BasicOp(OpE.not);
                    break;
            }
        }
        public void Lable(string lblName)
        {
            streamVm.WriteLine("label " + lblName);
            doWhenWriteToFile();
        }
        public void Goto(string lblName)
        {
            streamVm.WriteLine("goto "+ lblName);
            doWhenWriteToFile();
        }
        public void IfGoto(string lblName)
        {
            streamVm.WriteLine("if-goto " + lblName);
            doWhenWriteToFile();
        }
        public void CallSubroutine(string className,string subName,int numberOfArguments)
        {
            streamVm.WriteLine("call "+ className+"."+subName+" "+numberOfArguments);
            doWhenWriteToFile();
        }
        public void DeclareSubroutine(string className,string subName, int numberOfLocals)
        {
            streamVm.WriteLine("function "+ className+"."+subName + " "+numberOfLocals);
            doWhenWriteToFile();
        }
        public void Return()
        {
            streamVm.WriteLine("return");
        }
       

        string getStringMem(MemoryE memtyp)
        {
            string memTypStr = memtyp.ToString();
            if (memtyp == MemoryE.thisE)
            {
                memTypStr = "this";
            }
            if(memtyp == MemoryE.staticE)
            {
                memTypStr = "static";
            }
            return memTypStr;
        }

        public void Close()
        {
            //streamVm.Close();
        }
    }
}
