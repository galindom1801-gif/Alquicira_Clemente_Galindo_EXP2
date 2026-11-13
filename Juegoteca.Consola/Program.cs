using System.Text;
using Juegoteca.Biblioteca.Gestores;
using Juegoteca.Biblioteca.Modelos;
using Juegoteca.Biblioteca.Servicios;

Console.OutputEncoding = Encoding.UTF8;

var gestor = new GestorVideojuegos();
var exportador = new ExportadorTxt();
var servicio = new ServicioVideojuegos(gestor, exportador);

gestor.AgregarJuego(new Videojuego("Celeste", "Maddy Makes Games", 8));
gestor.AgregarJuego(new Videojuego("Hollow Knight", "Team Cherry", 40));
gestor.AgregarJuego(new Videojuego("Minecraft", "Mojang", 100));
gestor.AgregarJuego(new Videojuego("Tetris", "The Tetris Company", 5));
gestor.AgregarJuego(new Videojuego("Portal 2", "Valve", 10));
gestor.AgregarJuego(new Videojuego("Stardew Valley", "ConcernedApe", 60));
gestor.AgregarJuego(new Videojuego("Hades", "Supergiant", 25));
gestor.AgregarJuego(new Videojuego("Zelda: BOTW", "Nintendo", 120));

Console.WriteLine("Bienvenido al Sistema de Gestión de Videojuegos");
Console.WriteLine();

// 2. Registro de usuario 
Console.WriteLine("    REGISTRO DE USUARIO    ");
string nombreUsuario = LeerTextoNoVacio("Ingrese su nombre de usuario: ");
var usuario = servicio.RegistrarUsuario(nombreUsuario);
Console.WriteLine($"Usuario '{usuario.Nombre}' registrado. ¡Bienvenido/a, {usuario.Nombre}!");

