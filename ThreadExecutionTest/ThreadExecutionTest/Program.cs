using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadExecutionTest
{
    class Program
    {
        private static SemaphoreSlim semaphore;

        // A padding interval to make the output more orderly.
        private static int padding;

        static void Main(string[] args)
        {
            semaphore = new SemaphoreSlim(10, 10);

            Console.WriteLine("{0} tasks can enter the semaphore.",
                          semaphore.CurrentCount);
            Task[] tasks = new Task[10];

            try
            {
                // Create and start five numbered tasks.
                for (int i = 0; i < 10; i++)
                {
                    tasks[i] = Task.Run(() => Process());
                }

                // Wait for half a second, to allow all the tasks to start and block.
                Thread.Sleep(500);

                // Restore the semaphore count to its maximum value.
                Console.Write("Main thread calls Release(3) --> ");
                Console.WriteLine("{0} tasks can enter the semaphore.",
                                  semaphore.CurrentCount);
                // Main thread waits for the tasks to complete.


                while (semaphore.CurrentCount != 10)
                {
                    Thread.Sleep(50);
                    Console.WriteLine("Current count is " + semaphore.CurrentCount);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("Main thread exits.");
            Console.ReadKey();
        }

        private static void Process()
        {
            try
            {
                // Each task begins by requesting the semaphore.
                Console.WriteLine("Task {0} begins and waits for the semaphore.",
                                  Task.CurrentId);

                semaphore.Wait();

                Interlocked.Add(ref padding, 100);

                Console.WriteLine("Task {0} enters the semaphore.", Task.CurrentId);

                // The task just sleeps for 1+ seconds.
                Thread.Sleep(4000 + padding);

                //Console.WriteLine("Task {0} releases the semaphore; previous count: {1}.",
                  //                Task.CurrentId, semaphore.Release());
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                semaphore.Release();
            }
        }
    }
}
