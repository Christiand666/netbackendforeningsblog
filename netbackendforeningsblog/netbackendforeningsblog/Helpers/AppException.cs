namespace netbackendforeningsblog.Helpers;

using System.Globalization;

// custom exception class for throwing specific messages for handelt and unhandlet exceptions
public class AppException : Exception
{
   
    public AppException(string message) : base(message) { }

}