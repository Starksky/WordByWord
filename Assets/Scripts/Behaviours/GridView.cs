using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace Behaviours
{
    public class GridView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [FormerlySerializedAs("prefabItem")] [SerializeField] private WordItemView prefabItemView;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Gradient colorSelect;
        [SerializeField] private Gradient colorSuccess;
        [SerializeField] private Gradient colorFailure;
        
        private GameService _gameService;
        private IObjectResolver _resolver;
        private List<WordItemView> _pathWord = new List<WordItemView>();
        
        private WordItemView[,] _grid;
        public int Width => _grid.GetLength(0);
        public int Height => _grid.GetLength(1);
        public bool IsDownOnGrid { get; private set; }
        
        [Inject]
        public void Construct(GameService gameService, IObjectResolver resolver)
        {
            _gameService = gameService;
            _resolver = resolver;
        }

        private void ClearChild()
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
                Destroy(transform.GetChild(i).gameObject);
        }
        
        private void Awake()
        {
            ClearChild();
                
            lineRenderer.colorGradient = colorSelect;
            
            var width = _gameService.MapConfig.Width;
            var height = _gameService.MapConfig.Height;
            _grid = new WordItemView[width, height];
            
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    _grid[x, y] = _resolver.Instantiate(prefabItemView, parent: transform).Setup(this, _gameService.MapConfig[x, y], new Vector2Int(x, y));
        }

        private WordItemView GetByCoord(int x, int y)
        {
            if (x >= Width || y >= Height || x < 0 || y < 0)
                return null;
            return _grid[x, y];
        }
        
        public WordItemView GetByCoord(Vector2Int coord) => GetByCoord(coord.x, coord.y);
        
        public bool AddToPath(WordItemView itemView) 
        {
            
            
            if (_pathWord.Count == 0)
            {
                _pathWord.Add(itemView);
                return true;
            }

            bool result = false;
            var last = _pathWord.Last();
            if (last.Down == itemView || last.Up == itemView || last.Left == itemView || last.Right == itemView)
            {
                _pathWord.Add(itemView);
                result = true;
            }
            else
            {
                _pathWord.ForEach(w => w.Failure().Forget());
                _pathWord.Clear();
                IsDownOnGrid = false;
            }
            
            var positions = _pathWord.Select(w => w.transform.localPosition).ToArray();
            lineRenderer.positionCount = positions.Length;
            lineRenderer.SetPositions(positions);
            
            return result;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            IsDownOnGrid = eventData.button == PointerEventData.InputButton.Left;
        }
        public void OnPointerUp(PointerEventData eventData)
            => CheckWord();

        private void CheckWord()
        {
            if (_gameService.CheckWord(string.Concat(_pathWord.Select(w => w.Character))))
            {
                _pathWord.ForEach(async w =>
                {
                    lineRenderer.colorGradient = colorSuccess;
                    await w.Success();
                    lineRenderer.positionCount = 0;
                    lineRenderer.colorGradient = colorSelect;
                });
            }
            else
            {
                _pathWord.ForEach(async w =>
                {
                    lineRenderer.colorGradient = colorFailure;
                    await w.Failure();
                    lineRenderer.positionCount = 0;
                    lineRenderer.colorGradient = colorSelect;
                });
            }
            
            _pathWord.Clear();
            IsDownOnGrid = false;
        }
    }
}