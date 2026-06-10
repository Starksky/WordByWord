using System;
using System.Collections.Generic;
using Repositories;
using VContainer;

namespace Services
{
    public class GameService : IDisposable
    {
        private readonly MapConfigSo _mapConfig;
        private List<string> _findWords =  new ();
        private int _totalScore = 0;
        private int _totalWords;
        
        public int TotalWords => _totalWords;
        
        public event Action<int> EventUpScore;
        public event Action<int> EventUpWord;
        public event Action<string> EventAddWord;
        
        public MapConfigSo MapConfig => _mapConfig;
        
        [Inject]
        public GameService(MapConfigSo mapConfig)
        {
            _mapConfig = mapConfig;
            _totalWords = _mapConfig.CountWords;
        }

        public bool CheckWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return false;
            
            if (_findWords.Contains(word))
                return false;
            
            if (!_mapConfig.HasWord(word))
                return false;
            
            _findWords.Add(word);
            _totalScore += 100;
            
            EventUpScore?.Invoke(_totalScore);
            EventUpWord?.Invoke(_findWords.Count);
            EventAddWord?.Invoke(word);
            return true;
        }
        
        public void Dispose()
        {
            // TODO release managed resources here
        }
    }
}