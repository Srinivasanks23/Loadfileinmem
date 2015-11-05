using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadExecutionTest
{
    class ThreadExecutionSample
    {

        public void Process()
        {
            int maxElements = 500000;
            int maxArrays = 500;
            int[][] numbers = new int[maxArrays][];
            bool[] booleans = new bool[maxArrays];
            List<int> maxLog = new List<int>();
            List<int> maxLog2 = new List<int>();

            AutoResetEvent event1 = new AutoResetEvent(false);
            AutoResetEvent event2 = new AutoResetEvent(false);

            Random rand = new Random(1000000);

            //Feed the value to the number array
            for (int i = 0; i < maxArrays; i++)
            {
                numbers[i] = new int[maxElements];

                for (int j = 0; j < maxElements; j++)
                {
                    numbers[i][j] = rand.Next();
                }
            }

            List<Task> taskList = new List<Task>();
            //StreamWriter writer = File.AppendText(@"C:\Users\kesavsr\Documents\Log.txt");
            Console.WriteLine("Done building data" + DateTime.Now);
            try
            {
                for (int i = 0; i < maxArrays; i++)
                {
                    taskList.Add(Task.Factory.StartNew((x) =>
                    {
                        //Console.WriteLine("Running the task");
                        //Interlocked.Increment(ref threadCount);
                        booleans[(int)x] = true;
                        event1.Set();
                        Thread.Sleep(1);
                        event2.WaitOne();
                        booleans[(int)x] = false;

                    }, i));
                }

                //Task.WaitAll(taskList.ToArray());

                /*while (!Task.WhenAll(taskList.ToArray()).IsCompleted)
                {
                    maxLog.Add(booleans.Count((x) => x == true));
                    //Thread.Sleep(50);
                }*/

                while (!Task.WhenAll(taskList.ToArray()).IsCompleted)
                {
                    event1.WaitOne(500);
                    maxLog.Add(booleans.Count((x) => x == true));

                    /*
                    int count = 0;

                    foreach(ProcessThread t in Process.GetCurrentProcess().Threads)
                    {
                         ++count;
                    }
                    maxLog2.Add(count);*/

                    event2.Set();
                    //Thread.Sleep(50);
                }

                // writer.WriteLine("Logging ends here " + DateTime.Now);
            }
            catch (Exception ex)
            {

            }

            finally
            {
                /* if (writer != null)
                {
                    writer.Close();
                } */
            }

            Console.WriteLine("max ThreadCount : " + maxLog.Max());
            //Console.WriteLine("max ThreadCount2 : " + maxLog2.Max());
            Console.ReadLine();
        }
    }
}
