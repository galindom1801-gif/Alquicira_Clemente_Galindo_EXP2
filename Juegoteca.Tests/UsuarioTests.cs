using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class UsuarioTests
{
    private readonly Usuario _usuario;
    private readonly List<Videojuego> _catalogo;

    public UsuarioTests()
    {
        _usuario = new Usuario("Ana");
        _catalogo = DatosPrueba.Catalogo();
    }

    [Fact]
    public void Constructor_ConNombre_AsignaNombreYColeccionesVacio()
    {
        Assert.Equal("Ana", _usuario.Nombre);
        Assert.NotNull(_usuario.Colecciones);
        Assert.Empty(_usuario.Colecciones);
    }

    [Fact]
    public void CrearColeccion_NombreNuevo_CreaClaveConListaVacia()
    {
        _usuario.CrearColeccion("Pendientes");

        Assert.True(_usuario.Colecciones.ContainsKey("Pendientes"));
        Assert.Empty(_usuario.Colecciones["Pendientes"]);
    }

    [Fact]
    public void CrearColeccion_NombreRepetido_LanzaInvalidOperationException()
    {
        _usuario.CrearColeccion("Pendientes");

        Assert.Throws<InvalidOperationException>(() => _usuario.CrearColeccion("Pendientes"));
    }

    [Fact]
    public void AgregarJuegoAColeccion_MismoJuegoDosVeces_LoDejaUnaSolaVez()
    {
        _usuario.CrearColeccion("Pendientes");

        _usuario.AgregarJuegoAColeccion("Pendientes", _catalogo[0]);
        _usuario.AgregarJuegoAColeccion("Pendientes", _catalogo[0]);

        Assert.Single(_usuario.ObtenerColeccion("Pendientes"));
    }

    [Fact]
    public void AgregarJuegoAColeccion_ColeccionInexistente_LanzaKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => _usuario.AgregarJuegoAColeccion("NoExiste", _catalogo[0]));
    }

    [Fact]
    public void ContarJuegos_DosColeccionesConUnJuegoCadaUna_Retorna2()
    {
        _usuario.CrearColeccion("Pendientes");
        _usuario.CrearColeccion("Favoritos");
        _usuario.AgregarJuegoAColeccion("Pendientes", _catalogo[0]);
        _usuario.AgregarJuegoAColeccion("Favoritos", _catalogo[1]);

        Assert.Equal(2, _usuario.ContarJuegos());
    }

    [Fact]
    public void ContarJuegos_SinColecciones_Retorna0()
    {
        Assert.Equal(0, _usuario.ContarJuegos());
    }

    [Fact]
    public void ObtenerColeccion_ColeccionConJuego_DevuelveLaListaConSusJuegos()
    {
        var usuario = DatosPrueba.UsuarioConColecciones();

        var lista = usuario.ObtenerColeccion("Pendientes");

        Assert.Single(lista);
        Assert.Equal("Celeste", lista[0].Titulo);
    }

    [Fact]
    public void ObtenerColeccion_ColeccionInexistente_LanzaKeyNotFoundException()
    {
        Assert.Throws<KeyNotFoundException>(() => _usuario.ObtenerColeccion("NoExiste"));
    }

    [Fact]
    public void EliminarColeccion_ColeccionExistente_RetornaTrueLaPrimeraVezYFalseLaSegunda()
    {
        _usuario.CrearColeccion("Pendientes");

        bool primera = _usuario.EliminarColeccion("Pendientes");
        bool segunda = _usuario.EliminarColeccion("Pendientes");

        Assert.True(primera);
        Assert.False(segunda);
    }

    [Fact]
    public void CrearColeccion_EnUnUsuario_NoLaCreaEnOtroUsuario()
    {
        var otro = new Usuario("Beto");

        _usuario.CrearColeccion("Pendientes");

        Assert.True(_usuario.Colecciones.ContainsKey("Pendientes"));
        Assert.False(otro.Colecciones.ContainsKey("Pendientes"));
    }
}
