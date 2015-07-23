using System;
using System.Collections.Generic;

namespace DForm
{
    [Serializable]
    public class FormAnswer
    {
        private string _uniqueName;
        private Form Form;
        private Dictionary<QuestionBase, AnswerBase> dictionaryQuestionAnswer;

        public FormAnswer( string answerTitle, Form form )
        {
            _uniqueName = answerTitle;
            Form = form;
            dictionaryQuestionAnswer = new Dictionary<QuestionBase, AnswerBase>();
        }

        public Dictionary<QuestionBase, AnswerBase> Dictionary
        {
            get { return dictionaryQuestionAnswer; }
        }

        public AnswerBase FindAnswer( QuestionBase questionBase )
        {
            if( this.dictionaryQuestionAnswer.Count != this.Form.Questions.Dictionary.Count )
            {
                Dictionary<QuestionBase,AnswerBase> dictionary = this.Form.Questions.Dictionary;

                foreach( KeyValuePair<QuestionBase, AnswerBase> entry in dictionary )
                {
                    try
                    {
                        this.dictionaryQuestionAnswer.Add( entry.Key, null );
                    }
                    catch( ArgumentException )
                    {
                        Console.WriteLine( "An element with Key = " + entry.Key + " already exists." );
                    }
                }
                this.dictionaryQuestionAnswer = this.Form.Questions.Dictionary;
            }
            foreach( KeyValuePair<QuestionBase, AnswerBase> entry in dictionaryQuestionAnswer )
            {
                if( entry.Key == questionBase )
                {
                    return entry.Value;
                }
            }
            return null;
        }

        public AnswerBase AddAnswerFor( QuestionBase questionBase )
        {
            AnswerBase anwserBase = (AnswerBase)Activator.CreateInstance( Type.GetType( "DForm." + (questionBase.GetType().Name).Replace( "Question", "Answer" ) + ",DForm" ) );
            anwserBase.Title = questionBase.Title;
            anwserBase.Index = questionBase.Index;
            anwserBase.Parent = questionBase.Parent;
            this.dictionaryQuestionAnswer[questionBase] =  anwserBase ;
            return anwserBase;
        }

        public string UniqueName
        {
            get { return _uniqueName; }
            set { _uniqueName = value; }
        }

    }
}
