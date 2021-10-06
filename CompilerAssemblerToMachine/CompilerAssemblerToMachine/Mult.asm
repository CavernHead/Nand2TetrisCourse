// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Mult.asm

// Multiplies R0 and R1 and stores the result in R2.
// (R0, R1, R2 refer to RAM[0], RAM[1], and RAM[2], respectively.)
//
// This program only needs to handle arguments that satisfy
// R0 >= 0, R1 >= 0, and R0*R1 < 32768.

// Put your code here.

//pseudocode
//R0 =1;
//R1 =0;
//R2 =0;
//multCount = 0;
//if(R1!=0)
//Do
//{
//   R2=R2+R0;
//   multCount=multCount+1;
//}while(multCount-R1<0)

//R2 =0;
@R2
M=0
//multCount = 0;
@multCount
M=0

//if(R0!=0)
@R1
D=M
@END
D;JEQ
//Do
(MULT)
// R2=R2+R0;
@R0
D=M
@R2
M=D+M
//multCount=multCount+1;
@multCount
M=M+1
D=M
//While(multCount-R1<0)
@R1
D=D-M //(D=mulcount M=R1)

@MULT
D;JLT

(END)
@END
0;JEQ



