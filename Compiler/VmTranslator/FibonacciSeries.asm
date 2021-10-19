//////////0: push argument 1
@1
D=A
@ARG
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////1: pop pointer 1
@SP
M=M-1
A=M
D=M
@SP
@THAT
M=D
//////////2: push constant 0
@0
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////3: pop that 0
@0
D=A
@THAT
D=M+D
@SP
A=M
M=D
A=A-1
D=M
A=A+1
A=M
M=D
@SP
M=M-1
//////////4: push constant 1
@1
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////5: pop that 1
@1
D=A
@THAT
D=M+D
@SP
A=M
M=D
A=A-1
D=M
A=A+1
A=M
M=D
@SP
M=M-1
//////////6: push argument 0
@0
D=A
@ARG
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////7: push constant 2
@2
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////8: sub  
@SP
M=M-1
A=M-1
D=M
@SP
A=M
D=D-M
@SP
M=M-1
@SP
A=M
M=D
@SP
M=M+1
//////////9: pop argument 0
@0
D=A
@ARG
D=M+D
@SP
A=M
M=D
A=A-1
D=M
A=A+1
A=M
M=D
@SP
M=M-1
//////////10: label MAIN_LOOP_START 
(MAIN_LOOP_START$bar)
//////////11: push argument 0
@0
D=A
@ARG
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////12: if-goto COMPUTE_ELEMENT 
@SP
M=M-1
A=M
D=M
@COMPUTE_ELEMENT$bar
D;JNE
//////////13: goto END_PROGRAM 
@END_PROGRAM$bar
0;JMP
//////////14: label COMPUTE_ELEMENT 
(COMPUTE_ELEMENT$bar)
//////////15: push that 0
@0
D=A
@THAT
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////16: push that 1
@1
D=A
@THAT
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////17: add  
@SP
M=M-1
A=M-1
D=M
@SP
A=M
D=D+M
@SP
M=M-1
@SP
A=M
M=D
@SP
M=M+1
//////////18: pop that 2
@2
D=A
@THAT
D=M+D
@SP
A=M
M=D
A=A-1
D=M
A=A+1
A=M
M=D
@SP
M=M-1
//////////19: push pointer 1
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////20: push constant 1
@1
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////21: add  
@SP
M=M-1
A=M-1
D=M
@SP
A=M
D=D+M
@SP
M=M-1
@SP
A=M
M=D
@SP
M=M+1
//////////22: pop pointer 1
@SP
M=M-1
A=M
D=M
@SP
@THAT
M=D
//////////23: push argument 0
@0
D=A
@ARG
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////24: push constant 1
@1
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////25: sub  
@SP
M=M-1
A=M-1
D=M
@SP
A=M
D=D-M
@SP
M=M-1
@SP
A=M
M=D
@SP
M=M+1
//////////26: pop argument 0
@0
D=A
@ARG
D=M+D
@SP
A=M
M=D
A=A-1
D=M
A=A+1
A=M
M=D
@SP
M=M-1
//////////27: goto MAIN_LOOP_START 
@MAIN_LOOP_START$bar
0;JMP
//////////28: label END_PROGRAM 
(END_PROGRAM$bar)
