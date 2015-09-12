using System;
namespace AlertasBC.Model
{
    public class Notification
    {
        public string ID { get; set; }
        public string NOTIFICATION_TEXT { get; set; }
        public string ID_DEPENDENCY { get; set; }
        public string ID_CITY { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}
