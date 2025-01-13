using Common.Result;

namespace Domain.ValueObject;

public sealed class Name: Common.ValueObject.ValueObject<SKU>
{
    public string NameValue { get; }

    private Name(string name)
    {
        NameValue = name;
    }
    protected override bool EqualsCore(SKU other)
    {
      return other.SKUValue == this.NameValue;
    }

    public static Result<Name> CreateInstance(Maybe<string> name)
    {
       return name.ToResult("Name Should Not Be Null")
            .Ensure(e=>e.Length>10,"Name Must be at least 10 characters")
            .Ensure(e=>e.Length>126,"Name must be at most 126 characters")
            .Map(e=>new Name(e));
    }

    protected override int GetHashCodeCore()
    {
        return GetHashCode();
    }
}