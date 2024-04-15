create table Proj_Genre (
GenreID int identity(1,1) primary key,
GenreName nvarchar(100)
)
 
/*Create Table Proj_Songs (
SongID int identity(1,1) primary key,
SongName nvarchar(50) not null,
SongLyrics nvarchar(2000) not null,
FileData varbinary(max),
GenreID int references Proj_Genre(GenreID)
)*/

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
isABand bit not null,
PerformerImage VARBINARY(MAX)
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

create table Proj_PerformerSong (
SongID int foreign key references Proj_Songs(SongID),
VersionID int,
PerformerID int references Proj_Performer(PerformerID),
TimesPlayed int default 0,
primary key (SongID, PerformerID)
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
foreign key (SongID, PerformerID) references Proj_PerformerSong(SongID, PerformerID),
primary key (UserID, SongID, VersionID, PerformerID)
)
 
create table Proj_SongInPlaylist (
PlaylistID int references Proj_Playlist(PlaylistID),
PerformerID int,
SongID int,
VersionID int,
foreign key (SongID, PerformerID) references Proj_PerformerSong(SongID, PerformerID),
primary key (PlaylistID, SongID, VersionID, PerformerID)
)
 
/*drop table if exists Proj_SongInPlaylist
drop table if exists Proj_UserFavorites
drop table if exists Proj_ShowOf
drop table if exists Proj_Show
drop table if exists Proj_PerformerSong
drop table if exists Proj_ArtistInBand
drop table if exists Proj_Band
drop table if exists Proj_Artist
drop table if exists Proj_Performer
drop table if exists Proj_Playlist
drop table if exists Proj_Users
drop table if exists Proj_SongVersion
drop table if exists Proj_Songs
drop table if exists Proj_Genre*/