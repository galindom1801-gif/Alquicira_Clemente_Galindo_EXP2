namespace Juegoteca.Biblioteca.Modelos;

public class Usuario
{
    public string Nombre { get; }

    // Clave: nombre de la colección. Valor: videojuegos de esa colección.
    public Dictionary<string, List<Videojuego>> Colecciones { get; } =
        new(StringComparer.OrdinalIgnoreCase);

    public Usuario(string nombre)
    {
        Nombre = nombre ?? throw new ArgumentNullException(nameof(nombre));
    }

    public void CrearColeccion(string nombre)
    {
        if (Colecciones.ContainsKey(nombre))
            throw new InvalidOperationException($"La colección '{nombre}' ya existe.");

        Colecciones[nombre] = new List<Videojuego>();
    }

    public void AgregarJuegoAColeccion(string nombre, Videojuego juego)
    {
        if (!Colecciones.TryGetValue(nombre, out var lista))
            throw new KeyNotFoundException($"La colección '{nombre}' no existe.");

        if (!lista.Contains(juego))
            lista.Add(juego);
    }

    public bool EliminarColeccion(string nombre) => Colecciones.Remove(nombre);

    public List<Videojuego> ObtenerColeccion(string nombre)
    {
        if (!Colecciones.TryGetValue(nombre, out var lista))
            throw new KeyNotFoundException($"La colección '{nombre}' no existe.");

        return lista;
    }

    public int ContarJuegos()
    {
        int total = 0;
        foreach (var lista in Colecciones.Values)
            total += lista.Count;
        return total;
    }

    public void MostrarColecciones()
    {
        if (Colecciones.Count == 0)
        {
            Console.WriteLine("No tienes colecciones.");
            return;
        }

        foreach (var (nombre, juegos) in Colecciones)
        {
            Console.WriteLine($"Colección: {nombre} ({juegos.Count} juegos)");
            foreach (var juego in juegos)
                Console.WriteLine($"  - {juego}");
        }
    }
}
