
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using ServiceContracts.Helpers;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace Services
{
    public class PersonServices : IPersonsServiece
    {
        private readonly List<Person> people;
        private readonly ICountryService _countryService;
        public PersonServices()
        {
            people = new List<Person>();
            _countryService = new CountriesServices();
        }

        private PersonResponseDTO ConvertPersonToPersonResponse(Person person)
        {

            PersonResponseDTO personResponse = person.ToPersonResponse();
            //CountryResponseDTO countryObject = _countryService.GetCountryById(personResponse.CountryId);
            //personResponse.Country = countryObject.CountryName;

            personResponse.Country = _countryService.GetCountryById(personResponse.CountryId)?.CountryName;

            return personResponse;
        }




        public PersonResponseDTO AddPerson(PersonRequestDTO? PersonRequest)
        {
            // check if personRequest is null 
            if (PersonRequest == null) throw new ArgumentNullException(nameof(PersonRequest));
            //  if (string.IsNullOrEmpty(PersonRequest.Name)) throw new ArgumentException("Person Name can not be blank");

            // Model Validations 
            ValidationHelper.ModelValidation(PersonRequest);

           Person person = PersonRequest.ToPerson();
            person.Id = Guid.NewGuid();

            people.Add(person);
            PersonResponseDTO personResponse = ConvertPersonToPersonResponse(person);


            return personResponse;
        }

        public List<PersonResponseDTO> GetAllPersons()
        {
         List<PersonResponseDTO> result = people.Select(temp => temp.ToPersonResponse()).ToList();
       
            return result;

        }

        public PersonResponseDTO? GetPersonById(Guid? Id)
        {
            if (Id is null) { return null; }
          Person? person =  people.FirstOrDefault(temp => temp.Id == Id);

         if (person == null) return null;



          return person.ToPersonResponse();


        }

        public List<PersonResponseDTO> GetFilteredPersons(string SearchBy, string? SearchString)
        {

            List<PersonResponseDTO> allPersons = GetAllPersons();
            List<PersonResponseDTO> matchingPersonos = allPersons; 

            if (string.IsNullOrEmpty(SearchString)|| string.IsNullOrEmpty(SearchBy)) { return matchingPersonos; }
             
            switch (SearchBy)
            {
                case nameof(Person.Name):
                    matchingPersonos = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Name)?
                    temp.Name.Contains(SearchString ,
                    StringComparison.OrdinalIgnoreCase):true)).ToList();
                    break;


                case nameof(Person.Email):
                    matchingPersonos = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) :
                    true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersonos = allPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(SearchString,StringComparison.OrdinalIgnoreCase):
                    true).ToList();
                    break;


                case nameof(Person.Gender):
                    matchingPersonos = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(SearchString, StringComparison.OrdinalIgnoreCase) :
                    true)).ToList();
                    break;



                case nameof(Person.CountryId):
                    matchingPersonos = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(SearchString) :
                    true)).ToList();
                    break;


                case nameof(Person.Address):
                    matchingPersonos = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(SearchString) :
                    true)).ToList();
                    break;

                default:matchingPersonos = allPersons;
                        break;

            }
            return matchingPersonos;
        
        }

        public List<PersonResponseDTO> GetSortedPersons(List<PersonResponseDTO> allPersons , string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) { return allPersons; }

            List<PersonResponseDTO> SortedPersons = (sortBy, sortOrder)
                switch
            {
            
                (nameof(PersonResponseDTO.Name) , SortOrderOptions.Ascending)=>
                allPersons.OrderBy(temp => temp.Name , StringComparer.OrdinalIgnoreCase)
                .ToList(),

                (nameof(PersonResponseDTO.Name), SortOrderOptions.Descending)=>
                allPersons.OrderByDescending(temp => temp.Name , StringComparer.OrdinalIgnoreCase)
                .ToList(),


                (nameof(PersonResponseDTO.Email) , SortOrderOptions.Ascending)=>
                allPersons.OrderBy(temp => temp.Email , StringComparer.OrdinalIgnoreCase)
                .ToList(),

                (nameof(PersonResponseDTO.Email) , SortOrderOptions.Descending)=>
                allPersons.OrderByDescending(temp => temp.Email , StringComparer.OrdinalIgnoreCase)
                .ToList(),


                (nameof(PersonResponseDTO.DateOfBirth) ,SortOrderOptions.Ascending )=>
                allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                
                
                (nameof(PersonResponseDTO.DateOfBirth) ,SortOrderOptions.Descending )=>
                allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                
                

                (nameof(PersonResponseDTO.Age) ,SortOrderOptions.Ascending )=>
                allPersons.OrderBy(temp => temp.Age).ToList(),
                
                
                (nameof(PersonResponseDTO.Age) ,SortOrderOptions.Descending )=>
                allPersons.OrderByDescending(temp => temp.Age).ToList(),
                
                
                (nameof(PersonResponseDTO.Gender) ,SortOrderOptions.Ascending )=>
                allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                
                
                (nameof(PersonResponseDTO.Gender) ,SortOrderOptions.Descending )=>
                allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                

                  
                (nameof(PersonResponseDTO.Address) ,SortOrderOptions.Ascending )=>
                allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                
                
                (nameof(PersonResponseDTO.Address) ,SortOrderOptions.Descending )=>
                allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                


                
                (nameof(PersonResponseDTO.Country) ,SortOrderOptions.Ascending )=>
                allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                
                
                (nameof(PersonResponseDTO.Country) ,SortOrderOptions.Descending )=>
                allPersons.OrderByDescending(temp => temp.Country).ToList(),






                (nameof(PersonResponseDTO.ReceiveNewsLetters), SortOrderOptions.Ascending) =>
                allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),


                (nameof(PersonResponseDTO.ReceiveNewsLetters), SortOrderOptions.Descending) =>
                allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),




                _=> allPersons
                
            };

            return SortedPersons;

        }

        public PersonResponseDTO UpdatePerson(PersonUpdateRequest? person)
        {
           if (person is null) { throw new ArgumentNullException(nameof(person)); }

            // validation
            ValidationHelper.ModelValidation(person);

            // get matching person object to update 
            Person? MatchingPerson = people.FirstOrDefault(temp => temp.Id == person.Id);

            if (MatchingPerson is null) { throw new ArgumentException("Given Person id does not exist "); }

            MatchingPerson.Name = person.Name;
            MatchingPerson.Gender =person.Gender.ToString();
            MatchingPerson.Address = person.Address;
            MatchingPerson.CountryId = person.CountryId;
            MatchingPerson.DateOfBirth = person.DateOfBirth;
            MatchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;
            MatchingPerson.Email = person.Email;

            return MatchingPerson.ToPersonResponse();



        }

        public bool DeletePerson(Guid? PersonId)
        {
            if (PersonId is null) throw new ArgumentNullException(nameof(PersonId));
            Person? matchingPerson = people.FirstOrDefault(temp => temp.Id == PersonId);
            if (matchingPerson is null) return false;

            people.RemoveAll(temp => temp.Id == PersonId);
            return true;

        }
    }
}
