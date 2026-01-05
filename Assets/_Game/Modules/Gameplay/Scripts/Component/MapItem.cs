using DG.Tweening;
using ThirdParty.Other.EditorButton;
using UnityEngine;

namespace Modules.Gameplay
{
    public class MapItem : BaseItem
    {
        [SerializeField] private int _id;
        [SerializeField] private bool _isInteractable = true;
        [SerializeField] private bool _isCollectable = true;
        [SerializeField] private float _collectLocalScale = 1f;
        [Header("References")]
        [SerializeField] private SpriteRenderer _mainVisual;

        private Transform _initialParent;
        private Vector3 _initialLocalScale;
        private Vector3 _initialPosition;
        
        private void Start()
        {
            _initialParent = transform.parent;
            _initialLocalScale = transform.localScale;
            _initialPosition = transform.position;
        }

        public override int GetId()
        {
            return _id;
        }

        public override bool IsInteractable()
        {
            return _isInteractable;
        }

        public override bool IsCollectable()
        {
            return _isCollectable;
        }

        public override float GetCollectLocalScale()
        {
            return _collectLocalScale;
        }

        public override void OnBeforeCollect()
        {
            SetInteractable(false);
            bool isFlipped = _mainVisual.transform.localScale.x < 0f;
            if (isFlipped)
            {
                Flip();
            }
        }

        public override void OnReturnToMap()
        {
            transform.SetParent(_initialParent);

            transform.DOScale(_initialLocalScale, 0.3f);
            transform.DOMove(_initialPosition, 0.3f).OnComplete(() =>
            {
                SetInteractable(true);
            });
        }
        
        public Sprite GetIcon()
        {
            return _mainVisual.sprite;
        }

        public float GetCollectScale()
        {
            return _collectLocalScale;
        }

        public void SetInteractable(bool isInteractable)
        {
            Collider2D[] colliders = transform.GetComponentsInChildren<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = isInteractable;
            }
        }
        
        [EditorButton]
        public void Flip()
        {
            Vector3 localScale = _mainVisual.transform.localScale;
            localScale.x = -localScale.x;
            _mainVisual.transform.localScale = localScale;
        }
    }
}