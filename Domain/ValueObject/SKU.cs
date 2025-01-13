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

    protected override int GetHashCodeCore()
    {
      return  GetHashCode();
    }
}