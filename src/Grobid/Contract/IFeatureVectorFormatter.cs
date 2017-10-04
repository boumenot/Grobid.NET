
namespace Grobid.NET.Contract
{
    public interface IFeatureVectorFormatter<in T>
    {
        string[] Format(T featureVector);
    }
}
