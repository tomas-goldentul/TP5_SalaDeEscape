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
        if (palabra == palabraAdivinar)
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
//"perro", "cielo", "rojo", "feliz", "gato", "casa", "montana", "elefante", "pelota", "tren", "pirata", "corazon", "jardin", "mariposa", "nube", "avion", "telefono", "sombrero", "pintura", "zorro", "escuela", "planeta", "bosque", "estrella", "robot", "murcielago", "barco", "caramelo", "raton", "sol", "campana", "castillo", "espada", "relampago", "mochila", "piedra", "cuchara", "reloj", "dragon", "pajaro", "tigre", "cocodrilo", "libro", "pluma", "bandera", "diamante", "luna", "naranja", "fuego", "sombra", "ciudad", "puente", "playa", "caballo", "pez", "hoja", "arcilla", "roca", "espiral", "flor", "nieve", "hormiga", "musica", "danza", "ladrillo", "campamento", "volcan", "caminante", "fantasma", "carnaval", "vacaciones", "salto", "cable", "paracaidas", "sorpresa", "cuento", "cinturon", "rafaga", "latigo", "almohada", "sandia", "tobogan", "avellana", "bocadillo", "caracol", "aguacate", "limon", "tiburon", "escudo", "bruja", "trueno", "avellano", "maraton", "policia", "murmullo", "heroe", "fresquito", "horizonte", "sismografo", "oscuridad", "pendulo", "marioneta", "bilingue", "ortografia", "arcoiris", "buho", "aracnido", "prisma", "neblina", "pantano", "pasaporte", "jirafa", "cintillo", "relieve", "deportivo", "fantastico", "lapicero", "cascada", "silueta", "crater", "magia", "calculo", "naturaleza", "bumeran", "brujula", "serpiente", "telescopio", "eclipse", "veranillo", "caballero", "armadura", "compas", "camaleon", "almendra", "simbolico", "conspiracion", "revelacion", "anecdota", "inocente", "prodigio", "espontaneo", "aventura", "sorprendente", "desafio", "intrepido", "campeon", "valiente", "explorador", "pionero", "revolucion", "imaginacion", "creatividad", "descubrimiento", "sabiduria", "justicia", "armonia", "esperanza", "ternura", "compasion", "resplandor", "transformacion", "suenos", "vision", "realidad", "utopia", "proposito", "destino", "leyenda", "misterio", "enigmas", "conocimiento", "icono", "ritmo", "danza", "fraternidad", "esencia", "reminiscencia", "innovacion", "trascendencia", "equilibrio", "universo", "expansion", "paradoja", "sensacion", "emocion", "intensidad", "pasion", "maravilla", "unico"

}