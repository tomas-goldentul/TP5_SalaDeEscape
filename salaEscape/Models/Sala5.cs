namespace salaEscape.Models;

public class LetraResultado
{
    public char Letra { get; set; }
    public char Color { get; set; } // 'v' (verde), 'a' (amarillo), 'r' (rojo)
}

public class Sala5
{
    public const int Clave = 89264;
    public string palabraAdivinar { get; set; }
    public int intentos { get; set; }
    public List<string> palabrasJugadas { get; set; } = new List<string>();
    public int wordleActual { get; set; } = 1;

    public static bool Verificar(int clave) => clave == Clave;

    public static List<string> cargarPalabras() => new List<string>()
    {
        "manzana", "banana", "futbol", "perro", "raton"
    };

    public void crearRandom()
    {
        var lista = cargarPalabras().Except(palabrasJugadas).ToList();
        Random r = new Random();
        palabraAdivinar = lista[r.Next(lista.Count)];
    }

    public void siguienteWordle()
    {
        palabrasJugadas.Add(palabraAdivinar);
        wordleActual++;
        intentos = 0;
        crearRandom();
    }

    public List<LetraResultado> compararPalabra(string wordle)
    {
        var resultado = new List<LetraResultado>();

        for (int i = 0; i < palabraAdivinar.Length; i++)
        {
            var letra = wordle[i];
            char color;

            if (letra == palabraAdivinar[i])
                color = 'v';
            else if (palabraAdivinar.Contains(letra))
                color = 'a';
            else
                color = 'r';

            resultado.Add(new LetraResultado { Letra = letra, Color = color });
        }

        intentos++;
        return resultado;
    }

    public bool adivinaPalabra(string wordle) => wordle == palabraAdivinar;
}