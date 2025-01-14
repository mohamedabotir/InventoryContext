using Common.Result;

namespace Domain.ValueObject;

public sealed class Description(string description) : Common.ValueObject.ValueObject<Description>
{
    public string DescriptionValue { get;} = description;

    protected override bool EqualsCore(Description other)
    {
        return other.DescriptionValue == DescriptionValue;
    }

    public static Result<Description> CreateInstance(Maybe<string> description)
    {
       return description.ToResult("description Should Not Be Null")
            .Ensure(e=>e.Length>50,"Description must be at least 50 characters")
            .Ensure(e=>e.Length<500,"Description must be at most 500 characters")
            .Map(e=>new Description(e));
    }

    protected override int GetHashCodeCore()
    {
        return GetHashCode();
    }
}