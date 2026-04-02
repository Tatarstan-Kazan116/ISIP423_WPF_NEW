<img width="692" height="170" alt="image" src="https://github.com/user-attachments/assets/198227c2-fb12-4a72-9226-ad418531e621" />
<img width="357" height="394" alt="image" src="https://github.com/user-attachments/assets/0b7dd26f-b0c6-41f7-b986-8b4c922b60a0" />
<img width="1619" height="508" alt="image" src="https://github.com/user-attachments/assets/82090c33-ad7d-4b2a-ad23-af11023fff9b" />
Вывод: 
1. AuthTest (проверка пустых данных)
Успешен потому что:
Метод Auth() корректно проверяет входные данные на пустоту
Реализована валидация: if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
Возвращает false при пустых значениях
Assert правильно ожидает false
2. AuthTestFail (неверные данные для входа)
Успешен потому что:
Метод Auth() правильно обрабатывает неверные учетные данные
LINQ-запрос FirstOrDefault(c => c.Email == login && c.Password == password) возвращает null для несуществующих пользователей
Все 4 проверки (неверный логин, неверный пароль, пустой логин, пустой пароль) работают корректно
3. AuthTestSuccess (успешная авторизация)
Успешен потому что:
Исправлена критическая ошибка: email изменен с chetyrka_airways@gmail.com на chetyrka_airlines@gmail.com (соответствие данным в БД)
В базе данных существуют реальные пользователи:
chetyrka_airlines@gmail.com / chetyrka
bmw_liner@gmail.com / bmw
Настроено корректное подключение к БД через Entity Framework
Контекст Siraziev_Retunskih_CinemaEntities3 правильно инициализирован
4. RegistrationTestFail (некорректная регистрация)
Успешен потому что:
Все 5 правил валидации работают корректно:
Email без @ → отклонен 
Пароль ≤ 5 символов → отклонен 
Пустое ФИО → отклонено 
Возраст = 0 → отклонен
Телефон < 11 цифр → отклонен 
Метод Register() последовательно проверяет каждое поле
Assert'ы правильно ожидают false
5. RegistrationTestSuccess (успешная регистрация)
Успешен потому что:
Исправлена проблема с connectionString: удалено дублирование в CinemaUnitTestProject.dll.config
Синхронизированы конфигурации: App.config и CinemaUnitTestProject.dll.config содержат одинаковую строку подключения
Исправлен формат телефона: "77777777777" (11 цифр без +) соответствует правилу валидации
Пароль достаточной длины: "password123" (11 символов > 5)
Уникальный email: генерация через DateTime.Now.Ticks предотвращает конфликты
База данных доступна для записи
Entity Framework корректно сохраняет нового пользователя

