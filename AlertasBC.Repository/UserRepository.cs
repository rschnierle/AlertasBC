using AlertasBC.Model;
using AlertasBC.Model.Utils;
using Parse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlertasBC.Repository.Utils;

namespace AlertasBC.Repository
{
    public class UserRepository :BaseRepository
    {

        public RepositoryResponse<User> ValidateUser(User user)
        {
            var response = new RepositoryResponse<User> { };

            try
            {
                var objectUser= ParseUser.LogInAsync(user.USERNAME, user.PASSWORD).GetAwaiter().GetResult();
                response.Success = true;
                
                response.Data = objectUser.ToUser();

                // Login was successful.
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex;
            }

            return response;
        }

        public RepositoryResponse<User> Logout()
        {
            var response = new RepositoryResponse<User> { };

            try
            {
                ParseUser.LogOut();
                response.Success = true;
                
                // Logout was successful.
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex;
            }

            return response;
        }

        public async Task<RepositoryResponse<User>> RegisterUser(User user)
        {
            var response = new RepositoryResponse<User> { };

            try
            {
                var userObject = new ParseUser()
                {
                    Username = user.USERNAME,
                    Password = user.PASSWORD
                };
                userObject["Name"] = user.NAME;

                DependencyRepository DependencyContext = new DependencyRepository();

                RepositoryResponse<ParseObject> Dependency = await DependencyContext.GetDependencyById(user.ID_DEPENDENCY);

                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("Dependency");
                IEnumerable<ParseObject> result = await query.FindAsync();

                var relation = userObject.GetRelation<ParseObject>("ID_Dependency");
                relation.Add(Dependency.Data);

                await userObject.SignUpAsync();
                response.Success = true;
                response.Data = userObject.ToUser();
                // Register was successful.
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
