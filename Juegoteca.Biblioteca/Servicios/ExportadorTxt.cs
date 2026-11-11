using System.Text;
using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Biblioteca.Servicios;

public class ExportadorTxt : IExportador
{
    public void ExportarATxt(Usuario usuario, string rutaArchivo)
    {
        ArgumentNullException.ThrowIfNull(usuario);
        if (string.IsNullOrWhiteSpace(rutaArchivo))
            throw new ArgumentException("La ruta del archivo no puede estar vacía.", nameof(rutaArchivo));

        var sb = new StringBuilder();
        sb.AppendLine($"Jugador: {usuario.Nombre}");
        sb.AppendLine($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}");

        foreach (var (nombre, juegos) in usuario.Colecciones)
        {
            sb.AppendLine();
            sb.AppendLine($"Colección: {nombre}");

            int totalHoras = 0;
            foreach (var juego in juegos)
            {
                sb.AppendLine($"  - {juego}");
                totalHoras += juego.HorasJuego;
            }
            sb.AppendLine($"Total de horas: {totalHoras} h");
        }

        // WriteAllText sobrescribe el archivo si ya existe.
        File.WriteAllText(rutaArchivo, sb.ToString(), Encoding.UTF8);
    }
}
