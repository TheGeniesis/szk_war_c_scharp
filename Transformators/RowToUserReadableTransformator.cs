namespace Checkers.Transformators
{
    public class RowToUserReadableTransformator
    {
        public string Transform(int number)
        {
            //in ASCII 97 is an "a" letter
            return ((char)(number + 96)).ToString();
        }
    }
}
