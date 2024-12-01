namespace CountriesProject.Models.Country
{
    // for deserialization purposes 
    public class Country
    {
        public CountryName Name { get; set; }
        public List<string> Capital { get; set; }
        public List<string> Borders { get; set; }
    }
}
