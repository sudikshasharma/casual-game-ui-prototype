using UnityEngine;
using System.Collections;
using CharacterThemeShop.Views;
using CharacterThemeShop.Managers;

namespace CharacterThemeShop.Controllers
{
    public class GameController : MonoBehaviour
    {
        #region ---------------------- Private Serialized Fields ----------------------
        [SerializeField] private CharacterPanelController characterPanelController;
        [SerializeField] private GameView gameView;
        [SerializeField] private int characterRowCount;
        #endregion


        #region ---------------------- Monobehaviour Methods ----------------------
        void Start()
        {
            StartCoroutine(Init());
        }
        #endregion
        #region ---------------------- Private Methods ----------------------
        private IEnumerator Init()
        {
            while (!GameManager.Instance.isGameInitialized)
            {
                yield return new WaitForEndOfFrame();
            }
            characterPanelController.PopulateCharacters();
            gameView.Init();
        }
        #endregion
        #region ---------------------- Public Methods ----------------------
        public void UpdateUserDataCurrentLevel(int currentLevel)
        {
            GameManager.Instance.userData.userCurrentLevel = currentLevel;
        }
        public void SpawnCharactersTab2()
        {
            characterPanelController.SpawnCharactersTab2(characterRowCount);
        }
        public void ToggleCharacterPopUp(GameObject characterPanel)
        {
            characterPanelController.SetCharacterPanel();
            characterPanelController.CheckLockedCharacters();
            characterPanel.SetActive(true);
        }
        public void CloseCharacterPopup()
        {
            gameView.CloseCharacterPopup();
            StartCoroutine(gameView.RotateBg(false, 0f));
        }
        #endregion
    }
}
