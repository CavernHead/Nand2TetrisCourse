@256
D=A
@SP
M=D
@Sys.init$ret.0
D=A
@SP
A=M
M=D
@SP
M=M+1
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1
@5
D=A
@SP
D=M-D
@ARG
M=D
@SP
D=M
@LCL
M=D
@Sys.init
0;JMP
(Sys.init$ret.0)
//////////1: function Sys.init 0
(Sys.init)
@SP
//////////2: push constant 4000
@4000
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////3: pop pointer 0
@SP
M=M-1
A=M
D=M
@SP
@THIS
M=D
//////////4: push constant 5000
@5000
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////5: pop pointer 1
@SP
M=M-1
A=M
D=M
@SP
@THAT
M=D
//////////6: call Sys.main 0
@Sys.main$ret.6
D=A
@SP
A=M
M=D
@SP
M=M+1
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1
@5
D=A
@SP
D=M-D
@ARG
M=D
@SP
D=M
@LCL
M=D
@Sys.main
0;JMP
(Sys.main$ret.6)
//////////7: pop temp 1
@SP
M=M-1
A=M
D=M
@SP
@6
M=D
//////////8: label LOOP 
(LOOP$bar)
//////////9: goto LOOP 
@LOOP$bar
0;JMP
//////////10: function Sys.main 5
(Sys.main)
@SP
A=M
M=0
@SP
M=M+1
A=M
M=0
@SP
M=M+1
A=M
M=0
@SP
M=M+1
A=M
M=0
@SP
M=M+1
A=M
M=0
@SP
M=M+1
//////////11: push constant 4001
@4001
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////12: pop pointer 0
@SP
M=M-1
A=M
D=M
@SP
@THIS
M=D
//////////13: push constant 5001
@5001
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////14: pop pointer 1
@SP
M=M-1
A=M
D=M
@SP
@THAT
M=D
//////////15: push constant 200
@200
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////16: pop local 1
@1
D=A
@LCL
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
//////////17: push constant 40
@40
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////18: pop local 2
@2
D=A
@LCL
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
//////////19: push constant 6
@6
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////20: pop local 3
@3
D=A
@LCL
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
//////////21: push constant 123
@123
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////22: call Sys.add12 1
@Sys.add12$ret.22
D=A
@SP
A=M
M=D
@SP
M=M+1
@LCL
D=M
@SP
A=M
M=D
@SP
M=M+1
@ARG
D=M
@SP
A=M
M=D
@SP
M=M+1
@THIS
D=M
@SP
A=M
M=D
@SP
M=M+1
@THAT
D=M
@SP
A=M
M=D
@SP
M=M+1
@6
D=A
@SP
D=M-D
@ARG
M=D
@SP
D=M
@LCL
M=D
@Sys.add12
0;JMP
(Sys.add12$ret.22)
//////////23: pop temp 0
@SP
M=M-1
A=M
D=M
@SP
@5
M=D
//////////24: push local 0
@0
D=A
@LCL
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////25: push local 1
@1
D=A
@LCL
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////26: push local 2
@2
D=A
@LCL
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////27: push local 3
@3
D=A
@LCL
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////28: push local 4
@4
D=A
@LCL
A=M+D
D=M
@SP
A=M
M=D
@SP
M=M+1
//////////29: add  
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
//////////30: add  
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
//////////31: add  
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
//////////32: add  
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
//////////33: return  
@ARG
D=M
@SP
A=M+1
M=D
@LCL
D=M
@SP
A=M
M=D
@3
D=A
@SP
A=M
A=M-D
D=M
@ARG
M=D
@2
D=A
@SP
A=M
A=M-D
D=M
@THIS
M=D
@1
D=A
@SP
A=M
A=M-D
D=M
@THAT
M=D
@4
D=A
@SP
A=M
A=M-D
D=M
@LCL
M=D
@5
D=A
@SP
A=M
A=M-D
D=M
@SP
A=M+1
A=M+1
M=D
@SP
A=M-1
D=M
@SP
A=M+1
A=M
M=D
@SP
A=M+1
D=M+1
@SP
M=D
A=M
A=M
0;JMP
//////////34: function Sys.add12 0
(Sys.add12)
@SP
//////////35: push constant 4002
@4002
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////36: pop pointer 0
@SP
M=M-1
A=M
D=M
@SP
@THIS
M=D
//////////37: push constant 5002
@5002
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////38: pop pointer 1
@SP
M=M-1
A=M
D=M
@SP
@THAT
M=D
//////////39: push argument 0
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
//////////40: push constant 12
@12
D=A
@SP
A=M
M=D
@SP
M=M+1
//////////41: add  
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
//////////42: return  
@ARG
D=M
@SP
A=M+1
M=D
@LCL
D=M
@SP
A=M
M=D
@3
D=A
@SP
A=M
A=M-D
D=M
@ARG
M=D
@2
D=A
@SP
A=M
A=M-D
D=M
@THIS
M=D
@1
D=A
@SP
A=M
A=M-D
D=M
@THAT
M=D
@4
D=A
@SP
A=M
A=M-D
D=M
@LCL
M=D
@5
D=A
@SP
A=M
A=M-D
D=M
@SP
A=M+1
A=M+1
M=D
@SP
A=M-1
D=M
@SP
A=M+1
A=M
M=D
@SP
A=M+1
D=M+1
@SP
M=D
A=M
A=M
0;JMP
