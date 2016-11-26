namespace Grobid.NET.Contract
{
    interface IModel<T>
    {
        T Create(BlockState[] blockStates);
    }
}
