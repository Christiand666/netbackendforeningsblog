namespace netbackendforeningsblog.Authorization;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }
// custom allowAnonymousattribute (there is a standard one)
// skips authorization if the action method is decorated with [AllowAnonymous].