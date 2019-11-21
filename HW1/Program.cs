using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HW1
{
    class Program
    {
        static List<int> list = new List<int>();


        static void Foo(object i)
        {
            // check and add
        }

        static void Main(string[] args)
        {            
            var thFill = new Thread(() => FillList());
            var thPrint = new Thread(() => PrintList());
            var thRevert = new Thread(() => RevertList());
            var thMax = new Thread(() => PrintMaxValue());

            //Race Condition - it's supposed that first thread thFill is finished and we have filled list 
            //than take Max value from list in thMax thread - but first thread hasn't been finished yet
            //so we get Max value from the middle of the list or even exception 
            //"System.InvalidOperationException: 'Collection was modified; enumeration operation may not execute.'"
            Console.WriteLine("*********Race Codition*********");
            thFill.Start();
            thMax.Start();
            thFill.Join();
            thMax.Join();

            //Atomicity - thPrint thread print values from list from start to end and wants to display all values one by one
            //thRevert thread revert list of values using descending ordering
            //but first thread don't know about second thread and continue to display values
            //so we have a gap in displaying - some values aren't displyed at all, some values are displayed twice
            Console.WriteLine("************Atomicity************");
            thPrint.Start();
            thRevert.Start();
            thPrint.Join();
            thRevert.Join();
            Console.Read();
        }

        static void FillList()
        {
            for (int i = 0; i < 50; i++)
            {
                list.Add(i);
                Console.WriteLine($"{list[i]} - [{Thread.CurrentThread.ManagedThreadId}]");
                Thread.Sleep(2);
            }
            Console.WriteLine("All list is filled");
        }

        static void PrintMaxValue()
        {
            Thread.Sleep(25);
            Console.WriteLine($"Max - {list.Max()} - [{Thread.CurrentThread.ManagedThreadId}]");
        }

        static void PrintList()
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{list[i]} - [{Thread.CurrentThread.ManagedThreadId}]");
                Thread.Sleep(100);
            }
        }

        static void RevertList()
        {
            Thread.Sleep(1700);
            Console.WriteLine($"Going to change ordering from the thread - [{Thread.CurrentThread.ManagedThreadId}]");
            list = list.OrderByDescending(s => s).ToList();
        }
    }
}
