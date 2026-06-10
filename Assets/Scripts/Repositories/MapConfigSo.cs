using System;
using System.Linq;
using Newtonsoft.Json;
using SaintsField;
using UnityEngine;

namespace Repositories
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Create/MapConfig", order = 0)]
    public class MapConfigSo : ScriptableObject
    {
        [SerializeField, ResizableTextArea]
        [FieldBelowInfoBox("$" + nameof(DynamicMessage), show: nameof(_showError))]
        private string mapJson = "[[]]";

        [Space] [SerializeField] private string[] words;
        
        private bool _showError;
        private char[,] _chars;
        
        public int Width => _chars.GetLength(0);
        public int Height => _chars.GetLength(1);
        public int CountWords => words.Length;

        public char this[int x, int y] => _chars[x, y];
        
        public bool HasWord(string word) => words.Contains(word);
        
        private (EMessageType, string) DynamicMessage()
        {
            _showError = true;
            try
            {
                _chars = JsonConvert.DeserializeObject<char[,]>(mapJson);
                if (_chars == null)
                {
                    var errorMessage = $"The map json is invalid";
                    return (EMessageType.Error, errorMessage);
                }
            }
            catch (Exception e)
            {
                return (EMessageType.Error, e.Message);
            }
            _showError = false;
            return (EMessageType.None, "");
        }

        private void Awake()
        {
            _chars = JsonConvert.DeserializeObject<char[,]>(mapJson);
        }
    }
}