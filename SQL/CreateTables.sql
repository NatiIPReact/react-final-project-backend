Create Table Proj_Song (
SongID int identity(1,1) primary key,
SongName nvarchar(50) not null,
SongLyrics nvarchar(2000) not null,
SongLengthSeconds int
)

create table Proj_Genre (
GenreID int identity(1,1) primary key,
GenreName nvarchar(100)
)

create table Proj_SongVersion (
SongID int foreign key references Proj_Song(SongID),
VersionID int identity(1,1),
GenreID int references Proj_Genre(GenreID)
primary key(SongID, VersionID)
)

create table Proj_Users (
UserID int identity(1,1) primary key,
UserEmail nvarchar(50) check (UserEmail like '%@%.%') unique,
UserName nvarchar(50),
UserPassword nvarchar(100) check (len(UserPassword) >= 3),
UserIsVerified bit default 0 not null,
UserToken varchar(16) not null,
LastTokenTime datetime
)

create table Proj_Playlist(
PlaylistID int identity(1,1) primary key,
PlaylistName nvarchar(100) not null,
UserID int references Proj_Users(UserID)
)

create table Proj_Performer (
PerformerID int identity(1,1) primary key,
PerformerName nvarchar(100) not null,
isABand bit not null
)

create table Proj_Artist(
ArtistID int references Proj_Performer(PerformerID) primary key,
birthDate date
)

create table Proj_Band(
BandID int references Proj_Performer(PerformerID) primary key,
establishmentDate date
)

create table Proj_ArtistInBand (
BandID int references Proj_Band(BandID),
ArtistID int references Proj_Artist(ArtistID)
primary key (BandID, ArtistID)
)

create table Proj_PerformerSongVersion (
SongID int,
VersionID int,
PerformerID int references Proj_Performer(PerformerID),
TimesPlayed int default 0,
foreign key (SongID, VersionID) references Proj_SongVersion(SongID, VersionID),
primary key (SongID, VersionID, PerformerID)
)

create table Proj_Show (
ShowID int identity(1,1) primary key,
ShowPrice float,
ShowDate date,
ShowPlace nvarchar(200)
)

create table Proj_ShowOf(
PerformerID int references Proj_Performer(PerformerID),
ShowID int references Proj_Show(ShowID),
primary key (PerformerID, ShowID)
)

create table Proj_UserFavorites(
UserID int references Proj_Users(UserID),
PerformerID int,
SongID int,
VersionID int,
foreign key (SongID, VersionID, PerformerID) references Proj_PerformerSongVersion(SongID, VersionID, PerformerID),
primary key (UserID, SongID, VersionID, PerformerID)
)

create table Proj_SongVersionInPlaylist (
PlaylistID int references Proj_Playlist(PlaylistID),
PerformerID int,
SongID int,
VersionID int,
foreign key (SongID, VersionID, PerformerID) references Proj_PerformerSongVersion(SongID, VersionID, PerformerID),
primary key (PlaylistID, SongID, VersionID, PerformerID)
)

/*drop table if exists Proj_SongVersionInPlaylist
drop table if exists Proj_UserFavorites
drop table if exists Proj_ShowOf
drop table if exists Proj_Show
drop table if exists Proj_PerformerSongVersion
drop table if exists Proj_ArtistInBand
drop table if exists Proj_Band
drop table if exists Proj_Artist
drop table if exists Proj_Performer
drop table if exists Proj_Playlist
drop table if exists Proj_Users
drop table if exists Proj_SongVersion
drop table if exists Proj_Genre
drop table if exists Proj_Song*/