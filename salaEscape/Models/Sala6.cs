class Sala6
{
         string[] colores = { "red", "blue", "green", "yellow" };

        public List<string> letrasRandom { get; set; } = new List<string>();
        public List<string> letrasIngresadas { get; set; } = new List<string>();

        public void crearRandom()
        {
          Random rd = new Random();
            int NumRandom = rd.Next(colores.Length);
           
        }

        public bool juego()
        {
            for (int i = 0; i < letrasIngresadas.Count; i++)
            {
                if (letrasIngresadas[i] != letrasRandom[i])
                    return false;
            }
            return true;
        }
}