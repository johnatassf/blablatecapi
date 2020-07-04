using AutoMapper;
using Blablatec.Domain.Dto;
using Blablatec.Domain.Interface;
using Blablatec.Domain.Model;
using Blablatec.Infra.Authorize;
    using Blablatec.Infra.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Blablatec.Infra.Repositories
{
    public class RepositoryUserManage : BaseRepository<Usuario>, IRepositoryUserManage, IRepository<Usuario>
    {
        private readonly IMapper _mapper;
        private readonly IServiceEmail _serviceEmail;
        private readonly IServiceInformationUser _informacaoUsuario;
        private readonly IRepository<Carro> _repositoryCarro;

        public RepositoryUserManage(
            ContextBlablatec contextBlablatec, 
            IMapper mapper,
            IServiceEmail serviceEmail,
            IServiceInformationUser informacaoUsuario,
            IRepository<Carro> repositoryCarro) : base(contextBlablatec)
        {
            _mapper = mapper;
            _serviceEmail = serviceEmail;
            _informacaoUsuario = informacaoUsuario;
            _repositoryCarro = repositoryCarro;
        }

        public BaseResult<Usuario> RegisterUser(RegistroUsuarioDto user)
        {
            if (GetAll(u => u.Ra.Equals(user.Ra)).Any())
                return new BaseResult<Usuario> { Success = false, Message = "O RA informado já está cadastrado." };
            
            if (GetAll(u => u.Email.Equals(user.Email)).Any())
                return new BaseResult<Usuario> { Success = false, Message = "E-mail cadastrado cadastrado" };


            var currentUser =  _mapper.Map<Usuario>(user);
            byte[] hash, salt;
           
            GererateHash(user.Senha, out hash, out salt);
            currentUser.Passwordhash = hash;
            currentUser.Passwordsalt = salt;

            var _user = Save(currentUser);

            if (user.Motorista)
                RegistrarCarroAoMotorista(user, _user.Id);

            return new BaseResult<Usuario> { Success = true, Data = _user, Message = "Usuario criado com sucesso" };
        }

        private void RegistrarCarroAoMotorista(RegistroUsuarioDto user, int idUsuarioRegistrado)
        {
            if (string.IsNullOrEmpty(user.Placa))
                throw new Exception("Por favor informe a placa do carro para registro do mesmo");
            var carro = new Carro()
            {
                Cor = user.CorCarro,
                Marca = user.Modelo,
                Placa = user.Placa,
                QuantidadeLugares = Convert.ToInt32(user.QtsLugares),
                IdMotorista = idUsuarioRegistrado
            };

            _repositoryCarro.Save(carro);
        }

        public IUser Authorize(LoginUser loginUser)
        {
            var user = GetAll(u => u.Ra == loginUser.Ra).FirstOrDefault();
            if (user == null)
                return null;
            
            if (ValidateHash(loginUser.Password, user.Passwordhash, user.Passwordsalt))
                return user;

            return null;
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

        public async Task UpdatePassword(Usuario user)
        {
            var passWord = Guid.NewGuid().ToString().Substring(0, 8);
            var assunto = "Renovação de acesso";
            var template = @$"<strong> Olá, aqui está sua nova senha do Blablatec: {passWord} </strong>";
            var textContent = @$"Blablatec: Renovação de acesso -- Nova senha {passWord} ";

            await _serviceEmail.Send(user.Email, user.Nome, assunto, template, textContent);

            byte[] hash, salt;

                GererateHash(passWord, out hash, out salt);
                user.Passwordhash = hash;
                user.Passwordsalt = salt;

                Update(user);
          
        }

        public async Task<Usuario> UpdateProfile(UpdateProfile user)
        {
            var id = Convert.ToInt32(_informacaoUsuario.IdUsuario);

            var _user = GetById(id);

            _user.Nome = user.Name;
            _user.Ra = user.Ra;
            _user.Sobrenome = user.LastName;
            _user.Email = user.Email;
            _user.NumeroTelefone = user.NumeroTelefone;


            _user = Update(_user);

            return await Task.FromResult(_user);

        }

    }
}
