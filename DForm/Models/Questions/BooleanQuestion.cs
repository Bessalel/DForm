using System;

namespace DForm
{
    [Serializable]
    public class BooleanQuestion : QuestionBase
    {
        private bool _allowEmptyAnswer;
        private bool _boolAnswer;

        public bool AllowEmptyAnswer
        {
            get { return _allowEmptyAnswer; }
            set { _allowEmptyAnswer = value; }
        }

        public bool BoolAnswer
        {
            get { return _boolAnswer; }
            set { _boolAnswer = value; }
        }
    }
}
