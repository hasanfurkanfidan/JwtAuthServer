using JwtAuthServer.Core.Configuration;
using JwtAuthServer.Core.Dtos;
using JwtAuthServer.Core.Models;
using JwtAuthServer.Core.Repositories;
using JwtAuthServer.Core.Services;
using JwtAuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;
        public AuthenticationService(ITokenService tokenService, UserManager<UserApp> userManager
            , IGenericRepository<UserRefreshToken> userRefreshTokenService, IUnitOfWork unitOfWork, IOptions<List<Client>> options)
        {
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _userRefreshTokenService = userRefreshTokenService;
            _clients = options.Value;
        }
        public async Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                Response<TokenDto>.Fail(new ErrorDto("Email veya parola yanlış", true), 400);
            }
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                Response<TokenDto>.Fail(new ErrorDto("Email veya parola yanlış", true), 400);
            }
            var token = _tokenService.CreateToken(user);
            var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();
            if (userRefreshToken == null)
            {
                await _userRefreshTokenService.AddAsync(new UserRefreshToken
                {
                    Code = token.RefreshToken,
                    Expiration = token.RefreshTokenExpiration,
                    UserId = user.Id
                });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;

            }
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(p => p.ClientId == clientLoginDto.ClientId && p.ClientSecret == clientLoginDto.ClientSecret);
            if (client == null)
            {
                return Response<ClientTokenDto>.Fail(new ErrorDto("Client bulunamadı", false), 400);
            }
            var token = _tokenService.CreateClientTokenDto(client);
            return Response<ClientTokenDto>.Success(token, 200);
        }

        public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var wantedRefreshToken = await _userRefreshTokenService.Where(p => p.Code == refreshToken).FirstOrDefaultAsync();
            if (wantedRefreshToken == null)
            {
                return Response<TokenDto>.Fail(new ErrorDto("Hata", false), 400);
            }
            var user = await _userManager.FindByIdAsync(wantedRefreshToken.UserId);
            if (user == null)
            {
                return Response<TokenDto>.Fail(new ErrorDto("Kullanıcı bulunamadı", false), 400);
            }
            var token = _tokenService.CreateToken(user);

            wantedRefreshToken.Code = token.RefreshToken;
            wantedRefreshToken.Expiration = token.RefreshTokenExpiration;
            await _unitOfWork.CommitAsync();
            return Response<TokenDto>.Success(token, 200);
        }

        public async Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var wantedRefreshToken = await _userRefreshTokenService.Where(p => p.Code == refreshToken).SingleOrDefaultAsync();
            if (wantedRefreshToken == null)
            {
                return Response<NoDataDto>.Fail(new ErrorDto("Hata", false), 400);
            }
            _userRefreshTokenService.Remove(wantedRefreshToken);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(200);
        }
    }
}
