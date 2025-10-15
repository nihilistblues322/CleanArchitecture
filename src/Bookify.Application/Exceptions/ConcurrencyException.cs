namespace Bookify.Application.Exceptions;

public sealed class ConcurrencyException(string message, Exception inner) : Exception(message, inner);