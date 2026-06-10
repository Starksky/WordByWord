using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Behaviours
{
    public class WordItemView : MonoBehaviour, IPointerEnterHandler, IPointerMoveHandler
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text text;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;
        [SerializeField] private Color successColor;
        [SerializeField] private Color failureColor;
        
        public char Character => text.text.ToLower()[0];
        [CanBeNull] public WordItemView Left => _gridView?.GetByCoord(_coord + Vector2Int.left);
        [CanBeNull] public WordItemView Right => _gridView?.GetByCoord(_coord + Vector2Int.right);
        [CanBeNull] public WordItemView Up => _gridView?.GetByCoord(_coord + Vector2Int.up);
        [CanBeNull] public WordItemView Down => _gridView?.GetByCoord(_coord + Vector2Int.down);
        
        private Vector2Int _coord;
        private GridView _gridView;
        private bool _isSelected;
        private Color _defaultTextColor;
        
        public WordItemView Setup(GridView gridView, char character, Vector2Int coord) 
        {
            _gridView = gridView;
            _coord = coord;
            text.text = character.ToString().ToUpper();
            _defaultTextColor = text.color;
            return this;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_gridView.IsDownOnGrid)
                Select();
        }

        private void Select()
        {
            if (_isSelected)
                return;
            
            if (!_gridView.AddToPath(this))
                return;

            text.color = unselectedColor;
            background.color = selectedColor;
            _isSelected = true;
        }
        
        private void Unselect() 
        {
            text.color = _defaultTextColor;
            background.color = unselectedColor;
            _isSelected = false;
        }
        
        public async UniTask Success()
        {
            background.color = successColor;
            await UniTask.WaitForSeconds(0.5f, cancellationToken: destroyCancellationToken);
            Unselect();
        }
        
        public async UniTask Failure()
        {
            background.color = failureColor;
            await UniTask.WaitForSeconds(0.5f, cancellationToken: destroyCancellationToken);
            Unselect();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (_gridView.IsDownOnGrid)
                Select();
        }
    }
}