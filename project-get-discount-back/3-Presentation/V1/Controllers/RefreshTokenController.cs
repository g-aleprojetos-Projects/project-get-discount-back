using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project_get_discount_back._1_Domain.Dtos;
using project_get_discount_back._2_Infrastructure.Schemas.Responses;
using project_get_discount_back.Queries;
using project_get_discount_back.Results;
using project_get_discount_back.ViewModel;

namespace project_get_discount_back._3_Presentation.V1.Controllers
{
    /// <summary>
    /// Controller para atualizar o token.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do RefreshTokenController.
        /// </summary>
        /// <param name="mediator">Mediator para enviar a query do refreshToken.</param>
        /// <param name="mapper">mapeia o request do token.</param>
        public RefreshTokenController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Atualiza Token.
        /// </summary>
        /// <param name="token">O refresh token do usuário.</param>
        /// <param name="cancellationToken">O token de cancelamento de operação assíncrona.</param>
        /// <returns>O resultado envia um token novo para o usuario.</returns>
        [HttpPost("/refresh-token")]
        [Authorize]
        public async Task<ActionResult> Post([FromBody] TokenDto token, CancellationToken cancellationToken)
        {
            var query = _mapper.Map<GetTokenQuery>(token);
            var result = await _mediator.Send(query, cancellationToken);
            if (result is Fail<TokenViewModel> fail)
            {
                return BadRequest(new ErrosResponse(fail.Code, fail.Message));
            }
            return Ok(result);
        }
    }
}
