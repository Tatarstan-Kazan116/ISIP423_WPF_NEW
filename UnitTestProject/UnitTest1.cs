using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pr14.Pages;
using Pr14;
using System;
using System.Linq;

namespace CinemaUnitTestProject
{
    [TestClass]
    public class AuthTests
    {
        private Page2 _authPage;

        [TestInitialize]
        public void SetUp()
        {
            _authPage = new Page2();
        }

        [TestMethod]
        public void AuthTest()
        {
            bool result = _authPage.Auth("", "");
            Assert.IsFalse(result, "Пустые данные не должны проходить авторизацию");
        }

        [TestMethod]
        public void AuthTestSuccess()
        {
            
            bool result = _authPage.Auth("chetyrka_airlines@gmail.com", "chetyrka");
            Assert.IsTrue(result, "Авторизация первого пользователя должна быть успешной");

            bool result1 = _authPage.Auth("bmw_liner@gmail.com", "bmw");
            Assert.IsTrue(result1, "Авторизация второго пользователя должна быть успешной");
        }

        [TestMethod]
        public void AuthTestFail()
        {
            bool result1 = _authPage.Auth("wrong@email.com", "wrongpass");
            Assert.IsFalse(result1, "Неверный логин не должен проходить");

            bool result2 = _authPage.Auth("chetyrka_airlines@gmail.com", "wrongpass");
            Assert.IsFalse(result2, "Неверный пароль не должен проходить");

            bool result3 = _authPage.Auth("", "chetyrka");
            Assert.IsFalse(result3, "Пустой логин не должен проходить");

            bool result4 = _authPage.Auth("chetyrka_airlines@gmail.com", "");
            Assert.IsFalse(result4, "Пустой пароль не должен проходить");
        }
    }

    [TestClass]
    public class RegistrationTests
    {
        private Page3 _regPage;

        [TestInitialize]
        public void SetUp()
        {
            _regPage = new Page3();
        }

        [TestMethod]
        public void RegistrationTestSuccess()
        {
            string testEmail = $"test{DateTime.Now.Ticks}@test.com";

            
            bool result = _regPage.Register(
                testEmail,
                "password123",      
                "Retunskih K.A",    
                "19",               
                "77777777777"       
            );

            Assert.IsTrue(result, $"Регистрация должна быть успешной. Email: {testEmail}");
        }

        [TestMethod]
        public void RegistrationTestFail()
        {
            
            bool result1 = _regPage.Register("invalid", "pass123", "Иван", "25", "77777777777");
            Assert.IsFalse(result1, "Email без @ не должен проходить");

            
            bool result2 = _regPage.Register("test@test.com", "12345", "Иван", "25", "77777777777");
            Assert.IsFalse(result2, "Пароль короче 6 символов не должен проходить");

           
            bool result3 = _regPage.Register("test@test.com", "password123", "", "25", "77777777777");
            Assert.IsFalse(result3, "Пустое ФИО не должно проходить");

            
            bool result4 = _regPage.Register("test@test.com", "password123", "Иван", "0", "77777777777");
            Assert.IsFalse(result4, "Нулевой возраст не должен проходить");

            
            bool result5 = _regPage.Register("test@test.com", "password123", "Иван", "25", "123");
            Assert.IsFalse(result5, "Короткий телефон не должен проходить");
        }
    }
}