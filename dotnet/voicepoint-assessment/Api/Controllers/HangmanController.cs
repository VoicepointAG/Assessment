using Microsoft.AspNetCore.Mvc;
using Voicepoint.Assessment.Api.Models;
using Voicepoint.Assessment.Services;

namespace Voicepoint.Assessment.Api.Controllers
{
    [ApiController]
    [Route("api/hangman")]
    public class HangmanController : ControllerBase
    {
        private readonly IHelloService _helloService;

        public HangmanController(IHelloService helloService)
        {
            _helloService = helloService;
        }

        [HttpPost]
        [Route("say-hello")]
        public SayHelloResponse SayHello(string name)
        {
            return _helloService.SayHello(name);
        }
    }
}
