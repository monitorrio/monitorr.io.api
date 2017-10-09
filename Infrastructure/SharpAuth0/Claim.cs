namespace SharpAuth0
{
    public class Claim
    {
        public virtual string UserId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Picture { get; set; }
        public virtual string FamilyName { get; set; }
        public virtual string Gender{ get; set; }
        public virtual string GivenName { get; set; }
        public virtual string NickName { get; set; }
        public virtual string Provider { get; set; }
        public virtual string Connection { get; set; }
        public virtual string TokenId { get; set; }
        public virtual string AccessToken { get; set; }
    }
}
