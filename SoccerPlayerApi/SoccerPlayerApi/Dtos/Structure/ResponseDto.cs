namespace SoccerPlayerApi.Dtos.Structure
{
    public class ResponseDto<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
