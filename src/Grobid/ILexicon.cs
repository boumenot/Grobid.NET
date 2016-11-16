namespace Grobid.NET
{
    public interface ILexicon
    {
        bool IsFirstName(string name);
        bool IsLastName(string name);
        bool HasWord(string name);
    }
}
