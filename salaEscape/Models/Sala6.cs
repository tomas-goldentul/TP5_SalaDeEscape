class Sala6
{
    char[] colores = { 'R', 'B', 'G', 'Y' };
    public bool perdio {get; set;} = false;
    public List<char> letrasRandom { get; set; } = new List<char>();
    public List<char> letrasIngresadas { get; set; } = new List<char>();
    public int jugadas {get;set;} = 0;

    public void crearRandom()
    {
        Random rd = new Random();
        int NumRandom = rd.Next(colores.Length);
        letrasRandom.Add(colores[NumRandom]);

    }

    public bool juego(char letra)
    {            crearRandom();

         letrasIngresadas.Add(letra);
        for (int i = 0; i < letrasIngresadas.Count; i++)
        {
            if (letrasIngresadas[i] != letrasRandom[i])
                return perdio = true;
                jugadas++;
        }
        return perdio;
        
    }
    public List<char> devolverLetrasRandom(){
        return letrasRandom;
    }


public List<char> devolverLetrasIngresadas(){
    return letrasIngresadas;
}
}