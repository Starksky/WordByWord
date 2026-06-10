using Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace Behaviours
{
    public class WordsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private string _mask;
        private GameService _gameService;
        
        [Inject]
        public void Construct(GameService service)
        {
            _gameService = service;
        }

        private void Awake()
        {
            _mask = _text.text;
            _gameService.EventUpWord += GameServiceOnEventUpWord;
            GameServiceOnEventUpWord(0);
        }
        private void OnDestroy()
        {
            _gameService.EventUpScore -= GameServiceOnEventUpWord;
        }
        private void GameServiceOnEventUpWord(int countWords)
        {
            _text.text = string.Format(_mask, countWords, _gameService.TotalWords);
        }
    }
}