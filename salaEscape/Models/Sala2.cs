namespace salaEscape.Models;

public class Sala2
{
    public string palabraAdivinar { get; set; } = string.Empty;
    public List<char> letrasAdivinadas { get; set; } = new List<char>();
    public List<char> palabraJugada { get; set; } = new List<char>();
    public List<char> letrasErradas { get; set; } = new List<char>();

    public void crearRandom()
    {
        int num;
        Random r = new Random();
        num = r.Next(0, cargarPalabras().Count);
        palabraAdivinar = cargarPalabras()[num];
    }
    public bool ingresarLetra(char letra)
    {
        bool finalizo = false;
        palabraJugada.Clear();
            letra = char.ToLower(letra); 

        if (palabraAdivinar.Contains(letra))
        {
            letrasAdivinadas.Add(letra);
        }
        else if (!letrasErradas.Contains(letra))
        {
            letrasErradas.Add(letra);
        }

        for (int i = 0; i < palabraAdivinar.Length; i++)
        {
            if (letrasAdivinadas.Contains(palabraAdivinar[i]))
            {
                palabraJugada.Add(palabraAdivinar[i]);
            }
            else
            {
                palabraJugada.Add('_');
            }
        }
        if (!palabraJugada.Contains('_'))
        {
            finalizo = true;
        }
        return finalizo;
    }
    public bool ingresarPalabra(string palabra)
    {
        bool adivino = false;
        if (palabra.ToLower() == palabraAdivinar)
        {
            adivino = true;
        }
        return adivino;
    }
    public List<string> cargarPalabras()
    {
        List<string> listaPalabras = new List<string>() { "manzana" };
        return listaPalabras;
    }
}