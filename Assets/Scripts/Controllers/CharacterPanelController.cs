using UnityEngine;
using CharacterThemeShop.Views;
using CharacterThemeShop.Managers;

namespace CharacterThemeShop.Controllers
{
    public class CharacterPanelController : MonoBehaviour
    {
        #region ---------------------- Private Serialized Fields ----------------------
        [SerializeField] private GameController gameController;
        [SerializeField] private CharacterPanelView characterPanelView;
        #endregion
        #region ---------------------- Private NonSerialized Fields ----------------------
        private int currentCharacterId;
        #endregion


        #region ---------------------- Private Methods ----------------------
        private void SetPlayerData()
        {
            currentCharacterId = characterPanelView.SetCurrentCharacterDetails();
        }
        #endregion
        #region ---------------------- Public Methods ----------------------
        public void PopulateCharacters()
        {
            characterPanelView.StartPopulatingCharacters();
        }
        public void SetCharacterPanel()
        {
            SetPlayerData();
            characterPanelView.SetTab1Active();
            characterPanelView.SetCharacter(currentCharacterId);
            characterPanelView.SetCharacterProperties(currentCharacterId);
        }
        public void SpawnCharactersTab2(int characterRowCount)
        {
            characterPanelView.SpawnCharactersTab2(characterRowCount);
        }
        public void SetDefaultCharacter()
        {
            GameManager.Instance.userData.userCurrentCharacterId = 0;
            GameManager.Instance.UpdatePlayerPrefs();
            GameManager.Instance.SavePlayerPrefs();
        }
        public void CheckLockedCharacters()
        {
            characterPanelView.CheckLockedCharacters();
        }
        public void UpdateCharacterId(bool status)
        {
            if (status)
            {
                ++currentCharacterId;
            }
            else
            {
                --currentCharacterId;
            }
        }
        public void CloseCharacterPopup()
        {
            gameController.CloseCharacterPopup();
        }
        public int GetCharacterId()
        {
            return currentCharacterId;
        }
        #endregion
    }
}
