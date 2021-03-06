using System.Collections;
using Interactable_Objects;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class MeaningCanvasHolder : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private float delay = 0.1f;

        #endregion

        #region Private Fields

        private TextMeshProUGUI _myText;
        private int _letterCount;
        private string _meaningString;
        private AudioSource _myAudio;
        // private bool _startAnimation = false;

        #endregion

        #region Public Properties

        public string MeaningString
        {
            get => _meaningString;
            set => _meaningString = value;
        }

        #endregion

        #region MonoBehaviour

        private void Start()
        {
            _myText = GetComponent<TextMeshProUGUI>();
            _myAudio = GetComponent<AudioSource>();
        }

        #endregion

        #region Callback

        public void FoundMeaning(MeaningDescriptor sender, InteractableObject e)
        {
            _meaningString = sender.meaning;
            StartCoroutine(WriteLetters(false));
        }

        #endregion

        #region Coroutines

        IEnumerator WriteLetters(bool reset)
        {
            while (true)
            {
                if (!CanvasManager.ActiveCanvas.IsOpen)
                {
                    yield return new WaitForSeconds(delay);
                }

                if (_letterCount < _meaningString.Length && CanvasManager.ActiveCanvas.IsOpen)
                {
                    if (_letterCount == 0)
                    {
                        _myAudio.Play();
                        CanvasManager.WordsToWrite++;
                    }

                    _myText.text += _meaningString[_letterCount++];
                    yield return new WaitForSeconds(delay);
                }

                if (_letterCount >= _meaningString.Length && !reset)
                {
                    _myAudio.Stop();
                    CanvasManager.WordsToWrite--;
                    break;
                }

                if (reset)
                {
                    yield return new WaitForSeconds(2f);
                    _myText.gameObject.SetActive(false);
                }
            }

            // TODO: Auto call last tutorial - need to make that property public
            if (WordsGameManager.Current.WordComplete && CanvasManager.WordsToWrite == 0)
            {
                Tutorial.Instance.TutorialContinue();
                // DebugLog.Log(LogTag.HighPriority, "Word Completed - Should switch in cool way!!!!", this);
                // WordsGameManager.SwitchToNextAvailableWord();
            }
        }

        #endregion
    }
}