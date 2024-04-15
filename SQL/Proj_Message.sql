create table Proj_Message (
MessageID int identity(1,1) primary key,
subject nvarchar(100),
content nvarchar(1000),
dateOfMessage date,
UserID int references Proj_Users(UserID)
)