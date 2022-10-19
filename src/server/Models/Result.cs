﻿namespace V.TouristGuide.Server.Models
{
    public class Result
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public static Result<T> Success<T>(T data) => new Result<T>
        {
            Code = 0,
            Data = data
        };
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }

        public static Result<T> Fail(int code = -1, string msg = null) => new Result<T>
        {
            Code = code,
            Msg = msg
        };
    }

    public class PagedResult<T> : Result
    {
        public List<T> Data { get; set; }

        public int TotalCount { get; set; }

        public double PageCount { get; set; }
    }
}
