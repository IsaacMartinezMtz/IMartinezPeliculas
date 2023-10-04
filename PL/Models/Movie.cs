namespace PL.Models
{
    public class Movie
    {
        public int IdMovie { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public  List<object> Movies { get; set; }
        public string date { get; set; }
    }
}
