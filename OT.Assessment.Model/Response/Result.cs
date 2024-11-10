using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Model.Response
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        protected Result(bool isSuccess, string? error = null)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true);
        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(T data) : base(true)
        {
            Data = data;
        }

        private Result(string error) : base(false, error)
        {
            Data = default;
        }

        public static Result<T> Success(T data) => new(data);
        public static Result<T> Failure(string error) => new(error);
    }
}
