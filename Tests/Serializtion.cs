using NUnit.Framework;
using ProtoBuf;
using Shared.Messages;
using System;
using System.IO;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SerializationTest()
        {
            var messageToSerialize = new CreateGameMessage
            {
                Players = 5
            };

            var messageStream = new MemoryStream();
            Serializer.Serialize(messageStream, messageToSerialize);

            var messageCarrier = new MessageCarrier
            {
                MessageType = messageToSerialize.GetType(),
                Message = messageStream.ToArray()
            };

            var messageCarrierStream = new MemoryStream();
            Serializer.Serialize(messageCarrierStream, messageCarrier);

            messageCarrierStream.Position = 0;

            var deserializedMessageCarrier = Serializer.Deserialize<MessageCarrier>(messageCarrierStream);
            var messageToDeserialize = new MemoryStream(deserializedMessageCarrier.Message);
            var deserializedMessage = Serializer.Deserialize(deserializedMessageCarrier.MessageType, messageToDeserialize);
            var createGameMessage = deserializedMessage as CreateGameMessage;

            Assert.AreEqual(5, createGameMessage.Players);
        }
    }
}
