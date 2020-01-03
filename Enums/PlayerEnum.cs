namespace Checkers.Enums
{
    public enum PlayerEnum
    {
        Red,
        Blue
    }

    static class PlayerEnumMethods
    {
        public static PlayerEnum GetPlayer(string fieldName)
        {
            return fieldName.Contains("red") ? PlayerEnum.Red : PlayerEnum.Blue;
        }
    }
}
