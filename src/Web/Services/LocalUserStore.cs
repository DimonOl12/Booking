using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Web.Services
{
    /// <summary>
    /// In-memory user store used as fallback when the Reservio API is unreachable.
    /// </summary>
    public class LocalUserStore
    {
        private record LocalUser(string Email, string PasswordHash, string FirstName, string LastName);

        // Static so users persist for the lifetime of the process (across requests)
        private static readonly ConcurrentDictionary<string, LocalUser> _users =
            new(StringComparer.OrdinalIgnoreCase);

        public bool Register(string email, string password, string firstName, string lastName)
        {
            var hash = Hash(password);
            return _users.TryAdd(email, new LocalUser(email, hash, firstName, lastName));
        }

        public (string Email, string FirstName, string LastName)? Authenticate(string email, string password)
        {
            if (!_users.TryGetValue(email, out var user)) return null;
            if (Hash(password) != user.PasswordHash) return null;
            return (user.Email, user.FirstName, user.LastName);
        }

        public bool Exists(string email) => _users.ContainsKey(email);

        private static string Hash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToBase64String(bytes);
        }
    }
}
