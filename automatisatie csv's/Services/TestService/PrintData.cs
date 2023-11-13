using automatisatie_csv_s.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services.TestService
{
    public class PrintData
    {
        public static void PrintLists(List<Student> cursists,
                                  List<Course> cursussen,
                                  List<Teacher> leraren,
                                  List<TeachingAssignment> opdrachten,
                                  List<Enrollment> inschrijvingen)
        {
            // Afdrukken van Cursisten
            Console.WriteLine("Cursisten:");
            foreach (var cursist in cursists)
            {
                Console.WriteLine($"Naam: {cursist.Firstname} {cursist.Lastname}, Gebruikersnaam: {cursist.Username}, Email: {cursist.Email}");
            }

            // Afdrukken van Cursussen
            Console.WriteLine("\nCursussen:");
            foreach (var cursus in cursussen)
            {
                Console.WriteLine($"ID: {cursus.IdNumber}, Naam: {cursus.ShortName}, StartDatum: {cursus.StartDate}, EindDatum: {cursus.EndDate}");
            }

            // Afdrukken van Leraren
            Console.WriteLine("\nLeraren:");
            foreach (var leraar in leraren)
            {
                Console.WriteLine($"Naam: {leraar.Firstname} {leraar.Lastname}, Email: {leraar.Email}");
            }

            // Afdrukken van Cursusopdrachten
            Console.WriteLine("\nCursusopdrachten:");
            foreach (var opdracht in opdrachten)
            {
                Console.WriteLine($"Gebruikersnaam: {opdracht.Username}, Cursus: {opdracht.CourseShortName}, Type: {opdracht.Type1}");
            }

            // Afdrukken van Inschrijvingen
            Console.WriteLine("\nInschrijvingen:");
            foreach (var inschrijving in inschrijvingen)
            {
                Console.WriteLine($"Gebruikersnaam: {inschrijving.Username}, Cursus: {inschrijving.CourseShortName}, Type: {inschrijving.Type1}");
            }
        }
    }
}
