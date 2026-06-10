using System;
using Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace Behaviours
{
    public class ScoreView : MonoBehaviour
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
            _gameService.EventUpScore += GameServiceOnEventUpScore;
            GameServiceOnEventUpScore(0);
        }
        private void OnDestroy()
        {
            _gameService.EventUpScore -= GameServiceOnEventUpScore;
        }
        private void GameServiceOnEventUpScore(int score)
        {
            _text.text = string.Format(_mask, score);
        }
    }
}