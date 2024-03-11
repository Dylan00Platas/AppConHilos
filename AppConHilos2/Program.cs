using System;
using System.Threading;

namespace AppConHilos2
{
    class Program
    {
        static void Print(object obj)
        {
            // Se debe asegurar que el token viene como parámetro
            if (obj == null || !(obj is CancellationToken token))
                return;

            for (int i = 11; i < 20; i++)
            {
                // Agregue la verificación de cancelación del token
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine($"En la iteración {i}, la cancelación ha sido solicitada...");
                    // Termina la operación for
                    break;
                }

                Console.WriteLine($"Print thread: {i}");
                Thread.Sleep(1000);
            }
        }

        static void Main(string[] args)
        {
            // Cree un hilo secundario pasando un delegado ParameterizedThreadStart
            Thread workerThread = new Thread(new ParameterizedThreadStart(Print));

            // Le colocamos nombre al hilo
            workerThread.Name = "Hilo de Print";

            // Crear el token source.
            CancellationTokenSource cts = new CancellationTokenSource();

            // Iniciar hilo secundario, ahora le enviamos un token
            workerThread.Start(cts.Token);

            // Hilo principal: Imprime de 1 a 10 cada 0,2 segundos.
            // El método Thread.Sleep es responsable de hacer que el hilo actual entre en suspensión 
            // en milisegundos. Mientras duerme, un hilo no hace nada.
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Principal thread: {i}");
                Thread.Sleep(200);
            }

            // Si el hilo sigue ejecutándose, lo cancelamos
            if (workerThread.IsAlive)
                cts.Cancel();
        }
    }
}
