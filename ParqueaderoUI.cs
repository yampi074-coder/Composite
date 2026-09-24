using System;
using System.Collections.Generic;
using CompositeDemo.Parqueadero;

public static class ParqueaderoUI
{
    public static void Run()
    {
        var entrada = BuildParqueo();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Menú Parqueadero");
            Console.WriteLine("1 Mostrar estado del parqueadero");
            Console.WriteLine("2 Aparcar un coche");
            Console.WriteLine("3 Liberar una plaza por ID");
            Console.WriteLine("4 Salir");
            Console.Write("Elige una opción: ");
            var opt = Console.ReadLine();
            Console.WriteLine();

            switch (opt)
            {
                case "1":
                    MostrarEstado(entrada, "");
                    Console.WriteLine($"Plazas disponibles: {entrada.PlazasDisponibles()}");
                    break;
                case "2":
                    var aparcado = entrada.Aparcar();
                    Console.WriteLine(aparcado ? "Coche aparcado correctamente." : "No hay plazas disponibles.");
                    Console.WriteLine($"Plazas disponibles: {entrada.PlazasDisponibles()}");
                    break;
                case "3":
                    Console.Write("ID de la plaza a liberar (ej. B1): ");
                    var id = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(id)) { Console.WriteLine("ID inválido."); break; }
                    var liberado = entrada.LiberarPlaza(id.Trim());
                    Console.WriteLine(liberado ? "Plaza liberada." : "No se encontró la plaza.");
                    Console.WriteLine($"Plazas disponibles: {entrada.PlazasDisponibles()}");
                    break;
                case "4":
                    Console.WriteLine("Saliendo...");
                    return;
                default:
                    Console.WriteLine("Opción no válida.");
                    break;
            }
        }
    }

    private static ZonaParqueo BuildParqueo()
    {
        var entrada = new ZonaParqueo("Entrada");

        var zonaA = new ZonaParqueo("Zona A");
        zonaA.Add(new Plaza("A1"));
        zonaA.Add(new Plaza("A2"));
        zonaA.Add(new Plaza("A3"));

        var zonaB = new ZonaParqueo("Zona B");
        zonaB.Add(new Plaza("B1"));
        zonaB.Add(new Plaza("B2"));

        entrada.Add(zonaA);
        entrada.Add(zonaB);

        return entrada;
    }

    private static void MostrarEstado(ParqueComponent componente, string indent)
    {
        if (componente is Plaza p)
        {
            Console.WriteLine($"{indent}- Plaza {p.Id}: {(p.Ocupada ? "Ocupada" : "Libre")}");
            return;
        }

        if (componente is ZonaParqueo z)
        {
            Console.WriteLine($"{indent}+ Zona {z.Nombre}");
            foreach (var hijo in z.GetHijos())
            {
                MostrarEstado(hijo, indent + "  ");
            }
        }
    }
}
