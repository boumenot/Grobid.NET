namespace Grobid.NET.Contract
{
    public interface IFeatureRowStringJoiner
    {
        string Join(FeatureRow[] featureRows);
    }
}