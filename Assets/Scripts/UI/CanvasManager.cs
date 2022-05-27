using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class CanvasManager : MonoBehaviour
    {
        #region Public Static

        public static CanvasManager CanvasManagerInstance { get; private set; }

        public static event EventHandler<bool> OnCanvasChange;

        public static Pokedex ActiveCanvas
        {
            get => CanvasManagerInstance._activeCanvas;
            set => CanvasManagerInstance._activeCanvas = value;
        }

        public static bool WritingWord
        {
            get => CanvasManagerInstance.writingWord;
            set => CanvasManagerInstance.writingWord = value;
        }

        #endregion

        #region Inspector

        [SerializeField]
        private bool writingWord;

        #endregion

        #region Private Fields

        private Pokedex _activeCanvas;

        #endregion


        #region Input Callbacks

        private void OpenClose(InputAction.CallbackContext context)
        {
            if (!WritingWord && context.started)
            {
                OnCanvasChange?.Invoke(this, ActiveCanvas.IsOpen);
                ActiveCanvas.OpenClose();
            }
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (CanvasManagerInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            CanvasManagerInstance = this;
        }

        #endregion
    }
}