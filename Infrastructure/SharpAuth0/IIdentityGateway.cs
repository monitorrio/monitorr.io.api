namespace SharpAuth0
{
    public interface IIdentityGateway
    {
        Claim FindIdentiyClaims();
    }
}
