// This file is part of www.nand2tetris.org
// and the book "The Elements of Computing Systems"
// by Nisan and Schocken, MIT Press.
// File name: projects/04/Fill.asm

// Runs an infinite loop that listens to the keyboard input.
// When a key is pressed (any key), the program blackens the screen,
// i.e. writes "black" in every pixel;
// the screen should remain fully black as long as the key is pressed. 
// When no key is pressed, the program clears the screen, i.e. writes
// "white" in every pixel;
// the screen should remain fully clear as long as no key is pressed.

// Put your code here.
//screen registers = 8192
//Do
//{
//  counter = 0;
//  Do
//  {
//      if(keypard!=0)
//      {
//         [screen+counter]=-1
//      }
//      else
//      {
//         [screen+counter]=0
//      }
//      
//      counter= counter+1;
//  }while(counter -screenRegisters<0)
//}while(always)



//Do
(START)
//  counter = 0;
@counter
M=0
//Do color screen loop 
(COLORSCREEN)

    //if(keypard==0)
    @KBD
    D=M
    @ELSE
    D;JEQ
    //condition is true, whiten screen
        @counter
        D=M
        @SCREEN
        A=A+D
        M=-1 
    //condition is true end  
    @ENDIF
    0;JMP
    (ELSE)
    //condition is false,blacken screen
        @counter
        D=M
        @SCREEN
        A=A+D
        M=0
    //condition is false end
    (ENDIF)
    //counter= counter+1;
    @counter
    M=M+1

//while(counter -screenRegisters<0)
@8192
D=A
@counter
D=M-D
@COLORSCREEN
D;JLT
//end while condition

//restart main loop
@START
0;JMP