create table Proj_RecentlyPlayed (
RPID int identity(1,1) primary key,
UserID int references Proj_Users(UserID) not null,
SongID int references Proj_Song(SongID) not null,
DateTimePlayed datetime default getdate()
)