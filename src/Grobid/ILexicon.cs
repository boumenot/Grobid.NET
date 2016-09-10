namespace Grobid.NET
{
    public interface ILexicon : org.grobid.core.lexicon.Lexicon
    {
        bool IsFirstName(string name);
        bool IsLastName(string name);
        bool HasWord(string name);
    }
}
