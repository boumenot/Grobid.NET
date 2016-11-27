namespace Grobid.NET.Contract
{
    public interface ILexicon
    {
        bool IsFirstName(string name);
        bool IsLastName(string name);
        bool HasWord(string name);
    }
}
