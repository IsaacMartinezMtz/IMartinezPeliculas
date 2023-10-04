namespace PL.Models
{
    public class Peliculas
    {
        public int id { get; set; }
        public string overview { get; set; }
        public string title { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }

        public List<object> PeliculasL { get; set; }

    }
}
