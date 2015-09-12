using System;
using AlertasBC.Model;
using AlertasBC.Model.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Parse;

namespace AlertasBC.Repository
{
    public class NotificationRepository : BaseRepository
    {

        public async Task<RepositoryResponse<Notification>> AddNotification(Notification notification)
        {

            var response = new RepositoryResponse<Notification> { };
            try
            {
                var Noti = Parse.ParseObject.Create("Notification");

                

                #region Add Dependency Relationship

                DependencyRepository DependencyContext = new DependencyRepository();

                RepositoryResponse<ParseObject> Dependency = await DependencyContext.GetDependencyById(notification.ID_DEPENDENCY);

                ParseQuery<ParseObject> query = new ParseQuery<ParseObject>("Dependency");
                IEnumerable<ParseObject> result = await query.FindAsync();

                #endregion

                #region Add City Relationship

                CityRepository CityContext = new CityRepository();

                RepositoryResponse<ParseObject> City = await CityContext.GetCityById(notification.ID_CITY);

                query = new ParseQuery<ParseObject>("City");
                result = await query.FindAsync();

                #endregion

                var relation = Noti.GetRelation<ParseObject>("ID_Dependency");
                relation.Add(Dependency.Data);

                relation = Noti.GetRelation<ParseObject>("ID_City");
                relation.Add(City.Data);

                var message = string.Format("{0} > {1}: {2}", Dependency.Data["Name"].ToString(), City.Data["Name"].ToString(), notification.NOTIFICATION_TEXT);

                Noti.Add("NotificationText", message);
                await Noti.SaveAsync();

                await Noti.SaveAsync();

                var push = new ParsePush();
                push.Query = from installation in ParseInstallation.Query
                             where installation.Get<string>("City").Contains(notification.ID_CITY)
                             select installation;
                push.Alert = message;
                await push.SendAsync();

                notification.ID = Noti.ObjectId;
                response.Data = notification;

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex;
            }

            return response;
        }

        public async Task<RepositoryResponse<List<Notification>>> AddNotifications(List<Notification> notifications)
        {
            var response = new RepositoryResponse<List<Notification>>
            {
                Success = true,
                Data = new List<Notification>()
            };

            if (notifications.Count == 0)
            {
                response.Success = false;
                response.Error = new Exception("There are no notifications to add.");
                return response;
            }

            foreach (var notification in notifications)
            {
                var result = await AddNotification(notification);
                if (result.Success)
                {
                    response.Data.Add(notification);
                }
                else
                {
                    response.Success = false;
                    response.Error = result.Error;
                    break;
                }
            }

            return response;
        }
    }
}
