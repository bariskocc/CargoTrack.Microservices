using System;

namespace CargoTrack.Services.Identity.API.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {
        }
    }

    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid id) : base($"Kullanıcı bulunamadı. ID: {id}")
        {
        }

        public UserNotFoundException(string email) : base($"Kullanıcı bulunamadı. Email: {email}")
        {
        }
    }

    public class DuplicateEmailException : DomainException
    {
        public DuplicateEmailException(string email) : base($"Bu email adresi zaten kullanımda: {email}")
        {
        }
    }

    public class DuplicateUsernameException : DomainException
    {
        public DuplicateUsernameException(string username) : base($"Bu kullanıcı adı zaten kullanımda: {username}")
        {
        }
    }

    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException() : base("Geçersiz şifre.")
        {
        }
    }

    public class UserLockedOutException : DomainException
    {
        public UserLockedOutException(DateTime lockoutEnd) : base($"Hesabınız {lockoutEnd.ToLocalTime()} tarihine kadar kilitli.")
        {
        }
    }

    public class RoleNotFoundException : DomainException
    {
        public RoleNotFoundException(Guid id) : base($"Rol bulunamadı. ID: {id}")
        {
        }

        public RoleNotFoundException(string name) : base($"Rol bulunamadı. Ad: {name}")
        {
        }
    }

    public class DuplicateRoleNameException : DomainException
    {
        public DuplicateRoleNameException(string name) : base($"Bu rol adı zaten kullanımda: {name}")
        {
        }
    }

    public class PermissionNotFoundException : DomainException
    {
        public PermissionNotFoundException(Guid id) : base($"İzin bulunamadı. ID: {id}")
        {
        }

        public PermissionNotFoundException(string systemName) : base($"İzin bulunamadı. Sistem Adı: {systemName}")
        {
        }
    }

    public class DuplicatePermissionSystemNameException : DomainException
    {
        public DuplicatePermissionSystemNameException(string systemName) : base($"Bu sistem adı zaten kullanımda: {systemName}")
        {
        }
    }
} 