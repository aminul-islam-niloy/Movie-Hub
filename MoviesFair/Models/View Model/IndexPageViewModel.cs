namespace MoviesFair.Models.View_Model
{
    public class IndexPageViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Movie> Action { get; set; }
        public IEnumerable<Movie> Horror { get; set; }
        public IEnumerable<Movie> Romantic { get; set; }
        public IEnumerable<Movie> Animation { get; set; }
        public IEnumerable<Movie> Sci_Fi { get; set; }
        public IEnumerable<Movie> War { get; set; }
        public IEnumerable<Movie> Advanture { get; set; }
        public IEnumerable<Movie> History { get; set; }
        public IEnumerable<Movie> Comedy { get; set; }
    }
}
