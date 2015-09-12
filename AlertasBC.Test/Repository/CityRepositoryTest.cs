using AlertasBC.Model;
using AlertasBC.Model.Utils;
using AlertasBC.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertasBC.Test.Repository
{
    [TestClass]
    public class CityRepositoryTest
    {
        [TestMethod]
        [TestCategory("Repository")]
        public async void AddNotification_Success()
        {
            CityRepository repository = new CityRepository();

            City city = new City();
            //{
            //    NOTIFICATION_TEXT = "Unit Test notification",
            //    CREATED_DATE = DateTime.Now,
            //    ID_DEPENDENCY = "iIAW2gamws"
            //};

            RepositoryResponse<City> expected = new RepositoryResponse<City>
            {
                Success = true,
                Data = city
            };

            var cities = new List<City> { new City { ID = "GUf2kxo3Yl" } };

            RepositoryResponse<List<City>> result = await repository.GetCities();

            Assert.IsTrue(result.Success, "Notifications not created");
            Assert.IsNotNull(result.Data.Count > 0);
            Assert.IsNull(result.Error, "Some error ocurred");
        }
    }
}
