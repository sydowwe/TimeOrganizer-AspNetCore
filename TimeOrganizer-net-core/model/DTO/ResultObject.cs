using TimeOrganizer_net_core.helper;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.DTO.response.user;

namespace TimeOrganizer_net_core.model.DTO;


public class ServiceResult
{
    public bool Success { get; set; }
    public ServiceResultErrorType? ErrorType { get; set; }
    public string? ErrorMessage { get; set; }

    public static ServiceResult Successful()
    {
        return new ServiceResult
        {
            Success = true,
        };
    }
    
    public static ServiceResult Error(ServiceResultErrorType? errorType, string? errorMessage)
    {
        return new ServiceResult
        {
            Success = false,
            ErrorType = errorType,
            ErrorMessage = errorMessage,
        };
    }
}
public class ServiceResult<T> : ServiceResult where T : class 
{
    public T Data { get; set; }

    public static ServiceResult<T> Successful(T data)
    {
        return new ServiceResult<T>
        {
            Success = true,
            Data = data
        };
    }

    public new static ServiceResult<T> Error(ServiceResultErrorType? errorType, string? errorMessage)
    {
        return new ServiceResult<T>
        {
            Success = false,
            ErrorType = errorType,
            ErrorMessage = errorMessage,
        };;
    }

}