using System.IO;
using UnityEngine;
using UnityEngine.UI;
using CharacterThemeShop.Models;
using CharacterThemeShop.Managers;
using CharacterThemeShop.Controllers;

namespace CharacterThemeShop.Views
{
    public class CharacterPanelView : MonoBehaviour
    {
        #region ---------------------- Serialized Fields ----------------------
        [SerializeField] private CharacterPanelController characterPanelController;

        [Header("Character Panel Buttons")]
        [SerializeField] private Button rightBtn;
        [SerializeField] private Button leftBtn;
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button tab1Btn;
        [SerializeField] private Button tab2Btn;
        [SerializeField] private Button selectPlayerBtn;

        [Header("Character Components")]
        [SerializeField] private GameObject character;
        [SerializeField] public GameObject[] characterTempAr;
        [SerializeField] private GameObject characterTemp;
        [SerializeField] private Text characterName;
        [SerializeField] private RuntimeAnimatorController[] characterAnim;
        [SerializeField] private Sprite[] characterImg;

        [Header("Other Components")]
        [SerializeField] private GameObject characterPanel;
        [SerializeField] private GameObject tab1;
        [SerializeField] private GameObject tab2;
        [SerializeField] private string characterJsonPath;
        [SerializeField] private Image lockImg;
        [SerializeField] private Sprite selectPlayerBtnImg;
        #endregion


        #region ---------------------- Monobehaviour Methods ----------------------
        void Start()
        {
            leftBtn.onClick.AddListener(GetPreviousPlayer);
            rightBtn.onClick.AddListener(GetNextPlayer);
            closeBtn.onClick.AddListener(CloseCharacterPopup);
            tab1Btn.onClick.AddListener(OpenCharacterTab1);
            tab2Btn.onClick.AddListener(OpenCharacterTab2);
            selectPlayerBtn.onClick.AddListener(SelectPlayer);
        }
        #endregion
        #region ---------------------- Private On Button Click Methods ----------------------
        private void GetPreviousPlayer()
        {
            if (characterPanelController.GetCharacterId() > 0)
            {
                characterPanelController.UpdateCharacterId(false);
                SetCharacter(characterPanelController.GetCharacterId());
                SetCharacterProperties(characterPanelController.GetCharacterId());
            }
        }

