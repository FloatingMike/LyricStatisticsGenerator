using System;

namespace AireLogicTest
{
    public class ConsoleInputService : IInputService
    {
        public string RequestInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}