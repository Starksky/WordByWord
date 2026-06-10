using System;
using Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace Behaviours
{
    public class LastWordView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        
        private GameService _gameService;
        
        [Inject]
        public void Construct(GameService gameService)
        {
            _gameService = gameService;
        }

        private void Awake()
        {
            _gameService.EventAddWord += GameServiceOnEventAddWord;
        }
        
        private void OnDestroy()
        {
            _gameService.EventAddWord -= GameServiceOnEventAddWord;
        }
        
        private void GameServiceOnEventAddWord(string word)
        {
            text.text = word;
        }
    }
}
