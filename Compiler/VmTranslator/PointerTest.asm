//////////0: push constant 3030
@3030
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////1: pop pointer 0
@SP
M=M-1
A=M
D=M
@SP
@THIS
M=D
//////////2: push constant 3040
@3040
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////3: pop pointer 1
@SP
M=M-1
A=M
D=M
@SP
@THAT
M=D
//////////4: push constant 32
@32
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////5: pop this 2
@2
D=A
@THIS
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
//////////6: push constant 46
@46
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////7: pop that 6
@6
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
//////////8: push pointer 0
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////9: push pointer 1
@THAT
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
//////////11: push this 2
@2
D=A
@THIS
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////12: sub  
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
//////////13: push that 6
@6
D=A
@THAT
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////14: add  
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