using UnityEngine;
using CharacterThemeShop.Models;

namespace CharacterThemeShop.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region ---------------------- Private Fields ----------------------
        private string savedData = "savedUserData";
        #endregion
        #region ---------------------- Public Fields ----------------------
        public static GameManager Instance;
        [HideInInspector] public UserDataModel userData;
        [HideInInspector] public CharacterList characterStats;
        [HideInInspector] public bool isGameInitialized;
        [HideInInspector] public int totalCharacters;
        #endregion


        #region ---------------------- Monobehaviour Methods ----------------------
        void Awake()
        {
            isGameInitialized = false;
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }
        void Start()
        {
            userData = new UserDataModel();
            RestoreUserData();
            isGameInitialized = true;
        }
        #endregion
        #region ---------------------- Private Methods ----------------------
        private void RestoreUserData()
        {
            string userDataString;
            if (!PlayerPrefs.HasKey(savedData))
            {
                userData.SetDefaultUserData();
                UpdatePlayerPrefs();
            }
            else
            {
                userDataString = PlayerPrefs.GetString(savedData);
                userData = JsonUtility.FromJson<UserDataModel>(userDataString);
            }
        }
        #endregion
        #region ---------------------- Public Methods ----------------------
        public void UpdatePlayerPrefs()
        {
            string userDataString = JsonUtility.ToJson(userData);
            PlayerPrefs.SetString(savedData, userDataString);
        }
        public void SavePlayerPrefs()
        {
            PlayerPrefs.Save();
        }
        #endregion
    }
}
