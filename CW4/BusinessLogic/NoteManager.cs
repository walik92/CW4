using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CW4.DatabaseObjects;

namespace CW4.BusinessLogic
{
    public class NoteManager
    {
        public IEnumerable<Note> GetAllNotesOfUser(int userId)
        {
            using (var context = new DatabaseContext())
            {
                return context.Notes.Where(q => q.UserId == userId).ToList();
            }
        }

        public void Add(Note note)
        {
            if (string.IsNullOrEmpty(note.Value) || string.IsNullOrWhiteSpace(note.Value))
                throw new Exception("Podaj treść notatki");

            if (note.Value.Length > 5000)
                throw new Exception("Przekroczono dopuszczalną liczbę znaków (5000)");

            using (var context = new DatabaseContext())
            {
                try
                {
                    context.Users.Attach(note.User);
                    context.Notes.Add(note);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Wystąpił błąd podczas zapisu notatki", ex);
                }
            }
        }

        public void Edit(Note note)
        {
            if (string.IsNullOrEmpty(note.Value) || string.IsNullOrWhiteSpace(note.Value))
                throw new Exception("Podaj treść notatki");

            if (note.Value.Length > 5000)
                throw new Exception("Przekroczono dopuszczalną liczbę znaków notatki (5000)");

            using (var context = new DatabaseContext())
            {
                try
                {
                    context.Entry(note).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Wystąpił błąd podczas zapisu notatki", ex);
                }
            }
        }

        public void Delete(Note note)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    context.Entry(note).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception("Wystąpił błąd podczas usuwania notatki", ex);
                }
            }
        }
    }
}