using Juegoteca.Biblioteca.Interfaces;
using Juegoteca.Biblioteca.Modelos;

namespace Juegoteca.Tests.Datos;

// Exportador falso: no escribe archivos, solo registra cómo se le llamó.
public class ExportadorPrueba : IExportador
{
    public int Llamadas { get; private set; }
    public Usuario? UltimoUsuario { get; private set; }
    public string? UltimaRuta { get; private set; }

    public void ExportarATxt(Usuario usuario, string rutaArchivo)
    {
        Llamadas++;
        UltimoUsuario = usuario;
        UltimaRuta = rutaArchivo;
    }
}
