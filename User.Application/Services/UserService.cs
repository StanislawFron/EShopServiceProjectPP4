using AutoMapper;
using User.Domain.Models.Response;
using User.Application.Producer;
using User.Domain.Models.Entities;
using BCrypt.Net;
using User.Domain.Models.Requests;
using User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace User.Application.Services

{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _db;
        private readonly IJwtTokenService _jwtTokenService;


        public UserService(IMapper mapper, DataContext db, IJwtTokenService jwtTokenService)
        {
            _mapper = mapper;
            _db = db;
            _jwtTokenService = jwtTokenService;
        }

        public UserResponseDTO GetUser(int Id)
        {
            //var user = _db.Users.Find(userId);
            var user = new User.Domain.Models.Entities.User() { 
                Username = "aaa", 
                PasswordHash = "asas",
                IsActive=true, Id = Id, 
                Email= "User@email.commmm" 
            };
            if (user == null)
                throw new Exception("Record not found");

            return _mapper.Map<UserResponseDTO>(user);
        }


        public async Task<string> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                throw new Exception("User already exists.");

            var user = new User.Domain.Models.Entities.User
            {
                Email = request.Email,
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true
            };
            //test
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return _jwtTokenService.GenerateToken(user.Id, new List<string> ());
        }
    }
}
