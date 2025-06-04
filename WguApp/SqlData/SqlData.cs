using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using static SQLite.SQLite3;

namespace WguApp.SqlData
{
    public class SqlDatahelper
    {
        private readonly SQLiteAsyncConnection _database;

        public SqlDatahelper()
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "WguApp.db");
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<WguClasses.Term>().Wait();
            _database.CreateTableAsync<WguClasses.Course>().Wait();
            _database.CreateTableAsync<WguClasses.Assessment>().Wait();
            _database.CreateTableAsync<WguClasses.CourseNote>().Wait();
            _database.CreateTableAsync<WguClasses.Login>().Wait();
            
        }

        public async Task<WguClasses.Login> AttemptLogin(string username, string password)
        {
            WguClasses.Login user = await _database.Table<WguClasses.Login>()
                .Where(u => u.Username == username && u.Password == password)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                // User not found, return null or handle accordingly
                return null;
            }
            if(user.Role == "Admin")
            {
                return new WguClasses.Admin
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Password = user.Password,
                    Role = user.Role
                };
                
                
            }
            else
            {
                // User found, return the user object
                return user;
            }
        }

        public async Task<int> CreateUser(WguClasses.Login user)
        {
            // Check if the user already exists
            var existingUser = await _database.Table<WguClasses.Login>()
                .Where(u => u.Username == user.Username)
                .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                // User already exists, return -1 or handle accordingly
                return -1;
            }
            // Insert the new user
            return await _database.InsertAsync(user);
        }

        public async Task<List<WguClasses.Term>> PullTerms(WguClasses.Login userid)
        {
            return await _database.Table<WguClasses.Term>()
                .Where(t => t.UserId == userid.UserId)
                .ToListAsync();
        }

        public async Task CreateTerm(WguClasses.Term term)
        {
            await _database.InsertAsync(term);
        }
        public async Task UpdateTerm(WguClasses.Term term)
        {
            await _database.UpdateAsync(term);
        }
        public async Task DeleteTerm(WguClasses.Term term)
        {
            await _database.DeleteAsync(term);
        }
        public async Task<bool> IsTermOverlapping(DateTime newStart, DateTime newEnd, int termId, int userid)
        {
            var existingTerms = await _database.Table<WguClasses.Term>().ToListAsync();

            foreach (var term in existingTerms)
            {
                // Skip checking against itself (for edits)
                if (term.TermID == termId) continue;

                // Check if the new term's range overlaps with an existing term
                bool isOverlapping = newStart < term.EndDate && newEnd > term.StartDate && term.UserId == userid;

                if (isOverlapping)
                {
                    return true; // Conflict found
                }
            }

            return false; // No conflicts
        }
        public async Task UpdateAssessment(WguClasses.Assessment assessment)
        {
            await _database.UpdateAsync(assessment);
        }
        public async Task<List<WguClasses.Course>> GetCoursesForTerm(int termId)
        {
            return await _database.Table<WguClasses.Course>().Where(c => c.TermID == termId).ToListAsync();
        }
        public async Task<int> AddCourse(WguClasses.Course course)
        {
            return await _database.InsertAsync(course);
        }
        public async Task<int> DeleteCourse(WguClasses.Course course)
        {
            return await _database.DeleteAsync(course);


        }

        public async Task<List<WguClasses.Term>> GetAllTerms()
        {
            return await _database.Table<WguClasses.Term>().ToListAsync();
        }

        public async Task UpdateCourse(WguClasses.Course course)
        {
            await _database.UpdateAsync(course);
        }
        public async Task<List<WguClasses.Assessment>> PullAssessments(int courseID)
        {
            return await _database.Table<WguClasses.Assessment>().Where(c => c.CourseId == courseID).ToListAsync();
        }
        public async Task<int> AddAssessment(WguClasses.Assessment assessment)
        {
            return await _database.InsertAsync(assessment);
        }

        public async Task<int> DeleteAssessment(WguClasses.Assessment assessment)
        {
            return await _database.DeleteAsync(assessment);
        }

        
    }
}

