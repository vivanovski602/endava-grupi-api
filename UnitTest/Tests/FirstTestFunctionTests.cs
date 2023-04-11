using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Tests
{
    public static class FirstTestFunctionTests
    {
        //Naming Convention - ClassName_MethodName_ExpectedResult
        public static void FirstTestFunction_ReturnsSomethingIfZero_ReturnsString()
        {
            try
            {
                //Arrange - Get your variables, everything you need, your classes, functions - everything that is needed for our function to run
                int number = 0;
                FirstTestFunction firstTest = new FirstTestFunction();


                //Act - Execute the function
                string result = firstTest.ReturnsSomethingIfZero(number);


                //Assert - Whatever is returned is it what you want
                if (result == "SOMETHING")
                {
                    Console.WriteLine("PASSED: FirstTestFunction.ReturnsSomethingIfZero_ReturnsString");
                }
                else
                {
                    Console.WriteLine("FAILED: FirstTestFunction.ReturnsSomethingIfZero_ReturnsString");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
