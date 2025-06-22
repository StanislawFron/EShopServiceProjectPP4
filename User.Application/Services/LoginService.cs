using User.Application.Producer;
using User.Domain.Exceptions.Login;
using Microsoft.EntityFrameworkCore;
using User.Domain.Models.Entities;
using User.Domain.Models.Requests;
using User.Domain.Repositories;


namespace User.Application.Services
{
    public class LoginService : ILoginService
    {
        protected IJwtTokenService _jwtTokenService;
        protected Queue<int> _userLoggedIdsQueue;
        protected IKafkaProducer _kafkaProducer;
        private readonly DataContext _db;

        public LoginService(IJwtTokenService jwtTokenService, IKafkaProducer kafkaProducer, DataContext db)
        {
            _jwtTokenService = jwtTokenService;
            _userLoggedIdsQueue = new Queue<int>();
            _kafkaProducer = kafkaProducer;
            _db = db;
        }

        public string Login(string username, string password)
        {
            if (username == "admin" && password == "password")
            {
                var roles = new List<string> { "Client", "Employee", "Administrator" };
                var token = _jwtTokenService.GenerateToken(123, roles);
                _userLoggedIdsQueue.Enqueue(123);
                _kafkaProducer.SendMessageAsync("after-login-email-topic", "balsamb@uek.krakow.pl");
                return token;
            }else
            {
                throw new InvalidCredentialsException();
            }

        }




    }
}
