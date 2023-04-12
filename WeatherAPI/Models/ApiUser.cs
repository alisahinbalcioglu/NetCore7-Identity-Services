namespace WeatherAPI.Models
{
    public class ApiUser
    {
        public static List<ApiUsers> Users = new()
        {
            new ApiUsers {Id = 1,UserName="alisahin",Password="Passw0rd123.",Role="Admin"},
            new ApiUsers {Id = 2,UserName="ali",Password="123456",Role="Standart"},
        };
    }
}
