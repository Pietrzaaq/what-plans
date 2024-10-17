using MediatR;
using Microsoft.Extensions.Logging;

namespace WhatPlans.Infrastructure.Logging.Decorators;

public class LoggingRequestHandlerDecorator<TRequest> : IRequestHandler<TRequest> where TRequest : class, IRequest
{
    private readonly IRequestHandler<TRequest> _commandHandler;
    private readonly ILogger<LoggingRequestHandlerDecorator<TRequest>> _logger;

    public LoggingRequestHandlerDecorator(IRequestHandler<TRequest> commandHandler, ILogger<LoggingRequestHandlerDecorator<TRequest>> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    public async Task Handle(TRequest request, CancellationToken cancellationToken)
    {
        var commandName = typeof(TRequest).FullName;
        _logger.LogInformation("Started handling a request: {CommandName}...", commandName);
        await _commandHandler.Handle(request, cancellationToken);
        _logger.LogInformation("Completed handling a request: {CommandName}.", commandName);
    }
}