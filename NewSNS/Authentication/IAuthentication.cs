namespace Authentication
{
    public interface IAuthentication
    {
        bool IsAuthenticated(object authInfo);
    }
}
