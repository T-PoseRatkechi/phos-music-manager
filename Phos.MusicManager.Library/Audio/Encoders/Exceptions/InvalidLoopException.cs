namespace Phos.MusicManager.Library.Audio.Encoders.Exceptions;

using Phos.MusicManager.Library.Audio.Models;

#pragma warning disable SA1600 // Elements should be documented
public class InvalidLoopException : Exception
{
    public InvalidLoopException(Loop loop)
    {
        this.Loop = loop;
    }

    public Loop Loop { get; }

    public override string Message
        => $"End sample must be greater than start sample.\nStart Sample: {this.Loop.StartSample}\nEnd Sample: {this.Loop.EndSample}";
}
