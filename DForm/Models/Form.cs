using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using DForm;

namespace DForm
{
    [Serializable]
    public class Form : ITitle
    {
        public Questions Questions;
        readonly List<FormAnswer> _formAnswer;
        private string title;

        public Form()
        {
            Questions = new Questions( this );
            _formAnswer = new List<FormAnswer>();
        }

        public Form(bool civilStatus)
        {
            _formAnswer = new List<FormAnswer>();
            Questions = new Questions(this, civilStatus);
        }

        public FormAnswer FindOrCreateAnswer( string answerTitle )
        {
            if( answerTitle == null ) throw new ArgumentNullException();
            foreach( FormAnswer formAnswer in _formAnswer )
            {
                if( formAnswer.UniqueName == answerTitle )
                {
                    return formAnswer;
                }
            }
            var newFormAnswer = new FormAnswer( answerTitle, this );
            _formAnswer.Add( newFormAnswer );
            return newFormAnswer;
        }

        public int AnswerCount
        {
            get { return this._formAnswer.Count; }
        }

        public string Title {
            get { return title; }
            set 
            { 
                title = value;

                if( Questions != null && Questions.Title == null )
                {
                    Questions.Title = value;
                }
            }
        }

        public List<FormAnswer> ListOfFormAnswer
        {
            get { return _formAnswer; }
        }

        public Form CloneSerializableObject( Form @this )
        {
            Form cloned = TypeExtensions.CloneSerializableObject( this );
            cloned.ListOfFormAnswer.Clear();
            return cloned;
        }
    }
}
