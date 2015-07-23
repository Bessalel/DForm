using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DForm;
using System.Linq;
using System.Xml.Linq;

namespace DForm.Tests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void CreateAnswers()
        {
            Form f = new Form();
            Assert.IsNull( f.Title );
            f.Title = "jj";
            Assert.AreEqual( "jj", f.Title );

            FormAnswer a = f.FindOrCreateAnswer( "Emilie" );
            Assert.IsNotNull( a );
            FormAnswer b = f.FindOrCreateAnswer( "Emilie" );
            Assert.AreSame( a, b );

            Assert.AreEqual( 1, f.AnswerCount );
            FormAnswer c = f.FindOrCreateAnswer( "John Doe" );
            Assert.AreNotSame( a, c );

            Assert.AreEqual( "Emilie", a.UniqueName );
            Assert.AreEqual( "John Doe", c.UniqueName );
        }

        [Test]
        public void CreateQuestions()
        {
            Form f = new Form();
            f.Questions.Title = "HG67-Bis";
            Assert.AreEqual( "HG67-Bis", f.Title );

            QuestionBase q1 = f.Questions.AddNewQuestion( "DForm.CompositeQuestion,DForm" );
            QuestionBase q2 = f.Questions.AddNewQuestion( typeof( CompositeQuestion ) );
            q1.Title = "q1";
            q2.Title = "q2";
            Assert.AreEqual( 0, q1.Index );
            Assert.AreEqual( 1, q2.Index );
            q2.Index = 0;
            Assert.AreEqual( 0, q2.Index );
            Assert.AreEqual( 1, q1.Index );
            q2.Parent = null;
            Assert.AreEqual( 0, q1.Index );
            q2.Parent = q1;
            Assert.AreEqual( 0, q2.Index );
            Assert.IsTrue( f.Questions.Contains( q1 ) );
            Assert.IsTrue( f.Questions.Contains( q2 ) );
        }

        [Test]
        public void CreateQuestionsWithCivilStatus()
        {
            Form f = new Form(true);
            f.Questions.Title = "HG67-Bis";
            Assert.AreEqual("HG67-Bis", f.Title);

            Assert.IsNotEmpty(f.Questions.Dictionary);
        }

        [Test]
        public void LaTotale()
        {
            Form f = new Form();

            OpenQuestion qOpen = (OpenQuestion)f.Questions.AddNewQuestion( typeof( OpenQuestion ) );
            qOpen.Title = "First Question in the world!";
            qOpen.AllowEmptyAnswer = false;

            FormAnswer a = f.FindOrCreateAnswer( "Emilie" );
            Assert.IsNotEmpty( f.ListOfFormAnswer );

            BooleanQuestion qBool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            qBool.Title = "Second Question in the world!";
            qBool.AllowEmptyAnswer = false;

            AnswerBase theAnswerOfEmilieToQOpen = a.FindAnswer( qOpen );
            if( theAnswerOfEmilieToQOpen == null )
            {
                theAnswerOfEmilieToQOpen = a.AddAnswerFor( qOpen );
            }
            Assert.IsInstanceOf( typeof( OpenAnswer ), theAnswerOfEmilieToQOpen );

            OpenAnswer emilieAnswer = (OpenAnswer)theAnswerOfEmilieToQOpen;
            emilieAnswer.FreeAnswer = "I am very happy to be here";

            AnswerBase theAnswerOfEmilieToQBoolean = a.FindAnswer( qBool );
            if( theAnswerOfEmilieToQBoolean == null )
            {
                theAnswerOfEmilieToQBoolean = a.AddAnswerFor( qBool );
            }
            Assert.IsInstanceOf( typeof( BooleanAnswer ), theAnswerOfEmilieToQBoolean );

            BooleanAnswer emilieAnswerBool = (BooleanAnswer)theAnswerOfEmilieToQBoolean;
            emilieAnswerBool.BoolAnswer = true;

            Assert.IsTrue( emilieAnswerBool.BoolAnswer );

            qBool.Parent = qOpen;
            Assert.AreEqual( 0, qOpen.Index );
            Assert.AreEqual( 0, qBool.Index );
            Assert.IsTrue( f.Questions.Contains( qBool ) );
        }

        [Test]
        public void SerializableObject()
        {
            Form f = new Form();
            f.Title = "Prem's";

            OpenQuestion qOpen = (OpenQuestion)f.Questions.AddNewQuestion( typeof( OpenQuestion ) );
            qOpen.Title = "First Question in the world!";
            qOpen.AllowEmptyAnswer = false;

            FormAnswer a = f.FindOrCreateAnswer( "Emilie" );
            Assert.IsNotEmpty( f.ListOfFormAnswer );

            BooleanQuestion qBool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            qBool.Title = "Second Question in the world!";

            BooleanQuestion q2Bool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            q2Bool.Title = "Third Question in the world!";

            qBool.Parent = qOpen;
            q2Bool.Parent = qBool;
            Assert.AreEqual( 0, qOpen.Index );
            Assert.AreEqual( 0, q2Bool.Index );
            Assert.IsTrue( f.Questions.Contains( qBool ) );

            Form formBlank = f.CloneSerializableObject( f );
            formBlank.Title = "je suis un clone mais avec liste de FormAnswer vide !!!!!";
            Assert.IsEmpty( formBlank.ListOfFormAnswer );
            Assert.AreNotSame( f, formBlank );

            /* Création des réponses */
            AnswerBase theAnswerOfEmilieToQOpen = a.FindAnswer( qOpen );
            if( theAnswerOfEmilieToQOpen == null )
            {
                theAnswerOfEmilieToQOpen = a.AddAnswerFor( qOpen );
            }
            Assert.IsInstanceOf( typeof( OpenAnswer ), theAnswerOfEmilieToQOpen );

            OpenAnswer emilieAnswer = (OpenAnswer)theAnswerOfEmilieToQOpen;
            emilieAnswer.FreeAnswer = "I am very happy to be here";
            
            AnswerBase theAnswerOfEmilieToQBool = a.FindAnswer( qBool );
            if( theAnswerOfEmilieToQBool == null )
            {
                theAnswerOfEmilieToQBool = a.AddAnswerFor( qBool );
            }
            Assert.IsInstanceOf( typeof( BooleanAnswer ), theAnswerOfEmilieToQBool );
            BooleanAnswer emilieAnswerBool = (BooleanAnswer)theAnswerOfEmilieToQBool;
            emilieAnswerBool.BoolAnswer = true;
            //
            AnswerBase theAnswerOfEmilieToQBool2 = a.FindAnswer( q2Bool );
            if( theAnswerOfEmilieToQBool2 == null )
            {
                theAnswerOfEmilieToQBool2 = a.AddAnswerFor( q2Bool );
            }
            Assert.IsInstanceOf( typeof( BooleanAnswer ), theAnswerOfEmilieToQBool2 );

            BooleanAnswer emilieAnswerBool2 = (BooleanAnswer)theAnswerOfEmilieToQBool2;
            emilieAnswerBool2.BoolAnswer = false;

            Assert.IsEmpty( formBlank.ListOfFormAnswer );
            Assert.IsNotEmpty( f.ListOfFormAnswer );
        }

        [Test]
        public void Linq()
        {
            Form f = new Form();
            f.Title = "Prem's";

            OpenQuestion qOpen = (OpenQuestion)f.Questions.AddNewQuestion( typeof( OpenQuestion ) );
            qOpen.Title = "First Question in the world!";
            qOpen.AllowEmptyAnswer = false;

            FormAnswer a = f.FindOrCreateAnswer( "Emilie" );
            Assert.IsNotEmpty( f.ListOfFormAnswer );

            BooleanQuestion qBool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            qBool.Title = "Second Question in the world!";

            BooleanQuestion q2Bool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            q2Bool.Title = "Third Question in the world!";

            Assert.AreEqual( 0, qOpen.Index );
            Assert.AreEqual( 1, qBool.Index );
            Assert.AreEqual( 2, q2Bool.Index );
            Assert.IsTrue( f.Questions.Contains( qBool ) );

            /* Création des réponses */
            AnswerBase theAnswerOfEmilieToQOpen = a.FindAnswer( qOpen );
            if( theAnswerOfEmilieToQOpen == null )
            {
                theAnswerOfEmilieToQOpen = a.AddAnswerFor( qOpen );
            }
            Assert.IsInstanceOf( typeof( OpenAnswer ), theAnswerOfEmilieToQOpen );
            OpenAnswer emilieAnswer = (OpenAnswer)theAnswerOfEmilieToQOpen;
            emilieAnswer.FreeAnswer = "I am very happy to be here";
            //
            AnswerBase theAnswerOfEmilieToQBool = a.FindAnswer( qBool );
            if( theAnswerOfEmilieToQBool == null )
            {
                theAnswerOfEmilieToQBool = a.AddAnswerFor( qBool );
            }
            Assert.IsInstanceOf( typeof( BooleanAnswer ), theAnswerOfEmilieToQBool );
            BooleanAnswer emilieAnswerBool = (BooleanAnswer)theAnswerOfEmilieToQBool;
            emilieAnswerBool.BoolAnswer = true;
            //
            AnswerBase theAnswerOfEmilieToQBool2 = a.FindAnswer( q2Bool );
            if( theAnswerOfEmilieToQBool2 == null )
            {
                theAnswerOfEmilieToQBool2 = a.AddAnswerFor( q2Bool );
            }
            Assert.IsInstanceOf( typeof( BooleanAnswer ), theAnswerOfEmilieToQBool2 );

            BooleanAnswer emilieAnswerBool2 = (BooleanAnswer)theAnswerOfEmilieToQBool2;
            emilieAnswerBool2.BoolAnswer = false;

            var answerTitle = f.Questions.Dictionary
                                   .Select( i => i.Value )
                                   .OfType<BooleanAnswer>()
                                   .Where( d => d.BoolAnswer == false )
                                   .SelectMany( g => g.Title );

            Assert.AreEqual( answerTitle, "Third Question in the world!" );

            var answerIndex = f.Questions.Dictionary
                                .Select( i => i.Value )
                                .OfType<BooleanAnswer>()
                                .Where( i => i.Index == 2 )
                                .Select( h => h.Index ).FirstOrDefault();

            Assert.AreEqual( 2, answerIndex );

            Assert.IsNotEmpty( f.ListOfFormAnswer );
        }


        [Test]
        public void Xml()
        {
            Form f = new Form();
            f.Title = "Prem's";

            OpenQuestion qOpen = (OpenQuestion)f.Questions.AddNewQuestion( typeof( OpenQuestion ) );
            qOpen.Title = "First Question in the world!";
            qOpen.AllowEmptyAnswer = false;

            FormAnswer a = f.FindOrCreateAnswer( "Emilie" );
            Assert.IsNotEmpty( f.ListOfFormAnswer );

            BooleanQuestion qBool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            qBool.Title = "Second Question in the world!";

            BooleanQuestion q2Bool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            q2Bool.Title = "Third Question in the world!";


            XElement e = f.Questions.ToXml();

            var result = @"
                <Questions Title=""Prem's"">
                    <NumberOfQuestions Count=""3"">
                        <QuestionBase>First Question in the world!</QuestionBase> 
                        <QuestionBase>Second Question in the world!</QuestionBase> 
                        <QuestionBase>Third Question in the world!</QuestionBase> 
                    </NumberOfQuestions>
                </Questions>";
            var eTest = XElement.Parse( result );
            Assert.That( XElement.DeepEquals( eTest, e ) );
        }
    }
}
