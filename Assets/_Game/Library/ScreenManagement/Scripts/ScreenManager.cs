using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Library.ScreenManagement
{
    public class ScreenManager : MonoBehaviour
    {
        public event Action<Type> OnActiveScreenChanged;
     
        public int TopSortingOrder => Utility.GetCalculatedSortingOrder(_currentSortingOrderIndex);
        
        [SerializeField] private bool _initializeOnStart = true;
        [SerializeField] private bool _clearOnDestroy = true;
        [SerializeField] private List<BaseScreen> _screenReferences;

        private int _currentSortingOrderIndex;
        private readonly List<BaseScreen> _activeScreens = new();

        private void Start()
        {
            if (_initializeOnStart)
            {
                Initialize();
            }
        }

        private void OnDestroy()
        {
            if (_clearOnDestroy)
            {
                Clear();
            }
        }

        public void Initialize()
        {
            _currentSortingOrderIndex = 0;
            for (int i = 0; i < _screenReferences.Count; i++)
            {
                Register(_screenReferences[i]);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _screenReferences.Count; i++)
            {
                _screenReferences[i].OnClear();
            }
        }

        public void Register(BaseScreen screen)
        {
            if (!_screenReferences.Contains(screen))
            {
                _screenReferences.Add(screen);
            }

            int index = _screenReferences.IndexOf(screen);
            if (Utility.IsPrefab(_screenReferences[index].gameObject))
            {
                screen = Instantiate(_screenReferences[index], transform, false);
                screen.transform.localScale = Vector3.one;
                _screenReferences[index] = screen;
            }
            
            if (!screen.IsInitialized)
            {
                screen.Initialize(this);
                screen.OnInitialize();
            }
        }

        public void Open<T>(BaseScreenArgs args = default, ScreenOperationOption option = default) where T : BaseScreen
        {
            if (option == default) option = ScreenOperationOption.Empty();
            
            for (int i = 0; i < _activeScreens.Count; i++)
            {
                if (option.ShouldHideUnderneath)
                {
                    Hide(_activeScreens[i]);
                }
                else
                {
                    _activeScreens[i].GraphicRaycaster.enabled = false;
                }
            }
            
            bool shouldAnimateBg = option.ShouldAnimateBg && (_activeScreens.Count <= 0 || !option.ShouldHideUnderneath);
            
            BaseScreen screen = GetReference<T>();
            
            OnActiveScreenChanged?.Invoke(screen.GetType());

            _activeScreens.Add(screen);
            
            _currentSortingOrderIndex++;
            screen.Canvas.sortingOrder = Utility.GetCalculatedSortingOrder(_currentSortingOrderIndex);
        
            screen.Setup(args);
            screen.OnSetup();
            screen.OnOpenBegin();
            
            screen.Canvas.enabled = true;
            screen.GraphicRaycaster.enabled = true;

            BaseScreenAnimationCommand screenAnimCommand = screen.GetAnimationCommand();
            screenAnimCommand.PlayOpenAnimation(screen, shouldAnimateBg, () =>
            {
                screen.OnOpen();
                screen.Args?.OnOpened?.Invoke();
                
                option.PerformCompleteOperations();
            });
        }

        public void Close<T>(ScreenOperationOption option = default) where T : BaseScreen
        {
            if (option == default) option = ScreenOperationOption.Empty();

            BaseScreen screen = GetActive<T>();
            if (screen == null) return;
            
            Close(screen, option);
        }
        
        public void Close(BaseScreen screen, ScreenOperationOption option = default)
        {
            if (screen == null) return;
         
            if (option == default) option = ScreenOperationOption.Empty();
            
            _activeScreens.Remove(screen);
            
            bool shouldAnimateBg = option.ShouldAnimateBg && (_activeScreens.Count <= 0 || GetTop().Canvas.enabled);

            screen.OnCloseBegin();
            
            screen.GraphicRaycaster.enabled = false;
            BaseScreenAnimationCommand screenAnimCommand = screen.GetAnimationCommand();
            screenAnimCommand.PlayCloseAnimation(screen, shouldAnimateBg, () =>
            {
                _currentSortingOrderIndex--;
                screen.Canvas.enabled = false;

                screen.OnClose();
                screen.Args?.OnClosed?.Invoke();
                
                option.PerformCompleteOperations();

                BaseScreen topScreen = GetTop();
                if (topScreen != null)
                {
                    OnActiveScreenChanged?.Invoke(topScreen.GetType());

                    Show(topScreen);
                }
            });   
        }
        
        public void CloseAll()
        {
            _currentSortingOrderIndex = 0;
            for (int i = 0; i < _activeScreens.Count; i++)
            {
                BaseScreen screen = _activeScreens[i];
                screen.Canvas.enabled = false;
                screen.GraphicRaycaster.enabled = false;
                screen.OnClose();
                screen.Args?.OnClosed?.Invoke();
            }

            _activeScreens.Clear();
        }
        
        private void Hide(BaseScreen screen)
        {
            if (screen != null)
            {
                screen.GraphicRaycaster.enabled = false;
                screen.Canvas.enabled = false;
                screen.OnHide();
            }
        }

        private void Show(BaseScreen screen)
        {
            if (screen != null)
            {
                if (!screen.Canvas.enabled)
                {
                    screen.OnShow();
                    screen.Canvas.enabled = true;
                    BaseScreenAnimationCommand screenAnimCommand = screen.GetAnimationCommand();
                    screenAnimCommand.PlayOpenAnimation(screen, false,
                        () => { screen.GraphicRaycaster.enabled = true; });
                }
                else
                {
                    screen.GraphicRaycaster.enabled = true;
                }
            }
        }

        public void UpdateArgs<T>(BaseScreenArgs args = null) where T : BaseScreen
        {
            T screen = GetActive<T>();
            if (screen != null)
            {
                screen.Setup(args);
                screen.OnSetup();
            }
        }

        private BaseScreen GetTop()
        {
            if (_activeScreens.Count > 0)
            {
                return _activeScreens.Last();
            }

            return null;
        }

        public T GetActive<T>() where T : BaseScreen
        {
            for (int i = 0; i < _activeScreens.Count; i++)
            {
                if (_activeScreens[i] != null && _activeScreens[i].GetType() == typeof(T))
                {
                    return (T)_activeScreens[i];
                }
            }

            return null;
        }

        private T GetReference<T>() where T : BaseScreen
        {
            for (int i = 0; i < _screenReferences.Count; i++)
            {
                if (_screenReferences[i] != null && _screenReferences[i].GetType() == typeof(T))
                {
                    return (T)_screenReferences[i];
                }
            }

            return null;
        }
    }
}