using System.Diagnostics;
using BackEnd.Models;
using MediatR;

namespace BackEnd.Features.SendRequest.getRequest;

public class SendRequestCommandHandler : IRequestHandler<SendRequestCommand , ResponseApi>
{
    private readonly HttpClient _httpClient;

    public SendRequestCommandHandler(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseApi> Handle(SendRequestCommand request, CancellationToken cancellationToken)
    {
        if (request.Method == "GET")
        {
            
            var stopwatch = Stopwatch.StartNew();
            var response = await _httpClient.GetAsync(request.Url , cancellationToken);
            stopwatch.Stop();


            return new ResponseApi
            {
                StatusCode = response.StatusCode,
                ResponseTime = stopwatch.ElapsedMilliseconds,
                Body = await response.Content.ReadAsStringAsync(cancellationToken)
            };
        }
        return new ResponseApi();
    }
}