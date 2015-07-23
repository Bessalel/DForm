using System;

namespace DForm
{
    [Serializable]
    public class OpenQuestion : QuestionBase
    {
        private bool _allowEmptyAnswer;
        private string _freeAnswer;

        public bool AllowEmptyAnswer
        {
            get { return _allowEmptyAnswer; }
            set { _allowEmptyAnswer = value; }
        }
        
        public string FreeAnswer
        {
            get { return _freeAnswer; }
            set { _freeAnswer = value; }
        }
    }
}
