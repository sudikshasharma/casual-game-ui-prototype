using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CharacterThemeShop.Managers;
using CharacterThemeShop.Controllers;

namespace CharacterThemeShop.Views
{
    public class GameView : MonoBehaviour
    {
        #region ---------------------- Private Serialized Fields ----------------------
        [SerializeField] private GameController gameController;
        [SerializeField] private GameObject characterPanel;

        [Header("Main Menu UI")]
        [SerializeField] private Button characterPanelBtn;
        [SerializeField] private InputField currentLevel;
        [SerializeField] private Image rotatingBg;
        #endregion


        #region ---------------------- Monobehaviour Methods ---------------------- 
        void Start()
        {
            characterPanel.SetActive(false);
        }
        #endregion
        #region ---------------------- Private On Button Click Methods ----------------------
        private void ToggleCharacterPopUp()
        {
            if (!string.IsNullOrEmpty(currentLevel.text))
            {
                gameController.UpdateUserDataCurrentLevel(int.Parse(currentLevel.text));
                GameManager.Instance.UpdatePlayerPrefs();
                GameManager.Instance.SavePlayerPrefs();
                gameController.ToggleCharacterPopUp(characterPanel);
                StartCoroutine(RotateBg(true, 0.3f));
            }
        }
        #endregion
        #region ---------------------- Public Methods ----------------------
        public void Init()
        {
            characterPanelBtn.onClick.AddListener(ToggleCharacterPopUp);
            gameController.SpawnCharactersTab2();
        }
        public void CloseCharacterPopup()
        {
            characterPanel.SetActive(false);
        }
        public IEnumerator RotateBg(bool status, float time)
        {
            yield return new WaitForSeconds(time);
            rotatingBg.gameObject.SetActive(status);
        }
        #endregion
    }
}
