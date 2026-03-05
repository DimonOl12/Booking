using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace Web.Services
{
    public class AppUser
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Citizenship { get; set; }
        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Address { get; set; }
        public string? BirthDay { get; set; }
        public string? BirthMonth { get; set; }
        public string? BirthYear { get; set; }
        public string? PassportFirstName { get; set; }
        public string? PassportLastName { get; set; }
        public string? PassportNumber { get; set; }
        public string? PassportExpiry { get; set; }
        public string? PassportNationality { get; set; }
    }

    public class PendingRegistration
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public class PasswordResetToken
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }

    public interface IUserStore
    {
        AppUser? FindByEmail(string email);
        bool Register(string email, string password);
        bool ValidatePassword(string email, string password);
        void Update(AppUser user);

        // Pending registration (email confirmation)
        string CreatePendingRegistration(string email, string password);
        PendingRegistration? GetPendingRegistration(string email);
        bool ConfirmRegistration(string email, string code);

        // Password reset
        string CreatePasswordResetToken(string email);
        bool ValidatePasswordResetToken(string email, string token);
        bool ResetPassword(string email, string token, string newPassword);
    }

    public class InMemoryUserStore : IUserStore
    {
        private readonly ConcurrentDictionary<string, AppUser> _users =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentDictionary<string, PendingRegistration> _pending =
            new(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentDictionary<string, PasswordResetToken> _resetTokens =
            new(StringComparer.OrdinalIgnoreCase);

        public AppUser? FindByEmail(string email) =>
            _users.TryGetValue(email, out var user) ? user : null;

        public bool Register(string email, string password)
        {
            var user = new AppUser
            {
                Email = email,
                PasswordHash = Hash(password),
            };
            return _users.TryAdd(email, user);
        }

        public bool ValidatePassword(string email, string password)
        {
            var user = FindByEmail(email);
            return user != null && user.PasswordHash == Hash(password);
        }

        public void Update(AppUser user) => _users[user.Email] = user;

        // ── Pending Registration ──────────────────────────────────────────────────

        public string CreatePendingRegistration(string email, string password)
        {
            var code = GenerateCode();
            var pending = new PendingRegistration
            {
                Email = email,
                PasswordHash = Hash(password),
                Code = code,
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
            };
            _pending[email] = pending;
            return code;
        }

        public PendingRegistration? GetPendingRegistration(string email) =>
            _pending.TryGetValue(email, out var p) ? p : null;

        public bool ConfirmRegistration(string email, string code)
        {
            if (!_pending.TryGetValue(email, out var pending)) return false;
            if (pending.ExpiresAt < DateTime.UtcNow) return false;
            if (!string.Equals(pending.Code, code, StringComparison.OrdinalIgnoreCase)) return false;

            var user = new AppUser
            {
                Email = email,
                PasswordHash = pending.PasswordHash,
            };
            if (!_users.TryAdd(email, user)) return false;
            _pending.TryRemove(email, out _);
            return true;
        }

        // ── Password Reset ────────────────────────────────────────────────────────

        public string CreatePasswordResetToken(string email)
        {
            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(16));
            _resetTokens[email] = new PasswordResetToken
            {
                Email = email,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
            };
            return token;
        }

        public bool ValidatePasswordResetToken(string email, string token)
        {
            if (!_resetTokens.TryGetValue(email, out var rt)) return false;
            return rt.ExpiresAt >= DateTime.UtcNow &&
                   string.Equals(rt.Token, token, StringComparison.OrdinalIgnoreCase);
        }

        public bool ResetPassword(string email, string token, string newPassword)
        {
            if (!ValidatePasswordResetToken(email, token)) return false;
            var user = FindByEmail(email);
            if (user == null) return false;
            user.PasswordHash = Hash(newPassword);
            _users[email] = user;
            _resetTokens.TryRemove(email, out _);
            return true;
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static string Hash(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        private static string GenerateCode()
        {
            var n = RandomNumberGenerator.GetInt32(100000, 999999);
            return n.ToString();
        }
    }
}
