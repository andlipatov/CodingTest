namespace CodingTest
{
    public static class Challenge2
    {
        public static void Execute()
        {
            Thread writer1 = new(() => Writer(1));
            Thread writer2 = new(() => Writer(2));

            Thread reader1 = new(Reader);
            Thread reader2 = new(Reader);
            Thread reader3 = new(Reader);
            Thread reader4 = new(Reader);
            Thread reader5 = new(Reader);

            writer1.Start();
            writer2.Start();

            reader1.Start();
            reader2.Start();
            reader3.Start();
            reader4.Start();
            reader5.Start();

            writer1.Join();
            writer2.Join();
            reader1.Join();
            reader2.Join();
            reader3.Join();
            reader4.Join();
            reader5.Join();
        }

        static void Reader()
        {
            for (int i = 0; i < 5; i++)
            {
                int count = Server.GetCount();
                Console.WriteLine($"Читатель {Thread.CurrentThread.ManagedThreadId} прочитал: {count}");
                Thread.Sleep(100);
            }
        }

        static void Writer(int value)
        {
            for (int i = 0; i < 3; i++)
            {
                Server.AddToCount(value);
                Console.WriteLine($"Писатель {Thread.CurrentThread.ManagedThreadId} добавил: {value}");
                Thread.Sleep(200);
            }
        }
    }

    public static class Server
    {
        private static readonly object lockObject = new();
        private static int count;
        private static int readerCount = 0;

        public static int GetCount()
        {
            lock (lockObject)
            {
                readerCount++;
            }

            try
            {
                lock (lockObject)
                {
                    return count;
                }
            }
            finally
            {
                lock (lockObject)
                {
                    readerCount--;
                    if (readerCount == 0)
                    {
                        Monitor.Pulse(lockObject);
                    }
                }
            }
        }

        public static void AddToCount(int value)
        {
            lock (lockObject)
            {
                while (readerCount > 0)
                {
                    Monitor.Wait(lockObject);
                }

                count += value;
                Monitor.PulseAll(lockObject);
            }
        }
    }
}