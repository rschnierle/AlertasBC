
using AlertasBC.Model.Utils;
using Parse;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlertasBC.Repository
{
    public class BaseRepository
    {

        public BaseRepository()
        {
            Parse.ParseClient.Initialize("KOZTnHkdmrMemCqF8jZjmcOaUKwgIHAIclXUxEEh",
                   "HdRIJigwcuu6V9ijkUIAT50wDVE8RzSVcbAJXlic");
        }

    }
}
