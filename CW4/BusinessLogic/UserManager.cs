using System;
using System.Linq;
using CW4.DatabaseObjects;

namespace CW4.BusinessLogic
{
    public class UserManager
    {
        public User Get(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new Exception("Podaj login");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new Exception("Podaj hasło");
            }

            using (var context = new DatabaseContext())
            {
                User user;
                try
                {
                    user = context.Users.SingleOrDefault(q => q.Login == login);
                }
                catch (Exception ex)
                {
                    throw new Exception("Wystąpił błąd podczas łączenia z bazą danych", ex);
                }

                if (user == null)
                {
                    throw new Exception("Brak użytkownika o podanym loginie");
                }

                if (user.Password != password)
                {
                    throw new Exception("Nieprawidłowe hasło");
                }
                return user;
            }
        }
    }
}