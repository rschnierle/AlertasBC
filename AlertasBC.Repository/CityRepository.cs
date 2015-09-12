using AlertasBC.Model;
using AlertasBC.Model.Utils;
using Parse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlertasBC.Repository
{
    public class CityRepository : BaseRepository
    {

        public async Task<RepositoryResponse<List<City>>> GetCities()
        {
            var response = new RepositoryResponse<List<City>>
            {
                Data = new List<City>()
            };
            try
            {

                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("City");
                IEnumerable<ParseObject> result =  await query.FindAsync();

                foreach(ParseObject res in result)
                {
                    response.Data.Add(new City(){ ID = res.ObjectId, NAME =  res.Get<String>("Name")});
                }

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex;
        }

            return response;
        }

        public async Task<RepositoryResponse<ParseObject>> GetCityById(string id)
        {
            var response = new RepositoryResponse<ParseObject>
            {
            };

            try
            {

                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("City");
                ParseObject result = await query.GetAsync(id);

                response.Data = result;

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex;
            }

            return response;
        }
    }
}
