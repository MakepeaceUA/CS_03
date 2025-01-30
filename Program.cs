namespace ConsoleApp22
{
    internal class Program
    {
        static Mutex mutex = new Mutex();
        static string NumFile = "numbers.txt";
        static string PrimeFile = "primes.txt";
        static string PrimeSevFile = "primes_seven.txt";

        static void Main()
        {
            Thread thread1 = new Thread(Generate);
            Thread thread2 = new Thread(FilterPrimes);
            Thread thread3 = new Thread(FilterPrimesSeven);

            thread1.Start();
            thread1.Join(); 

            thread2.Start();
            thread2.Join(); 

            thread3.Start();
            thread3.Join();

            Console.WriteLine("Все операции завершены.");
        }

        static void Generate()
        {
            mutex.WaitOne();
            Random rand = new Random();
            var numbers = Enumerable.Range(0, 100).Select(x => rand.Next(1, 1000).ToString());
            File.WriteAllLines(NumFile, numbers);
            mutex.ReleaseMutex();
        }

        static void FilterPrimes()
        {
            mutex.WaitOne();
            if (!File.Exists(NumFile))
            {
                Console.WriteLine("Файл с числами не найден.");
                mutex.ReleaseMutex();
                return;
            }
            var numbers = File.ReadAllLines(NumFile).Select(int.Parse);
            var primes = numbers.Where(IsPrime);
            File.WriteAllLines(PrimeFile, primes.Select(n => n.ToString()));
            mutex.ReleaseMutex();
        }

        static void FilterPrimesSeven()
        {
            mutex.WaitOne();
            if (!File.Exists(PrimeFile))
            {
                Console.WriteLine("Файл с простыми числами не найден.");
                mutex.ReleaseMutex();
                return;
            }
            var primes = File.ReadAllLines(PrimeFile).Select(int.Parse);
            var primes_seven = primes.Where(n => n % 10 == 7);
            File.WriteAllLines(PrimeSevFile, primes_seven.Select(n => n.ToString()));
            mutex.ReleaseMutex();
        }

        static bool IsPrime(int number)
        {
            if (number < 2)
            { 
                return false; 
            }
            for (int i = 2; i * i <= number; i++)
            {
                if (number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}