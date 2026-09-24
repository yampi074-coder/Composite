using System;
using System.Collections.Generic;

namespace CompositeDemo.Parqueadero
{
    // Componente base del patrón Composite para el parqueadero
    abstract class ParqueComponent
    {
        public virtual void Add(ParqueComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(ParqueComponent component)
        {
            throw new NotImplementedException();
        }

        public virtual bool IsComposite() => true;

        // Devuelve plazas disponibles dentro del componente (1 para plaza libre, 0 ocupada)
        public abstract int PlazasDisponibles();

        // Intenta aparcar un coche en este componente o en sus hijos. Devuelve true si se aparcó.
        public abstract bool Aparcar();
    }

    // Hoja: plaza individual
    class Plaza : ParqueComponent
    {
        public string Id { get; }
        private bool _ocupada;

        public Plaza(string id)
        {
            Id = id;
            _ocupada = false;
        }

        public override int PlazasDisponibles() => _ocupada ? 0 : 1;

        public override bool Aparcar()
        {
            if (_ocupada) return false;
            _ocupada = true;
            return true;
        }

        public void Salir()
        {
            _ocupada = false;
        }

        // Estado público de ocupación
        public bool Ocupada => _ocupada;

        public override bool IsComposite() => false;
    }

    // Composite: zona de parqueo que contiene plazas u otras zonas
    class ZonaParqueo : ParqueComponent
    {
        private readonly List<ParqueComponent> _hijos = new();
        public string Nombre { get; }

        public ZonaParqueo(string nombre)
        {
            Nombre = nombre;
        }

        public override void Add(ParqueComponent component) => _hijos.Add(component);

        public override void Remove(ParqueComponent component) => _hijos.Remove(component);

        public override int PlazasDisponibles()
        {
            int total = 0;
            foreach (var h in _hijos) total += h.PlazasDisponibles();
            return total;
        }

        public override bool Aparcar()
        {
            foreach (var h in _hijos)
            {
                if (h.Aparcar()) return true;
            }
            return false;
        }

        // Busca una plaza por id y la libera (si existe)
        public bool LiberarPlaza(string id)
        {
            foreach (var h in _hijos)
            {
                if (h is Plaza p && p.Id == id)
                {
                    p.Salir();
                    return true;
                }
                if (h is ZonaParqueo z && z.LiberarPlaza(id)) return true;
            }
            return false;
        }

        // Permite acceder a los hijos para la UI
        public IEnumerable<ParqueComponent> GetHijos() => _hijos;
    }

    // Ejemplo de uso y cliente
    public static class EjemploParqueo
    {
        public static void Ejecutar()
        {
            // Crear un pequeño parqueo con zonas y plazas
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

            Console.WriteLine($"Plazas disponibles iniciales: {entrada.PlazasDisponibles()}");

            // Simular llegada de coches
            for (int i = 1; i <= 5; i++)
            {
                bool aparcado = entrada.Aparcar();
                Console.WriteLine(aparcado
                    ? $"Coche {i} aparcado. Plazas restantes: {entrada.PlazasDisponibles()}"
                    : $"Coche {i} no pudo aparcar. Plazas restantes: {entrada.PlazasDisponibles()}");
            }

            // Liberar una plaza (simulamos que sale B1)
            Console.WriteLine("Liberando una plaza (B1) y probando de nuevo...");
            entrada.LiberarPlaza("B1");

            Console.WriteLine($"Plazas disponibles tras liberar: {entrada.PlazasDisponibles()}");

            bool aparcado2 = entrada.Aparcar();
            Console.WriteLine(aparcado2
                ? $"Coche nuevo aparcado. Plazas restantes: {entrada.PlazasDisponibles()}"
                : $"Coche nuevo no pudo aparcar. Plazas restantes: {entrada.PlazasDisponibles()}");
        }
    }
}