        private void GetNextPlayer()
        {
            if (characterPanelController.GetCharacterId() < GameManager.Instance.characterStats.characterList.Count - 1)
            {
                characterPanelController.UpdateCharacterId(true);
                SetCharacter(characterPanelController.GetCharacterId());
                SetCharacterProperties(characterPanelController.GetCharacterId());
            }
        }
        private void CloseCharacterPopup()
        {
            tab2.SetActive(false);
            characterPanelController.CloseCharacterPopup();
        }
        private void OpenCharacterTab1()
        {
            tab2.SetActive(false);
            tab1Btn.transform.localScale = new Vector2(1.5f, 1.5f);
            tab2Btn.transform.localScale = new Vector2(1f, 1f);
            characterPanelController.SetCharacterPanel();
        }
        private void OpenCharacterTab2()
        {
            tab1.SetActive(false);
            tab2.SetActive(true);
            tab2Btn.transform.localScale = new Vector2(1.5f, 1.5f);
            tab1Btn.transform.localScale = new Vector2(1f, 1f);
        }
        private void SelectPlayer()
        {
            GameManager.Instance.userData.userCurrentCharacterId = characterPanelController.GetCharacterId();
            GameManager.Instance.UpdatePlayerPrefs();
            GameManager.Instance.SavePlayerPrefs();
            CharacterSelectionBtn("Selected", true, null);
        }
        #endregion
        #region ---------------------- Private Methods ----------------------
        private void CharacterSelectionBtn(string selectionStr, bool activeKey, Sprite btnImg)
        {
            selectPlayerBtn.transform.GetChild(0).GetComponent<Text>().text = selectionStr;
            selectPlayerBtn.transform.GetChild(1).gameObject.SetActive(activeKey);
            selectPlayerBtn.interactable = !activeKey;
            if (activeKey)
            {
                selectPlayerBtn.GetComponent<Image>().enabled = false;
            }
            else
            {
                selectPlayerBtn.GetComponent<Image>().enabled = true;
                selectPlayerBtn.GetComponent<Image>().sprite = btnImg;
            }
        }
        #endregion
        #region ---------------------- Public Methods ----------------------
        public void SetTab1Active()
        {
            tab1.SetActive(true);
            tab1Btn.transform.localScale = new Vector2(1.5f, 1.5f);
            tab2Btn.transform.localScale = new Vector2(1f, 1f);
        }
        public void StartPopulatingCharacters()
        {
            string characterDataString = File.ReadAllText(Application.dataPath + characterJsonPath);
            GameManager.Instance.characterStats = JsonUtility.FromJson<CharacterList>(characterDataString);
        }
        public int SetCurrentCharacterDetails()
        {
            if (GameManager.Instance.characterStats.characterList[GameManager.Instance.userData.userCurrentCharacterId].unlockLevel <= GameManager.Instance.userData.userCurrentLevel)
            {
                return GameManager.Instance.characterStats.characterList[GameManager.Instance.userData.userCurrentCharacterId].characterId;
            }
            else
            {
                GameManager.Instance.userData.userCurrentCharacterId = 0;
                GameManager.Instance.UpdatePlayerPrefs();
                GameManager.Instance.SavePlayerPrefs();
                return 0;
            }
        }
        public void SpawnCharactersTab2(int characterRowCount)
        {
            int j = 0;
            var rectTrans = characterPanel.GetComponent<RectTransform>();
            float xPos = -rectTrans.sizeDelta.x / 5f;
            float yPos = rectTrans.sizeDelta.y / 10;
            float spawningDistance = rectTrans.sizeDelta.x / 3f;
            for (int i = 0; i < GameManager.Instance.characterStats.characterList.Count; i++)
            {
                if (j == characterRowCount)
                {
                    yPos -= spawningDistance;
                    xPos = -rectTrans.sizeDelta.x / 5;
                }
                characterTempAr[i] = Instantiate(characterTemp);
                characterTempAr[i].transform.SetParent(tab2.transform, false);
                characterTempAr[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(xPos, yPos);
                characterTempAr[i].GetComponent<Image>().sprite = characterImg[i];
                characterTempAr[i].GetComponent<Animator>().runtimeAnimatorController = characterAnim[i];
                xPos += spawningDistance;
                j++;
            }
        }
        public void CheckLockedCharacters()
        {
            for (int i = 0; i < GameManager.Instance.characterStats.characterList.Count; i++)
            {
                if (GameManager.Instance.userData.userCurrentLevel < GameManager.Instance.characterStats.characterList[i].unlockLevel)
                {
                    characterTempAr[i].transform.GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    characterTempAr[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        public void SetCharacter(int characterId)
        {
            character.GetComponent<Image>().sprite = characterImg[characterId];
            character.GetComponent<Animator>().runtimeAnimatorController = characterAnim[characterId];
            if (GameManager.Instance.characterStats.characterList[characterId].unlockLevel
                > GameManager.Instance.userData.userCurrentLevel)
            {
                lockImg.gameObject.SetActive(true);

            }
            else
            {
                lockImg.gameObject.SetActive(false);
            }
        }
        public void SetCharacterProperties(int characterId)
        {
            characterName.text = GameManager.Instance.characterStats.characterList[characterId].characterName;
            if (GameManager.Instance.userData.userCurrentCharacterId == characterId)
            {
                CharacterSelectionBtn("Selected", true, null);
            }
            else if (GameManager.Instance.userData.userCurrentLevel < GameManager.Instance.characterStats.characterList[characterId].unlockLevel)
            {
                CharacterSelectionBtn("Selected", true, null);
                selectPlayerBtn.transform.GetChild(0).GetComponent<Text>().text = "LEVEL " + GameManager.Instance.characterStats.characterList[characterId].unlockLevel.ToString();
                selectPlayerBtn.transform.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                CharacterSelectionBtn("Select", false, selectPlayerBtnImg);
            }
        }
        #endregion
    }
}
