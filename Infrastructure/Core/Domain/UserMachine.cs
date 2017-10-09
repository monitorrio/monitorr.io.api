namespace Core.Domain
{
    public class UserMachine : MongoEntity
    {
        public virtual string IP { get; set; }
        public virtual string MachineName { get; set; }
        public virtual string Browser { get; set; }
        public virtual string OS { get; set; }
    }
}
