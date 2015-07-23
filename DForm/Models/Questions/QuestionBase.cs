using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DForm
{
    [Serializable]
    public abstract class QuestionBase : ITitle
    {
        private Form _form;
        private string _title;
        private int _index;
        private QuestionBase _parent;
        readonly Dictionary<QuestionBase, AnswerBase> dictionary;

        public QuestionBase( )
        {
            dictionary = new Dictionary<QuestionBase, AnswerBase>();
        }

        public QuestionBase( Form form )
        {
            _form = form;
            dictionary = new Dictionary<QuestionBase, AnswerBase>();
        }

        public int Index
        {
            get { return _index; }
            set
            {
                if( this._parent != null && this._parent.Dictionary != null && this._parent.Dictionary.Count > 0 )
                {
                    Dictionary<QuestionBase,AnswerBase> dictionaryParent = _parent.Dictionary;
                    
                    if ( value == -1 )
                    {
                        foreach( var qBase in dictionaryParent.Keys )
                        {
                            if( qBase._index > this._index )
                            {
                                qBase._index -= 1;
                            }
                            else if( qBase._index == this._index )
                            {
                                qBase._index = value;
                            }
                        }
                    }
                    else if( _index > value )
                    {
                        foreach( var qBase in dictionaryParent.Keys )
                        {
                            if( qBase._index >= value && qBase._index < this._index )
                            {
                                qBase._index += 1;
                            }
                            else if( qBase._index == this._index )
                            {
                                qBase._index = value;
                            }
                        }
                    }
                    else
                    {
                        foreach( var qBase in dictionaryParent.Keys )
                        {
                            if( qBase._index >= this._index && qBase._index < value )
                            {
                                qBase._index -= 1;
                            }
                            else if( qBase._index == this._index )
                            {
                                qBase._index = value;
                            }
                        }
                    }
                }
                else
                {
                    if( value == -2 )
                    {
                        if( this._parent.Dictionary.Count == 0 )
                        {
                            _index = 0;
                        }
                        else
                        {
                            _index = this._parent.Dictionary.Count;
                        }
                    }
                    else
                    {
                        _index = value;
                    }
                }
            }
        }

        public QuestionBase Parent
        {
            get { return _parent; }
            set
            {
                if( value == null )
                {
                    Index = -1;
                    _parent = null;
                }
                else
                {
                    _parent = value;
                    
                    if( value.Form == null )
                    {
                        Index = -2;
                    }
                    if( value.Parent != null )
                    {
                        CutLinkQuestions();
                        AddQuestionToParent( value );
                    } 
                }
            }
        }

        public virtual Form Form
        {
            get { return _form != null ? _form : null; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value;  }
        }

        public Dictionary<QuestionBase, AnswerBase> Dictionary
        {
            get { return dictionary; }
        }

        public void AddAnswer( AnswerBase answerBase )
        {
            this.Parent.Dictionary[this] = answerBase;
        }

        public void AddQuestionToParent( QuestionBase questionBase )
        {
            questionBase.Dictionary[this] = null;
        }

        private void CutLinkQuestions()
        {
            QuestionBase qb = this;
            do
            {
                qb = qb.Parent;
            }
            while( qb.Parent.Parent != null );
            qb.Parent.Dictionary.Remove( this );
        }
    }
}
