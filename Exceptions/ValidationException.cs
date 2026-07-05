namespace EcommerceMVC.Exceptions;

public sealed class ValidationException(string message) : Exception(message);
