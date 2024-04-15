create table Proj_Comment (
	CommentID int identity(1,1) primary key,
	UserID int references Proj_Users(UserID) not null,
	PerformerID int references Proj_Performer(PerformerID) not null,
	CommentContent nvarchar(256) not null,
	CommentDate date default getdate()
)
create table Proj_Following (
	UserID int references Proj_Users(UserID) not null,
	PerformerID int references Proj_Performer(PerformerID) not null,
	primary key (UserID, PerformerID)
)