namespace salaEscape.Models;

public class Sala6
{
    private readonly char[] colores = { 'R', 'B', 'G', 'Y' };
    public bool Perdio { get; private set; } = false;
    public List<char> LetrasRandom { get; private set; } = new List<char>();
    public List<char> LetrasIngresadas { get; private set; } = new List<char>();
    public int Jugadas { get; private set; } = 0;

    public void CrearRandom()
    {
        Random rd = new Random();
        int numRandom = rd.Next(colores.Length);
        LetrasRandom.Add(colores[numRandom]);
    }

    public bool Jugar(char letra)
    {
        CrearRandom();
        LetrasIngresadas.Add(char.ToUpper(letra));
        Jugadas++;

        for (int i = 0; i < LetrasIngresadas.Count; i++)
        {
            if (LetrasIngresadas[i] != LetrasRandom[i])
            {
                Perdio = true;
                return true;
            }
        }

        return false;
    }

    public bool HaGanado()
    {
        return !Perdio && Jugadas >= 5;
    }

    public void Reiniciar()
    {
        Perdio = false;
        LetrasRandom.Clear();
        LetrasIngresadas.Clear();
        Jugadas = 0;
    }

    public List<char> devolverLetrasRandom()
    {
        return LetrasRandom;
    }

    public List<char> devolverLetrasIngresadas()
    {
        return LetrasIngresadas;
    }
}