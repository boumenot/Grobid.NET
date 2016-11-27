using Grobid.NET.Feature;

namespace Grobid.NET.Contract
{
    interface IModel<T>
    {
        T Create(BlockState[] blockStates);
    }
}
