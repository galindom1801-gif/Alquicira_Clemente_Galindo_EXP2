using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Servicios;

public class ServicioVideojuegos
{
    public IGestorVideojuegos Gestor { get; }
    public IExportador Exportador { get; }
    public List<Usuario> Usuarios { get; }

    public ServicioVideojuegos(IGestorVideojuegos gestor, IExportador exportador)
    {
        Gestor = gestor ?? throw new ArgumentNullException(nameof(gestor));
        Exportador = exportador ?? throw new ArgumentNullException(nameof(exportador));
        Usuarios = new List<Usuario>();
    }

    public Usuario RegistrarUsuario(string nombre)
    {
        var usuario = new Usuario(nombre);
        Usuarios.Add(usuario);
        return usuario;
    }

    public Usuario? BuscarUsuario(string nombre)
    {
        foreach (var usuario in Usuarios)
        {
            if (string.Equals(usuario.Nombre, nombre, StringComparison.OrdinalIgnoreCase))
                return usuario;
        }
        return null;
    }

    public bool EliminarUsuario(string nombre)
    {
        var usuario = BuscarUsuario(nombre);
        return usuario != null && Usuarios.Remove(usuario);
    }

    public int ContarUsuarios() => Usuarios.Count;

    public void OrdenarCatalogo(string algoritmo)
    {
        switch (algoritmo?.ToLowerInvariant())
        {
            case "quick":
                Gestor.QuickSort(Gestor.JuegosDisponibles);
                break;
            case "merge":
                ReemplazarContenido(Gestor.JuegosDisponibles, Gestor.MergeSort(Gestor.JuegosDisponibles));
                break;
            default:
                throw new ArgumentException($"Algoritmo desconocido: '{algoritmo}'. Use 'quick' o 'merge'.", nameof(algoritmo));
        }

        Console.WriteLine($"Catálogo ordenado con {algoritmo!.ToLowerInvariant()}.");
    }

    // Ordena la colección por HorasJuego y retorna la suma total de horas.
    public int OrdenarColeccionYCalcularHoras(Usuario usuario, string coleccion, string algoritmo = "quick")
    {
        ArgumentNullException.ThrowIfNull(usuario);

        var juegos = usuario.ObtenerColeccion(coleccion);

        switch (algoritmo?.ToLowerInvariant())
        {
            case "quick":
                Gestor.QuickSort(juegos);
                break;
            case "merge":
                ReemplazarContenido(juegos, Gestor.MergeSort(juegos));
                break;
            default:
                throw new ArgumentException($"Algoritmo desconocido: '{algoritmo}'. Use 'quick' o 'merge'.", nameof(algoritmo));
        }

        int total = 0;
        foreach (var juego in juegos)
            total += juego.HorasJuego;
        return total;
    }

    public void ExportarColecciones(Usuario usuario, string ruta) =>
        Exportador.ExportarATxt(usuario, ruta);

    private static void ReemplazarContenido(List<Videojuego> destino, List<Videojuego> ordenada)
    {
        destino.Clear();
        destino.AddRange(ordenada);
    }
}
