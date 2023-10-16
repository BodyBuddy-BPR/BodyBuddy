using BodyBuddy.ViewModels.ExerciseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ViewModels.ExerciseViewModels
{
    public class CategoryViewModelTests
    {
        private CategoryViewModel target;
        [SetUp]
        public void Setup()
        {
            target = new CategoryViewModel();
        }

        [Test]
        public void CorrectAmountOfCatagories()
        {
            Assert.That(target.Categories.Count, Is.EqualTo(7));
        }
    }
}
