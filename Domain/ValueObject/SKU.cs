using Common.Result;
using Common.Utils;

namespace Domain.ValueObject;

public sealed class SKU : Common.ValueObject.ValueObject<SKU>
{
    private SKU(string skuValue)
    {
        SKUValue = skuValue;
    }

    public string SKUValue { get;}
    protected override bool EqualsCore(SKU other)
    {
       return SKUValue == other.SKUValue;
    }

    public static SKU CreateInstance()
    {
        var skuNumber = NumberGeneratorBase.CreateGenerator(NumberGenerator.Sku)
            .GenerateNumber();
      return  new SKU(skuNumber);
    }

    public static Result<SKU> IsValidSku(Maybe<string> sku)
    {
        var result = sku.ToResult("SKU cannot be null or empty.")
            .Ensure(e=> NumberGeneratorBase.CreateGenerator(NumberGenerator.Sku).IsValidNumber(e),"Invalid sku.")
            .Map(e=>new SKU(e));

        return result;
    }

    protected override int GetHashCodeCore()
    {
      return  GetHashCode();
    }
}