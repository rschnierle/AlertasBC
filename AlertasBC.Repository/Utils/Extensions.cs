using AlertasBC.Model;
using Parse;

namespace AlertasBC.Repository.Utils
{
    public static class Extensions
    {
        public static User ToUser(this ParseUser parseUser)
        {
            var relation = (ParseRelation<ParseObject>)parseUser["ID_Dependency"];
            var dep = relation.Query.FirstAsync().GetAwaiter().GetResult();

            var user = new User
            {
                ID = parseUser.ObjectId,
                ID_DEPENDENCY = dep.ObjectId,
                NAME = parseUser.ContainsKey("Name") ? parseUser["Name"].ToString() : parseUser.Username,
                USERNAME = parseUser.Username
            };

            return user;
        }
    }
}
