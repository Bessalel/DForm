﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DForm;

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

            //QuestionBase q1 = f.Questions.AddNewQuestion( "DForm.CompositeQuestion,DForm" );
            //QuestionBase q2 = f.Questions.AddNewQuestion( typeof( CompositeQuestion ) );
            QuestionBase q1 = f.Questions.AddNewQuestion( "DForm.OpenQuestion,DForm" );
            QuestionBase q2 = f.Questions.AddNewQuestion( typeof( OpenQuestion ) );
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
        public void LaTotale()
        {
            Form f = new Form();

            OpenQuestion qOpen = (OpenQuestion)f.Questions.AddNewQuestion( typeof( OpenQuestion ) );
            qOpen.Title = "First Question in the world!";
            qOpen.AllowEmptyAnswer = false;

            FormAnswer a = f.FindOrCreateAnswer( "Emilie" );
            AnswerBase theAnswerOfEmilieToQOpen = a.FindAnswer( qOpen );
            if( theAnswerOfEmilieToQOpen == null )
            {
                theAnswerOfEmilieToQOpen = a.AddAnswerFor( qOpen );
            }
            Assert.IsInstanceOf( typeof( OpenAnswer ), theAnswerOfEmilieToQOpen );

            OpenAnswer emilieAnswer = (OpenAnswer)theAnswerOfEmilieToQOpen;
            emilieAnswer.FreeAnswer = "I am very happy to be here";


            BooleanQuestion qBool = (BooleanQuestion)f.Questions.AddNewQuestion( typeof( BooleanQuestion ) );
            qBool.Title = "Second Question in the world!";
            qBool.AllowEmptyAnswer = false;

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
    }
}