// Creación de la colección inicial
string coleccionActual = "";
while (coleccionActual == "")
{
    string nombreColeccion = LeerTextoNoVacio("Nombre de su primera colección: ");
    try
    {
        usuario.CrearColeccion(nombreColeccion);
        coleccionActual = nombreColeccion;
        Console.WriteLine($"Colección '{nombreColeccion}' creada exitosamente.");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

bool salir = false;
while (!salir)
{
    Console.WriteLine();
    Console.WriteLine("    MENÚ PRINCIPAL    ");
    Console.WriteLine($"Colección actual: '{coleccionActual}' ({usuario.ObtenerColeccion(coleccionActual).Count} juegos)");
    Console.WriteLine("1. Mostrar juegos disponibles");
    Console.WriteLine("2. Ordenar colección (QuickSort o MergeSort)");
    Console.WriteLine("3. Búsqueda binaria en catálogo");
    Console.WriteLine("4. Crear nueva colección");
    Console.WriteLine("5. Cambiar de colección actual");
    Console.WriteLine("6. Ver mis colecciones");
    Console.WriteLine("7. Exportar mis colecciones a .txt");
    Console.WriteLine("8. Salir");
    Console.Write("Seleccione una opción: ");

    if (!int.TryParse(Console.ReadLine(), out int opcion))
    {
        Console.WriteLine("Entrada inválida. Escriba un número del 1 al 8.");
        continue;
    }

    switch (opcion)
    {
        case 1:
        {
            var enColeccion = usuario.ObtenerColeccion(coleccionActual);
            Console.WriteLine("Juegos que se pueden incluir en la colección actual:");
            int numero = 0;
            foreach (var juego in gestor.JuegosDisponibles)
            {
                if (enColeccion.Contains(juego))
                    continue;
                numero++;
                Console.WriteLine($"  {numero}. {juego}");
            }
            if (numero == 0)
                Console.WriteLine("  (no hay juegos disponibles: todos ya están en la colección)");
            break;
        }

        case 2:
        {
            Console.WriteLine("1. QuickSort");
            Console.WriteLine("2. MergeSort");
            Console.Write("Elija el algoritmo: ");
            if (!int.TryParse(Console.ReadLine(), out int algoritmo) || (algoritmo != 1 && algoritmo != 2))
            {
                Console.WriteLine("Opción de algoritmo inválida.");
                break;
            }

            int totalHoras = servicio.OrdenarColeccionYCalcularHoras(
                usuario, coleccionActual, algoritmo == 1 ? "quick" : "merge");

            Console.WriteLine($"Colección '{coleccionActual}' ordenada por horas de juego (menor a mayor):");
            foreach (var juego in usuario.ObtenerColeccion(coleccionActual))
                Console.WriteLine($"  - {juego}");
            Console.WriteLine($"Total de horas de juego: {totalHoras} h");
            break;
        }

        case 3:
        {
            Console.Write("Título a buscar: ");
            string texto = (Console.ReadLine() ?? "").Trim();

            var coincidencias = gestor.BuscarPorTitulo(texto);
            if (coincidencias.Count == 0)
            {
                Console.WriteLine("No se encontraron coincidencias.");
                break;
            }

            if (gestor.BusquedaBinaria(texto) >= 0)
                Console.WriteLine("La búsqueda binaria encontró una coincidencia exacta.");

            for (int i = 0; i < coincidencias.Count; i++)
                Console.WriteLine($"  {i + 1}. {coincidencias[i]}");

            Console.Write("Número del juego a agregar (0 para cancelar): ");
            if (!int.TryParse(Console.ReadLine(), out int seleccion))
            {
                Console.WriteLine("Entrada inválida.");
            }
            else if (seleccion == 0)
            {
                Console.WriteLine("Operación cancelada.");
            }
            else if (seleccion < 0 || seleccion > coincidencias.Count)
            {
                Console.WriteLine("Número fuera de rango.");
            }
            else
            {
                var elegido = coincidencias[seleccion - 1];
                if (usuario.ObtenerColeccion(coleccionActual).Contains(elegido))
                {
                    Console.WriteLine($"'{elegido.Titulo}' ya está en la colección '{coleccionActual}'.");
                }
                else
                {
                    usuario.AgregarJuegoAColeccion(coleccionActual, elegido);
                    Console.WriteLine($"'{elegido.Titulo}' agregado a la colección '{coleccionActual}'.");
                }
            }
            break;
        }

        case 4:
        {
            string nueva = LeerTextoNoVacio("Nombre de la nueva colección: ");
            try
            {
                usuario.CrearColeccion(nueva);
                Console.WriteLine($"Colección '{nueva}' creada exitosamente.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            break;
        }

        case 5:
        {
            Console.WriteLine("Sus colecciones:");
            foreach (var nombre in usuario.Colecciones.Keys)
                Console.WriteLine($"  - {nombre}");

            string elegida = LeerTextoNoVacio("Nombre de la colección que será la actual: ");
            string? clave = usuario.Colecciones.Keys
                .FirstOrDefault(k => string.Equals(k, elegida, StringComparison.OrdinalIgnoreCase));

            if (clave == null)
            {
                Console.WriteLine($"La colección '{elegida}' no existe.");
            }
            else
            {
                coleccionActual = clave;
                Console.WriteLine($"Colección actual: '{coleccionActual}'.");
            }
            break;
        }

        case 6:
            usuario.MostrarColecciones();
            break;

        case 7:
        {
            string rutaPredeterminada = $"colecciones_{usuario.Nombre}.txt";
            Console.Write($"Ruta del archivo (Enter para '{rutaPredeterminada}'): ");
            string ruta = (Console.ReadLine() ?? "").Trim();
            if (ruta == "")
                ruta = rutaPredeterminada;

            try
            {
                servicio.ExportarColecciones(usuario, ruta);
                Console.WriteLine($"Colecciones exportadas a: {Path.GetFullPath(ruta)}");
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
            {
                Console.WriteLine($"No se pudo exportar: {ex.Message}");
            }
            break;
        }

        case 8:
            salir = true;
            Console.WriteLine("¡Hasta pronto!");
            break;

        default:
            Console.WriteLine("Opción no válida. Escriba un número del 1 al 8.");
            break;
    }
}

// Funciones locales 
static string LeerTextoNoVacio(string mensaje)
{
    while (true)
    {
        Console.Write(mensaje);
        string texto = (Console.ReadLine() ?? "").Trim();
        if (texto != "")
            return texto;
        Console.WriteLine("El valor no puede estar vacío.");
    }
}
