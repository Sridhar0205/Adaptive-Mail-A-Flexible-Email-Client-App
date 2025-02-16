// ---------------------------------------
// Copyright (c) 2024 sridharv.
// Made with love for the .NET Community
// ---------------------------------------

using FlexiMail.Models.Foundations.Messages;
using FlexiMail.Models.Foundations.Messages.Exceptions;
using FluentAssertions;
using Microsoft.Exchange.WebServices.Data;
using Moq;

namespace FlexiMail.Tests.Unit.Services
{
    public partial class FlexiExchangeServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionIfFlexiMessageIsNull()
        {
            // given 
            FlexiMessage nullMessage = null;

            var nullFlexiMessageException =
                new NullFlexiMessageException(
                    message: "FlexiMessage is null.");

            var expectedFlexiMessageValidationException =
                new FlexiMessageValidationException(
                    message: "Flexi Message validation error occurred, fix errors and try again.",
                    innerException: nullFlexiMessageException);

            // when
            var sendMessageTask =
                this.flexiExchangeService.SendAndSaveCopyAsync(nullMessage);

            var actualFlexiMessageValidationException =
                await Assert.ThrowsAsync<FlexiMessageValidationException>(
                    sendMessageTask.AsTask);
            // then

            actualFlexiMessageValidationException.Should()
                .BeEquivalentTo(expectedFlexiMessageValidationException);

            this.exchangeBrokerMock.Verify(broker =>
                    broker.GetAccessTokenAsync(),
                Times.Never);

            this.exchangeBrokerMock.Verify(broker =>
                    broker.CreateExchangeService(
                        ExchangeVersion.Exchange2013,
                        It.IsAny<string>(),
                        It.IsAny<ImpersonatedUserId>()),
                Times.Never);

            this.exchangeBrokerMock.Verify(broker =>
                    broker.SendAndSaveCopy(It.IsAny<EmailMessage>()),
                Times.Never);
        }

        [Fact]
        public async void ShouldThrowValidationExceptionIfNoReceiverWasAdded()
        {
            // given 
            var randomFlexiMessage = CreateRandomFlexiMessage();

            randomFlexiMessage.To = null;
            randomFlexiMessage.Cc = null;
            randomFlexiMessage.Bcc = null;

            var inputFlexiMessage = randomFlexiMessage;

            var invalidFlexiMessageException =
                new InvalidFlexiMessageException(
                    message: "Invalid message error occurred, fix errors and try again.");

            invalidFlexiMessageException.AddData(
                key: nameof(FlexiMessage.To),
                values: "Value is not set");

            invalidFlexiMessageException.AddData(
                key: nameof(FlexiMessage.Cc),
                values: "Value is not set");

            invalidFlexiMessageException.AddData(
                key: nameof(FlexiMessage.Bcc),
                values: "Value is not set");

            var expectedFlexiMessageValidationException =
                new FlexiMessageValidationException(
                    message: "Flexi Message validation error occurred, fix errors and try again.",
                    innerException: invalidFlexiMessageException);

            // when
            var sendMessageTask =
                this.flexiExchangeService.SendAndSaveCopyAsync(inputFlexiMessage);

            var actualFlexiMessageValidationException =
                await Assert.ThrowsAsync<FlexiMessageValidationException>(
                    sendMessageTask.AsTask);
            // then

            actualFlexiMessageValidationException.Should()
                .BeEquivalentTo(expectedFlexiMessageValidationException);

            this.exchangeBrokerMock.Verify(broker =>
                    broker.GetAccessTokenAsync(),
                Times.Never);

            this.exchangeBrokerMock.Verify(broker =>
                    broker.CreateExchangeService(
                        ExchangeVersion.Exchange2013,
                        It.IsAny<string>(),
                        It.IsAny<ImpersonatedUserId>()),
                Times.Never);

            this.exchangeBrokerMock.Verify(broker =>
                    broker.SendAndSaveCopy(It.IsAny<EmailMessage>()),
                Times.Never);
        }
    }
}