using Core.Business.Services.Base;
using Core.Domain.Models;
using MediatR;
using Core.Infrastructure.Helper;
using Core.Domain.Entities;

namespace Taurus.Core.Business.CQRS.Execution.Command
{
    public class CreateExecutionCommand : IRequest<Response>
    {
        public int PageNumbers { get; set; }

        public class CreateExecutionCommandHandler : IRequestHandler<CreateExecutionCommand, Response>
        {
            private readonly IServiceBase<ExecutionEntity> _service;
            public CreateExecutionCommandHandler(IServiceBase<ExecutionEntity> service)
            {
                _service = service;
            }

            public async Task<Response> Handle(CreateExecutionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    await _service.InsertAsync(new ExecutionEntity
                    {
                        StartDate = DateTime.UtcNow,
                        PageNumbers = request.PageNumbers,
                    });

                    var execution = await _service.ListAsync();

                    return new Response(execution.Last());
                }
                catch (Exception e)
                {
                    throw new AppException(e.Message, null);
                }
            }
        }
    }
}
