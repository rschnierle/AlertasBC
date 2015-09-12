using AlertasBC.Model;
using AlertasBC.Model.Utils;
using AlertasBC.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace AlertasBC.Test
{
    [TestClass]
    public class NotificationRepositoryTest
    {
        [TestMethod]
        [TestCategory("Repository")]
        public void AddNotifications_Success()
        {
            NotificationRepository repository = new NotificationRepository();

            Notification notification = new Notification {
                NOTIFICATION_TEXT="Unit Test notification",
                ID_DEPENDENCY = "iIAW2gamws",
                ID_CITY = "vxwnP2GrHn"
            };

            RepositoryResponse<Notification> expected = new RepositoryResponse<Notification> { Success = true,
                Data= notification
            };

            RepositoryResponse<Notification> result = repository.AddNotification(notification).GetAwaiter().GetResult();

            Assert.IsTrue(result.Success, "Notifications not created");
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error,"Some error ocurred");
        }
    }
}
