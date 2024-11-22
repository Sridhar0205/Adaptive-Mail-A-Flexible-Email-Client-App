// ---------------------------------------
// Copyright (c) 2024 sridharv.
// Made with love for the .NET Community
// ---------------------------------------

using System.Threading.Tasks;
using FlexiMail.Models.Foundations.Messages;

namespace FlexiMail.Services
{
    internal interface IFlexiExchangeService
    {
        ValueTask SendAndSaveCopyAsync(FlexiMessage flexiMessage);
    }
}