// ---------------------------------------
// Copyright (c) 2024 sridharv.
// Made with love for the .NET Community
// ---------------------------------------

using Xeptions;

namespace FlexiMail.Models.Foundations.Messages.Exceptions
{
    public class FlexiMessageValidationException(string message, Xeption innerException)
        : Xeption(message, innerException)
    {
    }
}