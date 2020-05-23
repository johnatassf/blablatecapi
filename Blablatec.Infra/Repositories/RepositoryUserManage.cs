using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Model;
using Blablatec.Infra.Authorize;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Blablatec.Infra.Repositories
{
    public class RepositoryUserManage : BaseRepository<Usuario>, IRepositoryUserManage, IRepository<Usuario>
    {
        private readonly IMapper _mapper;

        public RepositoryUserManage(
            ContextBlablatec contextBlablatec, 
            IMapper mapper): base(contextBlablatec)
        {
            _mapper = mapper;
        }

        public BaseResult<Usuario> RegisterUser(RegistroUsuarioDto user)
        {
            if (GetAll(u => u.Nome == user.Ra).Any())
                return new BaseResult<Usuario> { Success = false, Message = "Ra cadastrado" };
            
            if (GetAll(u => u.Email == user.Email).Any())
                return new BaseResult<Usuario> { Success = false, Message = "E-mail cadastrado cadastrado" };

            var currentUser =  _mapper.Map<Usuario>(user);
            byte[] hash, salt;
           
            GererateHash(user.Senha, out hash, out salt);
            currentUser.Passwordhash = hash;
            currentUser.Passwordsalt = salt;

            var _user = Save(currentUser);

            return new BaseResult<Usuario> { Success = true, Data = _user, Message = "Usuario criado com sucesso" };
        }

        public bool Authorize(LoginUser loginUser)
        {
            var user = GetAll(u => u.Ra == loginUser.Ra).FirstOrDefault();
            if (user == null)
                return false;

            return ValidateHash(loginUser.Password, user.Passwordhash, user.Passwordsalt);
        }
        private Boolean ValidateHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < newPassHash.Length; i++)
                    if (newPassHash[i] != passwordhash[i])
                        return false;
            }
            return true;
        }

        private void GererateHash(String Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
                PasswordSalt = hash.Key;
            }
        }
    }
}
