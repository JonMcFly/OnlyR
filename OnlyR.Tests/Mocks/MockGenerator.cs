﻿namespace OnlyR.Tests.Mocks
{
    using Moq;
    using Services.Audio;
    using Services.Options;
    using Services.RecordingDestination;

    public static class MockGenerator
    {
        public static IAudioService CreateAudioService()
        {
            return new MockAudioService();
        }

        public static Mock<IOptionsService> CreateOptionsService()
        {
            var m = new Mock<IOptionsService>();
            m.Setup(o => o.Options).Returns(new Options());

            return m;
        }

        public static Mock<IRecordingDestinationService> CreateRecordingsDestinationService()
        {
            return new Mock<IRecordingDestinationService>();
        }

        public static Mock<ICommandLineService> CreateCommandLineService()
        {
            return new Mock<ICommandLineService>();
        }
    }
}
