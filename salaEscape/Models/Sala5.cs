namespace salaEscape.Models;

public class Sala5
{
    public const int Clave = 89264;
    public static string palabraAdivinar{get; set;}
     
    public static bool Verificar(int clave)
    {
        return clave == Clave;
    }
    static public List<string> cargarPalabras()
    {
        List<string> listaPalabras = new List<string>() { "manzana" };
        return listaPalabras;
    }
    public static  List<char> compararPalabra(string wordle){
        List<char> respuesta = new List<char>();
        for(int i = 0; i < palabraAdivinar.Count(); i++){
            if (palabraAdivinar[i] == wordle[i]){
                respuesta.Add(wordle[i]);
                respuesta.Add('v');
            } else if(palabraAdivinar.Contains(wordle[i])){
                    respuesta.Add(wordle[i]);
                    respuesta.Add('a');
                } else
                {
                   respuesta.Add(wordle[i]); 
                   respuesta.Add('r');
                }}
        
        return respuesta;
    }
    public static bool adivinaPalabra(string wordle){
        return wordle == palabraAdivinar;
    }
    public static void crearRandom()
    {
        cargarPalabras();
        int num;
        Random r = new Random();
        num = r.Next(0, cargarPalabras().Count);
        palabraAdivinar = cargarPalabras()[num];
    }
}