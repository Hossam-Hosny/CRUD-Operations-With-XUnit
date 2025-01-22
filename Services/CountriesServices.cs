using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesServices : ICountryService 
    {
        private readonly List<Country> _countries;
        public CountriesServices()
        {
            _countries = new List<Country>();
        }



        public CountryResponseDTO AddCountry(CountryRequestDTO? dto)
        {
            if (dto == null) { throw new ArgumentNullException(nameof(dto)); }
            if (dto.CountryName is null) { throw new ArgumentException(nameof(dto.CountryName)); }
            if (_countries.Where(temp => temp.Name == dto.CountryName).Count() > 0) { throw new ArgumentException("Given country exist"); }
            Country country = dto.ToCountry();
            country.Id = Guid.NewGuid();
            
            _countries.Add(country);

            return country.ToCountryResponse();

        }

        public List<CountryResponseDTO> GetAllCountries()
        {
           return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponseDTO? GetCountryById(Guid? Id)
        {
           if (Id is null) { return null; }


          Country? country =
                _countries.FirstOrDefault(temp => temp.Id == Id);

            if (country is null) return null;

            return country.ToCountryResponse();


        }
    }
}
