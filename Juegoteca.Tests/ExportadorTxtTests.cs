using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class ExportadorTxtTests : IDisposable
{
    private readonly ExportadorTxt _exportador = new();
    private readonly string _ruta = Path.Combine(Path.GetTempPath(), $"juegoteca_{Guid.NewGuid():N}.txt");

    // Limpieza: cada prueba borra su archivo temporal al terminar.
    public void Dispose()
    {
        if (File.Exists(_ruta))
            File.Delete(_ruta);
    }

    [Fact]
    public void ExportarATxt_UsuarioConColecciones_CreaElArchivoEnLaRuta()
    {
        var usuario = DatosPrueba.UsuarioConColecciones();

        _exportador.ExportarATxt(usuario, _ruta);

        Assert.True(File.Exists(_ruta));
    }

    [Fact]
    public void ExportarATxt_UsuarioConColecciones_IncluyeJugadorColeccionJuegoYTotalDeHoras()
    {
        var usuario = DatosPrueba.UsuarioConColecciones();

        _exportador.ExportarATxt(usuario, _ruta);
        string texto = File.ReadAllText(_ruta);

        Assert.Contains("Ana", texto);
        Assert.Contains("Pendientes", texto);
        Assert.Contains("Celeste", texto);
        Assert.Contains("Total de horas: 8 h", texto);
    }

    [Fact]
    public void ExportarATxt_UsuarioSinColecciones_CreaArchivoSoloConEncabezadoSinExcepcion()
    {
        var usuario = new Usuario("Beto");

        var excepcion = Record.Exception(() => _exportador.ExportarATxt(usuario, _ruta));
        string texto = File.ReadAllText(_ruta);

        Assert.Null(excepcion);
        Assert.Contains("Beto", texto);
        Assert.DoesNotContain("Colección:", texto);
    }

    [Fact]
    public void ExportarATxt_DosVecesALaMismaRuta_DejaSoloElContenidoDeLaUltimaExportacion()
    {
        var primero = new Usuario("Primero");
        var segundo = new Usuario("Segundo");

        _exportador.ExportarATxt(primero, _ruta);
        _exportador.ExportarATxt(segundo, _ruta);
        string texto = File.ReadAllText(_ruta);

        Assert.Contains("Segundo", texto);
        Assert.DoesNotContain("Primero", texto);
    }

    [Fact]
    public void ExportarATxt_UsuarioNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _exportador.ExportarATxt(null!, _ruta));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void ExportarATxt_RutaVacia_LanzaArgumentException(string rutaInvalida)
    {
        var usuario = DatosPrueba.UsuarioConColecciones();

        Assert.Throws<ArgumentException>(() => _exportador.ExportarATxt(usuario, rutaInvalida));
    }
}
