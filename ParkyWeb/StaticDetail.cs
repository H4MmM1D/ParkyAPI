using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public class StaticDetail
    {
        public static string APIBaseUrl = "http://localhost:53018/";
        public static string NationalParkAPIPath = APIBaseUrl + "api/v1/nationalpark/";
        public static string TrailAPIPath = APIBaseUrl + "api/v1/trail/";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/user/";
    }
}
