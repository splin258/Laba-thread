using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace _1лаба_ТЯП_Акт
{
    class Program
    {

        static List<List<int>> matrix_1 = new List<List<int>>();
        static List<List<int>> matrix_2 = new List<List<int>>();
        static int size_matrix = 3;
        static int count_thread = 3;
        static int range;
        static volatile int starterCount = 0;
        static ManualResetEvent startEvent = new ManualResetEvent(false);
        static object LockObject = new object();
        static List<Thread> threads = new List<Thread>();
        //static List<line_matrix> thread_line = new List<line_matrix>();
        static object r = 0;
        static int count = 0;
        struct line_matrix
        {
            public Thread thread;
            public int line;
        }

        static void Main(string[] args)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            stopWatch.Stop();
            long ts = stopWatch.ElapsedMilliseconds;
            /*string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds);*/
            Console.WriteLine("RunTime " + ts);
            //
           // Console.ReadLine();
            //stopWatch.Reset();
            Thread thread1 = new Thread(init1);
             Thread thread2 = new Thread(init2);
             threads.Add(thread1);
             threads.Add(thread2);
            stopWatch.Reset();
            
            foreach (var thread in threads)
            {
                new Thread(Starting).Start(thread);
            }
            while (starterCount < 2)
            {
                Thread.Sleep(1);
            }
            startEvent.Set();
            stopWatch.Start();
            while (count < 2)
            {
                //Console.WriteLine("sdkjdfksdbvf");
            }
            stopWatch.Stop();

            Console.WriteLine("RunTime " + ts);
            count = 0;
          //  ret();
           // ret2();
            /*for (int i = 0; i < size_matrix; i++)
            {
                line_matrix tmp;
                tmp.line = i;
                tmp.thread = new Thread(summ_line);
                new Thread(Starting_line).Start(tmp);
            }
            */
            int j = 0;
            for (int k = 1; k <= count_thread; k++)
            {
                range = size_matrix / k;
                stopWatch.Reset();
                for (int i = 0; i < k; i++)
                {
                    line_matrix tmp;
                    tmp.line = i;
                    tmp.thread = new Thread(summ_line);
                    new Thread(Starting_line).Start(tmp);

                }

                while (starterCount < k)
                {
                    Thread.Sleep(1);
                }
                startEvent.Set();
                stopWatch.Start();
                while (count < k)
                {
                    //Console.WriteLine(count);
                }
                stopWatch.Stop();
                
                count = 0;
                 ts = (int)stopWatch.ElapsedMilliseconds;

                Console.WriteLine("RunTime " + k + "    " + ts);
            }

            Console.WriteLine("end");

            //ret();
            /*for(int i = 0;  i < size_matrix; i++)
            {
                Thread thread_sum = new Thread(summ_line);
                threads.Add(thread_sum);
            }*/
            //stopWatch.Start();
            //stopWatch.Stop();
            //int ts = (int)stopWatch.ElapsedMilliseconds;
            /*string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds);*/
            //Console.WriteLine("RunTime " + ts);
            //*/
            Console.ReadLine();
        }
        /*
        static void summ_line(object line)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int t = (int)line;
            for (int i = 0; i < matrix_1[0].Count; i++)
            {
                matrix_1[t][i] = matrix_1[t][i] + matrix_2[t][i];
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds);
            Console.WriteLine("RunTime " + line + "     " + elapsedTime);
            count++;
        }
        */
        static void summ_line(object line)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int t = (int)line;
            int startRange = 0;

            if (t == 0)
                startRange = 0;
            else if (t == 1)
                startRange = range;
            else
                startRange = (t - 1) * range;
            if ( t == 0)
            {
                for (; startRange < (t+1)*range; startRange++)
                {
                    for (int i = 0; i < size_matrix; i++)
                    {
                        matrix_1[startRange][i] = matrix_1[startRange][i] + matrix_2[startRange][i];
                    }
                }
            }
            else
            {
                for (; startRange < t*range; startRange++)
                {
                    for (int i = 0; i < matrix_1[0].Count; i++)
                    {
                        matrix_1[startRange][startRange] = matrix_1[startRange][i] + matrix_2[startRange][i];
                    }
                }
            }
            stopWatch.Stop();
            int ts = (int)stopWatch.ElapsedMilliseconds;
            /*string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:0000}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds);*/
            //Console.WriteLine("" + ts);
            count++;
        }

        static void ret()
        {

            
            for (int i = 0; i < size_matrix; i++)
            {
                for(int j = 0; j < size_matrix; j++)
                {
                    Console.Write(matrix_1[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void ret2()
        {


            for (int i = 0; i < size_matrix; i++)
            {
                for (int j = 0; j < size_matrix; j++)
                {
                    Console.Write(matrix_2[i][j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void Starting(object paramThread)
        {
            lock (LockObject)
            {
                starterCount++;
            }
            startEvent.WaitOne();
            (paramThread as Thread).Start();
       
        }

        static void Starting_line(object paramThread)
        {
            lock (LockObject)
            {
                starterCount++;
            }
            startEvent.WaitOne();
            line_matrix t = (line_matrix)paramThread;
            t.thread.Start(t.line);

        }
        static void init1()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < size_matrix; i++)
            {
                List<int> row_for_matrix_1 = new List<int>();
                for (int j = 0; j < size_matrix; j++)
                {
                    Random rnd = new Random();
                    row_for_matrix_1.Add(rnd.Next(-100, 100));//i+j+1);
                    //Thread.Sleep(1);
                }
                matrix_1.Add(row_for_matrix_1);
            }
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds);
            Console.WriteLine("RunTime 1  " + elapsedTime);
            count++;
        }


        static void init2()
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            for (int i = 0; i < size_matrix; i++)
            {

                List<int> row_for_matrix_2 = new List<int>();
                for (int j = 0; j < size_matrix; j++)
                {

                    Random rnd = new Random();
                    row_for_matrix_2.Add(rnd.Next(-100, 100));//(i+j+1)*2);
                    //Thread.Sleep(1);

                }

                matrix_2.Add(row_for_matrix_2);
                }
            stopWatch.Stop(); 
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds);
            Console.WriteLine("RunTime 2  " + elapsedTime);
            count++;
        }
        
    }
}
