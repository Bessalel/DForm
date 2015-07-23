using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
//using System.Data.Linq.Mapping;

namespace DForm
{
    [Serializable]
    public class Questions : QuestionBase
    {
        private string title;
        private int _currentIndex;

        public Questions( Form form ) 
            : base( form )
        {
            Title = form.Title;
        }

        public Questions(Form form, bool civilStatus)
            : base(form)
        {
            Title = form.Title;

            OpenQuestion qOpen = (OpenQuestion)this.AddNewQuestion(typeof(OpenQuestion));
            qOpen.Title = "Entrez votre nom : ";
            qOpen.AllowEmptyAnswer = false;

            OpenQuestion qOpen1 = (OpenQuestion)this.AddNewQuestion(typeof(OpenQuestion));
            qOpen1.Title = "Entrez votre prenom :";
            qOpen1.AllowEmptyAnswer = false;

            OpenQuestion qOpen2 = (OpenQuestion)this.AddNewQuestion(typeof(OpenQuestion));
            qOpen2.Title = "Quelle est votre date de naissance ? ";
            qOpen2.AllowEmptyAnswer = false;

            BooleanQuestion qBool3 = (BooleanQuestion)this.AddNewQuestion(typeof(BooleanQuestion));
            qBool3.Title = "Quel est votre sexe ? ";
            qBool3.AllowEmptyAnswer = false;
        }

        public Boolean Contains(QuestionBase questionBase)
        {
            if( questionBase == null ) throw new ArgumentNullException();
            if( questionBase == this || questionBase.Parent.Parent == null )
            {
                return true;
            } else {
                do
                {
                    questionBase = questionBase.Parent;
                    if( questionBase.Parent == this ) return true;
                }
                while( questionBase.Parent != this && questionBase != null );
                return false;
            }
        }

        public QuestionBase AddNewQuestion( string type ) 
        {
            return AddNewQuestion( Type.GetType( type ) );
        }

        public QuestionBase AddNewQuestion( Type t )
        {
            if( !typeof( QuestionBase ).IsAssignableFrom( t ) ) throw new ArgumentException( "The type Must be a QuestionBase" );
            QuestionBase qb = (QuestionBase)Activator.CreateInstance( t );
            qb.Index = _currentIndex++;
            this.Dictionary.Add( qb, null );

            if( this.Form.ListOfFormAnswer.Count != 0 )
            {
                List<FormAnswer> list = this.Form.ListOfFormAnswer;

                foreach( var tmp in list )
                {
                    tmp.Dictionary.Add( qb, null );
                }
            }
            qb.Parent = this;
            return qb;
        }


        public override Form Form {
            get { return base.Form; }
        }
        
        public new string Title
        {
            get { return title; }
            set 
            {
                title = value;

                if( value != null && Form.Title == null ) {
                    Form.Title = value;
                }
            }
        }

        public XElement ToXml()
        {
            return new XElement( "Questions", new XAttribute( "Title", title ),
                       new XElement( "NumberOfQuestions", new XAttribute( "Count", Dictionary.Count ),
                            Dictionary
                               .OrderBy( i => i.Key.Index )
                               .Select( i => new XElement( "QuestionBase", i.Key.Title ) ) ) );
            //return new XElement( "Questions", new XAttribute( "Title", title ),
            //           new XElement( "NumberOfQuestions", new XAttribute( "Count", Dictionary.Count ),
            //                Dictionary
            //                   .OrderBy( i => i.Key.Index )
            //                   .Select( i => new XElement( "QuestionBase", i.Key.Title ) ) ),
            //                     new XElement( "QuestionBase", new XAttribute( "Count", Dictionary.Count != 0 ),
            //                         Dictionary
            //                            .OrderBy( i => i.Key.Index )
            //                            .Select( t => new XElement( "QuestionBase", t.Key.Title ) ) ) );
        }
    }
}
