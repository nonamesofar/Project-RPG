using UnityEngine;
using RPG.Saving;
using System.Collections;
using RPG.Core;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        [SerializeField] float fadeInTime = 0.5f;
        const string defaultSaveFile = "save";

        PlayerControls keys;
        private bool saveGame = false;
        private bool loadGame = false;
        private bool deleteSave = false;

        private void Awake()
        {
            Debug.Log("Hello!");
            //StartCoroutine(LoadLastScene());
        }

        private void Start()
        {
            //new input manager
            keys = new PlayerControls();
            //keys.UI.Save.started += i => saveGame = true;
            //keys.UI.Load.started += i => loadGame = true;
            //keys.UI.DeleteSave.started += i => deleteSave = true;
            keys.Enable();
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);

        }
        // Update is called once per frame
        void Update()
        {

            saveGame = (ButtonStatus.ButtonDown == GetButtonStatus(keys.UI.Save));
            loadGame = (ButtonStatus.ButtonDown == GetButtonStatus(keys.UI.Load));
            deleteSave = (ButtonStatus.ButtonDown == GetButtonStatus(keys.UI.DeleteSave));

            if (loadGame)
            {
                Load();
            }
            if (saveGame)
            {
                Save();
            }
            if (deleteSave)
            {
                Delete();
            }

        }

        ButtonStatus GetButtonStatus(InputAction button)
        {
            if (InputActionButtonExtensions.GetButtonDown(button))
                return ButtonStatus.ButtonDown;
            if (InputActionButtonExtensions.GetButtonUp(button))
                return ButtonStatus.ButtonUp;
            if (InputActionButtonExtensions.GetButton(button))
                return ButtonStatus.ButtonHold;
            return ButtonStatus.NONE;
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

        public void OnClick()
        {
            //if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        public void OnEndEdit(string name)
        {
            Debug.Log("Name is: " + name);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (1 < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            Fader fader = FindObjectOfType<Fader>();
            DontDestroyOnLoad(gameObject);

            //yield return fader.FadeOut(100);

            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();
            wrapper.Save();

            yield return SceneManager.LoadSceneAsync(1);

            wrapper.Load();

            //Portal otherPortal = GetOtherPortal();
            //UpdatePlayer(otherPortal);

            wrapper.Save();

            //yield return new WaitForSeconds(100);
            //yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

    }
}
