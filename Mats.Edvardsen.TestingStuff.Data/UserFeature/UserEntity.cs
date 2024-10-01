namespace Mats.Edvardsen.TestingStuff.Data.UserFeature;

public class UserEntity : IIdentifiableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public int Age { get; set; }
}