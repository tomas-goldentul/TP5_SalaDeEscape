class Sala6
{
    string[] colores = { "red", "blue", "green", "yellow" };
    public bool perdio {get; set;} = false;
    public List<string> letrasRandom { get; set; } = new List<string>();
    public List<string> letrasIngresadas { get; set; } = new List<string>();
    public int jugadas {get;set;} = 0;

    public void crearRandom()
    {
        Random rd = new Random();
        int NumRandom = rd.Next(colores.Length);

    }

    public bool juego()
    {
        letrasRandom.Clear();

        for (int i = 0; i < letrasIngresadas.Count; i++)
        {
            if (letrasIngresadas[i] != letrasRandom[i])
                return perdio = true;
                jugadas++;
        }
        return perdio = false;
    }
}