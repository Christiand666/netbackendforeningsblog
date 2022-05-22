namespace netbackendforeningsblog.Helpers;

public class AppSettings
{
    //used to dependency injection into other classes for mysecret generated key for the unique user
    public string Secret { get; set; }
}