using System.Collections.Generic;

namespace CharacterThemeShop.Models
{
    [System.Serializable]
    public class CharacterModel
    {
        public int characterId;
        public int unlockLevel;
        public string characterName;
    }

    [System.Serializable]
    public class CharacterList
    {
        public List<CharacterModel> characterList;
    }
}