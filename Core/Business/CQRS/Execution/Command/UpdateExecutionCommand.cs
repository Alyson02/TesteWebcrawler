using MediatR;
using Core.Business.Services.Base;
using Core.Domain.Models;
using Core.Infrastructure.Helper;
using Core.Domain.Entities;

namespace Taurus.Core.Business.CQRS.Execution.Command
{
    public class UpdateExecutionCommand : IRequest<Response>
    {
        public int ExecutionId { get; set; }
        public int LineNumbers { get; set; }
        public string JsonFile { get; set; }

        public class UpdateExecutionCommandHandler : IRequestHandler<UpdateExecutionCommand, Response>
        {
            private readonly IServiceBase<ExecutionEntity> _service;
            public UpdateExecutionCommandHandler(IServiceBase<ExecutionEntity> service)
            {
                _service = service;
            }

            public async Task<Response> Handle(UpdateExecutionCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var spec = _service.CreateSpec(x => x.ExecutionId == request.ExecutionId);
                    var execution = await _service.FindAsync(spec, cancellationToken);

                    if (execution != null)
                    {
                        execution.EndDate = DateTime.UtcNow;
                        execution.LineNumbers = request.LineNumbers;
                        execution.JsonFile = request.JsonFile;

                        await _service.UpdateAsync(execution);

                        return new Response("Execuçao atualizada com sucesso");
                    }

                    throw new Exception("Execuçao nao encontrada");
                }
                catch (Exception e)
                {
                    throw new AppException(e.Message, null);
                }
            }
        }
    }
}
