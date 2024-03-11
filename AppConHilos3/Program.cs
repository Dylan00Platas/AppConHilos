using System;
using System.Threading;

public class Persona
{
    public string Nombre { get; set; }
    public int Edad { get; set; }
    public string Sexo { get; set; }

    public Persona(string nombre, int edad, string sexo)
    {
        this.Nombre = nombre;
        this.Edad = edad;
        this.Sexo = sexo;
    }
}

class Program
{
    static void TareaDeFondoConParametro(Object? stateInfo)
    {
        if (stateInfo == null)
            return;

        Thread currentThread = Thread.CurrentThread;
        Console.WriteLine($"Es un thread background: {currentThread.IsBackground}");

        Persona data = (Persona)stateInfo;
        Console.WriteLine($"Hola {data.Nombre} desde ThreadPool.");
        Thread.Sleep(1000);
    }

    static void TareaDeFondo(Object? stateInfo)
    {
        Console.WriteLine($"Hilo 1: Hola soy un hilo sin uso parámetros desde ThreadPool.");
        Thread.Sleep(1500);
    }

    static void Main(string[] args)
    {
        int workers, ports;

        // Obtener el máximo número de hilos
        ThreadPool.GetMaxThreads(out workers, out ports);
        Console.WriteLine($"Máximos hilos de trabajo: {workers}");
        Console.WriteLine($"Máximos puertos para hilos: {ports}");

        // Obtener un número mínimo de hilos
        ThreadPool.GetMinThreads(out workers, out ports);
        Console.WriteLine($"Mínimos hilos de trabajo: {workers}");
        Console.WriteLine($"Mínimos puertos (completion port) para hilos: {ports}");

        // Obtener hilos disponibles
        ThreadPool.GetAvailableThreads(out workers, out ports);
        Console.WriteLine($"Hilos de trabajo disponibles: {workers}");
        Console.WriteLine($"Hilos de puerto (completion port) disponibles: {ports}");

        // Obtenga el número total de procesadores disponibles en la máquina
        int processCount = Environment.ProcessorCount;
        Console.WriteLine($"No. de procesadores disponibles en el sistema: {processCount}");
        Console.WriteLine($"---------------------------------");

        // Utilice ThreadPool para un hilo de trabajo
        ThreadPool.QueueUserWorkItem(TareaDeFondo);

        // Obtener hilos disponibles
        ThreadPool.GetAvailableThreads(out workers, out ports);
        Console.WriteLine($"Hilos de trabajo disponibles después del hilo 1: {workers}");

        // Crea un objeto y lo envía al hilo de trabajo de ThreadPool
        Persona p = new Persona("Dylan Platas", 21, "Hombre");
        ThreadPool.QueueUserWorkItem(TareaDeFondoConParametro, p);

        // Obtener hilos disponibles
        ThreadPool.GetAvailableThreads(out workers, out ports);
        Console.WriteLine($"Hilos de trabajo disponibles después del hilo 2: {workers}");

        // Esperamos a que ambos hilos finalicen
        Thread.Sleep(2000);

        // Obtener hilos disponibles
        ThreadPool.GetAvailableThreads(out workers, out ports);
        Console.WriteLine($"Hilos de trabajo disponibles al final: {workers}");

        // Esta instrucción es necesaria para que no termine el programa
        Console.ReadKey();
    }
}
