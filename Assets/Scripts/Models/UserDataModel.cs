namespace CharacterThemeShop.Models
{
    public class UserDataModel
    {
        public int userCurrentLevel;
        public int userCurrentCharacterId;

        public void SetDefaultUserData()
        {
            userCurrentLevel = 0;
            userCurrentCharacterId = 0;
        }
    }
}