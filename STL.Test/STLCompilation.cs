using Antlr4.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using STL.Grammar;
using System;

namespace STL.Test
{
    [TestClass]
    public class STLCompilation
    {

        private TestContext m_testContext;

        public TestContext TestContext

        {

            get { return m_testContext; }

            set { m_testContext = value; }

        }

        [TestMethod]
        public void ProgramAssign()
        {
            string source = @"PROGRAM pippo

                                VAR
                                x:BOOL;
                                END_VAR

                                x:=1;

                                END_PROGRAM;";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramAssignString()
        {
            string source = @"PROGRAM pippo

                                VAR
                                x:BOOL;
                                END_VAR

                                x:='hello world';

                                END_PROGRAM;";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramAssignCompositeID()
        {
            string source = @"PROGRAM pippo

                                VAR
                                x:BOOL;
                                END_VAR

                                x.y:='hello world';

                                END_PROGRAM;";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }


        [TestMethod]
        public void ProgramIf()
        {
            string source = @"PROGRAM stexample  
                            VAR     OUTPUT1 : BOOL;   END_VAR 
                                OUTPUT1 := TRUE;   
                                
                                IF INPUT1 = TRUE THEN           
                                    OUTPUT1 := TRUE; 
                                END_IF;
 
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }


        [TestMethod]
        public void ProgramIfElse()
        {
            string source = @"PROGRAM stexample  
                            VAR     OUTPUT1 : BOOL;   END_VAR 
                                OUTPUT1 := TRUE;   
                                
                                IF INPUT1 = TRUE THEN           
                                    OUTPUT1 := TRUE; 
                                ELSE
                                    OUTPUT1 := FALSE;
                                END_IF;
 
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramIfElseIf()
        {
            string source = @"PROGRAM stexample  
                            VAR     OUTPUT1 : BOOL;   END_VAR 
                                OUTPUT1 := TRUE;   
                                
                                IF INPUT1 = TRUE THEN           
                                    OUTPUT1 := TRUE; 
                                ELSEIF INPUT1 = FALSE THEN
                                    OUTPUT1 := FALSE;
                                END_IF;
 
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramIfElseIfMulti()
        {
            string source = @"PROGRAM stexample  
                            VAR     OUTPUT1 : INTEGER;   END_VAR 
                                OUTPUT1 := TRUE;   
                                
                                IF INPUT1 = 1 THEN           
                                    OUTPUT1 := TRUE; 
                                ELSEIF INPUT1 = 2 THEN
                                    OUTPUT1 := FALSE;
                                ELSEIF INPUT1 = 3 THEN
                                    OUTPUT1 := FALSE;
                                ELSE
                                    OUTPUT1 := FALSE;
                                END_IF;
 
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramRepeat()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                x := TRUE;   
                                REPEAT    
                                    x := FALSE;   
                                UNTIL x = FALSE  END_REPEAT; 
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void FunctionBlock()
        {
            string source = @"FUNCTION_BLOCK stexample  
                            VAR     x : BOOL;   END_VAR 
                                x := TRUE;   
                                REPEAT    
                                    x := FALSE;   
                                UNTIL x = FALSE  END_REPEAT; 
                            END_FUNCTION_BLOCK;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramFunctionCallNoArgs()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                
                                f();

                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramFunctionCall()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                
                                f(0);

                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramFunctionCall1()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                
                                f(0,1);

                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

        [TestMethod]
        public void ProgramFunctionCall2()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                
                                f(x,y);

                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }


        [TestMethod]
        public void ProgramFunctionCall3()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                
                                f(x:=10, y:= 20);

                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramFunctionCallAssign()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                
                                x:= f(x:=10, y:= 20);

                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramCase()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                x := TRUE;   
                                CASE y OF
                                    1: x:=1;
                                    2: y:=2;
                                END_CASE;
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramCaseElse()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                x := TRUE;   
                                CASE y OF
                                    1: x:=1;
                                    2: y:=2;
                                ELSE
                                    x:= 0;                            
                                END_CASE;
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramForLoop()
        {
            string source = @"PROGRAM stexample  
                            VAR     x,y : INTEGER;   END_VAR 
                                FOR x:=0 TO 10 DO
                                    y:= y+1;
                                END_FOR;
                            END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramWhile()
        {
            string source = @"PROGRAM stexample  
                            VAR     x,y : INTEGER;   END_VAR 
                                WHILE Y < 10 DO
                                    y:= y+1;
                                END_WHILE;
                            END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramForLoopWithBreak()
        {
            string source = @"PROGRAM stexample  
                            VAR     x,y : INTEGER;   END_VAR 
                                FOR x:=0 TO 10 DO
                                    y:= y+1;
                                    BREAK;
                                END_FOR;
                            END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramWhileWithBreak()
        {
            string source = @"PROGRAM stexample  
                            VAR     x,y : INTEGER;   END_VAR 
                                WHILE Y < 10 DO
                                    y:= y+1;
                                    BREAK;
                                END_WHILE;
                            END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            var result = compiler.Compile(source);

            foreach (var x in compiler.Errors)
            {
                TestContext.WriteLine(String.Format("{0} {1} {2}", x.Line, x.Column, x.Message));
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ProgramRepeatWithBreak()
        {
            string source = @"PROGRAM stexample  
                            VAR     x : BOOL;   END_VAR 
                                x := TRUE;   
                                REPEAT    
                                    x := FALSE;   
                                    BREAK;
                                UNTIL x = FALSE  END_REPEAT; 
                                END_PROGRAM;   ";
            STCompiler compiler = new STCompiler();
            Assert.IsTrue(compiler.Compile(source));
        }

    }
}
