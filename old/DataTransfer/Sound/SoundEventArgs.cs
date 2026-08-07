namespace DataTransfer.Sound;

public record SoundStartedEventArgs(string Identifier, bool IsLooped, bool IsFaded);

public record SoundStoppedEventArgs(string Identifier, bool IsFaded);
