using AlertasBC.Model;
using AlertasBC.Model.Utils;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertasBC.Repository
{
    public class DependencyRepository : BaseRepository
    {

        public async Task<RepositoryResponse<List<Dependency>>> GetDependencies()
        {
            var response = new RepositoryResponse<List<Dependency>>
            {
                Data = new List<Dependency>()
            };
            try
            {

                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("Dependency");
                IEnumerable<ParseObject> result = await query.FindAsync();

                foreach (ParseObject res in result)
                {
                    response.Data.Add(new Dependency() { ID = res.ObjectId, NAME = res.Get<String>("Name") });
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

        public async Task<RepositoryResponse<ParseObject>> GetDependencyById(string id)
        {
            var response = new RepositoryResponse<ParseObject>{ };

            try
            {

                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("Dependency");
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

        public async Task<RepositoryResponse<Dependency>> Get(string objectID)
        {
            var response = new RepositoryResponse<Dependency>();
            try
            {
                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("Dependency");
                ParseObject result = await query.GetAsync(objectID);
                response.Success = true;
                response.Data = new Dependency { ID = result.ObjectId, NAME = result["Name"].ToString() };
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
