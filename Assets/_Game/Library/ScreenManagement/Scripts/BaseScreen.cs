using UnityEngine;
using UnityEngine.UI;

namespace Library.ScreenManagement
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public abstract class BaseScreen : MonoBehaviour
    {
        public Canvas Canvas
        {
            get
            {
                if (_canvas != null) { return _canvas; }
                _canvas = GetComponent<Canvas>();
                return _canvas;
            }
        }
        private Canvas _canvas;

        public GraphicRaycaster GraphicRaycaster
        {
            get
            {
                if (_graphicRaycaster != null) { return _graphicRaycaster; }
                _graphicRaycaster = GetComponent<GraphicRaycaster>();
                return _graphicRaycaster;
            }
        }
        private GraphicRaycaster _graphicRaycaster;

        public bool IsInitialized { get; private set; }
        
        [Header("Screen References")]
        public Image Background;
        public CanvasGroup Content;

        public BaseScreenArgs Args;
        
        private ScreenManager _screenManager;
        
        internal void Initialize(ScreenManager screenManager)
        {
            IsInitialized = true;
            
            _screenManager = screenManager;
            Canvas.overrideSorting = true;
            Canvas.enabled = false;
            GraphicRaycaster.enabled = false;
        }
        
        internal void Setup(BaseScreenArgs args)
        {
            Args = args;
        }
        
        internal void Clear() { }
        
        internal void Close()
        {
            _screenManager.Close(this);
        }

        public virtual BaseScreenAnimationCommand GetAnimationCommand()
        {
            return new DefaultScreenAnimationCommand();
        }
        
        public virtual void OnInitialize() { }
        public virtual void OnSetup() { }
        public virtual void OnOpenBegin() { }
        public virtual void OnOpen() { }
        public virtual void OnCloseBegin() { }
        public virtual void OnClose() { }
        public virtual void OnClear() { }

        public virtual void OnShow() { }
        public virtual void OnHide() { }
    }
    
    public abstract class BaseScreen<TArgs> : BaseScreen 
        where TArgs : BaseScreenArgs
    {
        protected TArgs GetArgs()
        {
            return (TArgs)Args;
        }
    }
}