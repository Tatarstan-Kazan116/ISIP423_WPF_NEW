using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pr14.Pages;
using System;
using System.Linq;
using Pr14;

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

        // ✅ Базовый тест — пустые данные
        [TestMethod]
        public void AuthTest()
        {
            bool result = _authPage.AuthUser("", "");
            Assert.IsFalse(result, "Пустые данные не должны проходить авторизацию");
        }

        // ✅ Позитивный тест — успешная авторизация
        [TestMethod]
        public void AuthTestSuccess()
        {
            // ⚠️ Замени на реальные данные из твоей БД
            // Проверь в SQL: SELECT Email, Password FROM Client
            bool result1 = _authPage.AuthUser("user1@test.com", "password123");
            Assert.IsTrue(result1, "Авторизация с правильными данными должна быть успешной");

            bool result2 = _authPage.AuthUser("user2@test.com", "password456");
            Assert.IsTrue(result2, "Авторизация второго пользователя должна быть успешной");
        }

        // ✅ Негативный тест — неудачная авторизация
        [TestMethod]
        public void AuthTestFail()
        {
            // Неверный пароль
            bool result1 = _authPage.AuthUser("user1@test.com", "wrongpassword");
            Assert.IsFalse(result1, "Неверный пароль не должен проходить авторизацию");

            // Несуществующий пользователь
            bool result2 = _authPage.AuthUser("nonexistent@test.com", "password123");
            Assert.IsFalse(result2, "Несуществующий пользователь не должен проходить авторизацию");

            // Пустой логин
            bool result3 = _authPage.AuthUser("", "password123");
            Assert.IsFalse(result3, "Пустой логин не должен проходить авторизацию");

            // Пустой пароль
            bool result4 = _authPage.AuthUser("user1@test.com", "");
            Assert.IsFalse(result4, "Пустой пароль не должен проходить авторизацию");
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

        // ✅ Позитивный тест регистрации
        [TestMethod]
        public void RegistrationTestSuccess()
        {
            string testEmail = $"test{DateTime.Now.Ticks}@test.com";
            bool result = _regPage.RegistrationUser(
                testEmail,
                "password123",
                "Иванов Иван Иванович",
                25,
                "+79991234567"
            );
            Assert.IsTrue(result, "Регистрация с корректными данными должна быть успешной");
        }

        // ✅ Негативный тест регистрации
        [TestMethod]
        public void RegistrationTestFail()
        {
            // Неверный email (без @)
            bool result1 = _regPage.RegistrationUser(
                "invalidemail",
                "password123",
                "Иванов Иван",
                25,
                "+79991234567"
            );
            Assert.IsFalse(result1, "Неверный email не должен проходить регистрацию");

            // Короткий пароль
            bool result2 = _regPage.RegistrationUser(
                "test@test.com",
                "123",
                "Иванов Иван",
                25,
                "+79991234567"
            );
            Assert.IsFalse(result2, "Короткий пароль не должен проходить регистрацию");

            // Пустое ФИО
            bool result3 = _regPage.RegistrationUser(
                "test@test.com",
                "password123",
                "",
                25,
                "+79991234567"
            );
            Assert.IsFalse(result3, "Пустое ФИО не должно проходить регистрацию");

            // Неверный возраст (0 или отрицательный)
            bool result4 = _regPage.RegistrationUser(
                "test@test.com",
                "password123",
                "Иванов Иван",
                0,
                "+79991234567"
            );
            Assert.IsFalse(result4, "Нулевой возраст не должен проходить регистрацию");

            // Короткий телефон
            bool result5 = _regPage.RegistrationUser(
                "test@test.com",
                "password123",
                "Иванов Иван",
                25,
                "123"
            );
            Assert.IsFalse(result5, "Короткий телефон не должен проходить регистрацию");

            // Неверный формат телефона (+ но не 12 символов)
            bool result6 = _regPage.RegistrationUser(
                "test@test.com",
                "password123",
                "Иванов Иван",
                25,
                "+7999"
            );
            Assert.IsFalse(result6, "Неверный формат телефона не должен проходить регистрацию");
        }
    }
}
