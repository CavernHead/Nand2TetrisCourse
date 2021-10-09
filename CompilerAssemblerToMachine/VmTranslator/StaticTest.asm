//////////0: push constant 111
@111
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////1: push constant 333
@333
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////2: push constant 888
@888
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////3: pop static 8
@SP
M=M-1
A=M
D=M
@SP
@static8
M=D
//////////4: pop static 3
@SP
M=M-1
A=M
D=M
@SP
@static3
M=D
//////////5: pop static 1
@SP
M=M-1
A=M
D=M
@SP
@static1
M=D
//////////6: push static 3
@static3
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////7: push static 1
@static1
D=M
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
//////////9: push static 8
@static8
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////10: add  
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
