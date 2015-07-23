using System;
using System.Collections.Generic;

namespace DForm
{
    [Serializable]
    public class CompositeQuestion : QuestionBase
    {
        private List<QuestionBase> _topics;

        public CompositeQuestion()
        {
            _topics = new List<QuestionBase>();
        }
    }
}
