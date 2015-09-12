using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlertasBC.Repository;
using AlertasBC.Model;
using AlertasBC.Model.Utils;

namespace AlertasBC.Test.Repository
{
    [TestClass]
    public class UserRepositoryTest
    {
        [TestMethod]
        [TestCategory("Repository")]
        public void ValidateUser_Success()
        {
            UserRepository repository = new UserRepository();

            User user = new User
            {
               USERNAME= "tizarro",
               PASSWORD = "tizarro"
            };

            RepositoryResponse<User> expected = new RepositoryResponse<User>
            {
                Success = true,
                Data = user
            };

            RepositoryResponse<User> result = repository.ValidateUser(user);

            Assert.IsTrue(result.Success, "User not valid");
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error, "Some error ocurred");
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void RegisterUser_Success()
        {
            UserRepository repository = new UserRepository();

            User user = new User
            {
                USERNAME = string.Format("UT_{0}",DateTime.Now.Ticks),
                PASSWORD = "123",
                NAME = "Unit Test User",
                ID_DEPENDENCY= "iIAW2gamws"

            };

            RepositoryResponse<User> expected = new RepositoryResponse<User>
            {
                Success = true,
                Data = user
            };

            RepositoryResponse<User> result = repository.RegisterUser(user).GetAwaiter().GetResult();

            Assert.IsTrue(result.Success, "User not registered");
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error, "Some error ocurred");
        }
    }
}
