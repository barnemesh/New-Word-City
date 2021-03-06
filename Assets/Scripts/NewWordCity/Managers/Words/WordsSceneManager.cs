using System.Collections.Generic;
using Avrahamy;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player_Control;
using UnityEngine.InputSystem;

namespace Managers
{
    /// <summary>
    /// Manager to handle actions related to words in current scene.
    /// </summary>
    public class WordsSceneManager : MonoBehaviour
    {
        private static readonly Dictionary<string, List<MeaningfulWord>> SavedScenesData =
            new Dictionary<string, List<MeaningfulWord>>();

        #region Inspector

        [SerializeField]
        [Tooltip("List of words in this scene")]
        internal List<MeaningfulWord> words = new List<MeaningfulWord>();

        [SerializeField]
        private bool keepWordsAtReload;

        #endregion

        #region Internal Values

        /// <summary>
        /// Number of meanings found for current word.
        /// TODO wtf is internal?
        /// </summary>
        internal int MeaningFoundCount;

        // TODO: setter auto modulo
        /// <summary>
        /// Index of current word in the words array.
        /// </summary>
        internal int CurrentIndex;

        /// <summary>
        /// The current word.
        /// TODO: all the above 3 values are duplicated: with the index we can manage all of them in one.
        /// </summary>
        internal MeaningfulWord Current;

        #endregion

        #region Public Methods

        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (keepWordsAtReload)
            {
                // TODO: check on default getters?
                if (SavedScenesData.ContainsKey(SceneManager.GetActiveScene().name))
                {
                    //TODO: move to MeaningfulWord as update method!
                    var saved = SavedScenesData[SceneManager.GetActiveScene().name];
                    for (int i = 0; i < words.Count; i++)
                    {
                        for (int j = 0; j < words[i].Meanings.Count; j++)
                        {
                            words[i].Meanings[j].Found = saved[i].Meanings[j].Found;
                        }
                    }
                    // TODO: save more information! can be done by creating a state class with all the
                    //  serialized data! 
                    // TODO: update MeaningCountFound?
                }
            }
            else
            {
                foreach (var word in words)
                {
                    word.WordComplete = false;
                    foreach (var meaning in word.Meanings)
                    {
                        meaning.Found = false;
                    }
                }

                SavedScenesData.Clear();
                WordsGameManager.Completed.Clear();
            }
            
            WordsGameManager.Instance = this;

            DebugLog.Log(
                "Debug Keys: \nExit\t\t[Esc]\nReset\t\t[F4]\nReset Position\t[F7]\nSwitch Word\t[F8]\nShow Logs\t[F10]\n",
                this
            );
        }

        private void Start()
        {
            WordsGameManager.SwitchToNextAvailableWord();
        }

        private void OnDisable()
        {
            SavedScenesData[SceneManager.GetActiveScene().name] = words;
            WordsGameManager.Instance = null;
        }

        #endregion

        #region Input Callbacks

        public void OnReset(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DebugLog.Log(LogTag.HighPriority, "Reset Game");
                PureCoroutines.Instance.RemoveAllCoroutines();
                SceneManager.LoadScene(0);
            }
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                DebugLog.Log(LogTag.HighPriority, "Application Quit");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }

        public void OnSwitchToNextWord(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DebugLog.Log(LogTag.HighPriority, "Switching to next word");
                if (WordsGameManager.Current != null)
                {
                    WordsGameManager.Current.WordComplete = true;
                }

                WordsGameManager.SwitchToNextAvailableWord();
            }
        }

        public void OnResetPosition(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DebugLog.Log(LogTag.HighPriority, "Resetting Position");
                FindObjectOfType<Movement>().TeleportPlayer(Vector3.zero);
                CameraAndTeleportManager.Shared.ChangeCamera(1);
                CameraAndTeleportManager.Shared.ChangeFollowPlayer(2);
                StaticEventsGameManager.OnPlayerShouldInteract(this, true);
            }
        }

        #endregion
    }
}