namespace Ediki.Domain.Enums;

public enum DemoSessionType
{
    ProductDemo = 1,
    Training = 2,
    Webinar = 3,
    ClientPresentation = 4,
    TeamMeeting = 5,
    Workshop = 6
}

public enum DemoSessionStatus
{
    Scheduled = 1,
    Starting = 2,
    Live = 3,
    Paused = 4,
    Ended = 5,
    Cancelled = 6
}

public enum DemoParticipantRole
{
    Host = 1,
    CoHost = 2,
    Presenter = 3,
    Participant = 4,
    Observer = 5
}

public enum DemoParticipantStatus
{
    Invited = 1,
    Joined = 2,
    Left = 3,
    Kicked = 4,
    Disconnected = 5
}

public enum DemoMessageType
{
    Chat = 1,
    Question = 2,
    SystemMessage = 3,
    Announcement = 4,
    Poll = 5
}

public enum DemoActionType
{
    Join = 1,
    Leave = 2,
    StartRecording = 3,
    StopRecording = 4,
    ShareScreen = 5,
    StopScreenShare = 6,
    SendMessage = 7,
    CreateTask = 8,
    UpdateTask = 9,
    CreateProject = 10,
    Navigate = 11,
    Annotation = 12
}

public enum RecordingQuality
{
    SD = 1,      // 480p
    HD = 2,      // 720p
    FullHD = 3,  // 1080p
    UHD = 4      // 4K
}

public enum RecordingStatus
{
    Starting = 1,
    Recording = 2,
    Processing = 3,
    Completed = 4,
    Failed = 5,
    Cancelled = 6
} 