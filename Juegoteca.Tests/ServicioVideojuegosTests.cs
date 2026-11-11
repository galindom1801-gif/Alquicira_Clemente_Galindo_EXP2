using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;
using Juegoteca.Tests.Datos;

namespace Juegoteca.Tests;

public class ServicioVideojuegosTests
{
    private readonly GestorVideojuegos _gestor;
    private readonly ExportadorPrueba _exportador;
    private readonly ServicioVideojuegos _servicio;

    public ServicioVideojuegosTests()
    {
        _gestor = DatosPrueba.GestorPoblado();
        _exportador = new ExportadorPrueba();
        _servicio = new ServicioVideojuegos(_gestor, _exportador);
    }

    [Fact]
    public void Constructor_DependenciasValidas_AsignaGestorExportadorYUsuariosVacia()
    {
        Assert.Same(_gestor, _servicio.Gestor);
        Assert.Same(_exportador, _servicio.Exportador);
        Assert.NotNull(_servicio.Usuarios);
        Assert.Empty(_servicio.Usuarios);
    }

    [Fact]
    public void Constructor_GestorNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ServicioVideojuegos(null!, _exportador));
    }

    [Fact]
    public void Constructor_ExportadorNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new ServicioVideojuegos(_gestor, null!));
    }

    [Fact]
    public void RegistrarUsuario_NombreValido_RetornaUsuarioLoGuardaYContarPasaDe0a1()
    {
        Assert.Equal(0, _servicio.ContarUsuarios());

        var usuario = _servicio.RegistrarUsuario("Ana");

        Assert.Equal("Ana", usuario.Nombre);
        Assert.Contains(usuario, _servicio.Usuarios);
        Assert.Equal(1, _servicio.ContarUsuarios());
    }

    [Fact]
    public void RegistrarUsuario_NombreNull_LanzaArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _servicio.RegistrarUsuario(null!));
    }

    [Fact]
    public void BuscarUsuario_ANAEnMayusculas_EncuentraAlUsuarioAna()
    {
        var registrado = _servicio.RegistrarUsuario("Ana");

        var encontrado = _servicio.BuscarUsuario("ANA");

        Assert.Same(registrado, encontrado);
    }

    [Fact]
    public void BuscarUsuario_NombreNoRegistrado_RetornaNull()
    {
        Assert.Null(_servicio.BuscarUsuario("Nadie"));
    }

    [Fact]
    public void EliminarUsuario_UsuarioExistente_RetornaTrueYElConteoBajaA0()
    {
        _servicio.RegistrarUsuario("Ana");

        bool resultado = _servicio.EliminarUsuario("Ana");

        Assert.True(resultado);
        Assert.Equal(0, _servicio.ContarUsuarios());
    }

    [Fact]
    public void EliminarUsuario_UsuarioInexistente_RetornaFalse()
    {
        Assert.False(_servicio.EliminarUsuario("Nadie"));
    }

    [Theory]
    [InlineData("quick")]
    [InlineData("merge")]
    public void OrdenarCatalogo_AlgoritmoValido_TetrisQuedaPrimero(string algoritmo)
    {
        _servicio.OrdenarCatalogo(algoritmo);

        Assert.Equal("Tetris", _gestor.JuegosDisponibles[0].Titulo);
        Assert.Equal(8, _gestor.JuegosDisponibles.Count);
    }

    [Fact]
    public void OrdenarCatalogo_AlgoritmoBurbuja_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => _servicio.OrdenarCatalogo("burbuja"));
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_HadesYCeleste_Retorna33YCelesteQuedaPrimero()
    {
        var usuario = _servicio.RegistrarUsuario("Ana");
        usuario.CrearColeccion("Pendientes");
        usuario.AgregarJuegoAColeccion("Pendientes", new Videojuego("Hades", "Supergiant", 25));
        usuario.AgregarJuegoAColeccion("Pendientes", new Videojuego("Celeste", "Maddy Makes Games", 8));

        int total = _servicio.OrdenarColeccionYCalcularHoras(usuario, "Pendientes");

        Assert.Equal(33, total);
        Assert.Equal("Celeste", usuario.ObtenerColeccion("Pendientes")[0].Titulo);
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_ConMergeSort_Retorna33YCelesteQuedaPrimero()
    {
        var usuario = _servicio.RegistrarUsuario("Ana");
        usuario.CrearColeccion("Pendientes");
        usuario.AgregarJuegoAColeccion("Pendientes", new Videojuego("Hades", "Supergiant", 25));
        usuario.AgregarJuegoAColeccion("Pendientes", new Videojuego("Celeste", "Maddy Makes Games", 8));

        int total = _servicio.OrdenarColeccionYCalcularHoras(usuario, "Pendientes", "merge");

        Assert.Equal(33, total);
        Assert.Equal("Celeste", usuario.ObtenerColeccion("Pendientes")[0].Titulo);
    }

    [Fact]
    public void OrdenarColeccionYCalcularHoras_ColeccionInexistente_LanzaKeyNotFoundException()
    {
        var usuario = _servicio.RegistrarUsuario("Ana");

        Assert.Throws<KeyNotFoundException>(() =>
            _servicio.OrdenarColeccionYCalcularHoras(usuario, "NoExiste"));
    }

    [Fact]
    public void ExportarColecciones_ConExportadorPrueba_LlamaUnaVezConElMismoUsuarioYRuta()
    {
        var usuario = DatosPrueba.UsuarioConColecciones();

        _servicio.ExportarColecciones(usuario, "salida.txt");

        Assert.Equal(1, _exportador.Llamadas);
        Assert.Same(usuario, _exportador.UltimoUsuario);
        Assert.Equal("salida.txt", _exportador.UltimaRuta);
    }
}
