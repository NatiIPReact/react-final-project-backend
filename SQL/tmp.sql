select * from Proj_UserFavorites
select * from Proj_Songs

ALTER TABLE Proj_PerformerSong
ADD SongName nvarchar(150) not null,
    SonyLyrics nvarchar(4000) not null,
    GenreID int,
    ReleaseYear int;


CREATE TABLE Temp_PerformerSong (
    SongID int,
    SongName nvarchar(150) not null,
    SonyLyrics nvarchar(4000) not null,
    GenreID int,
    ReleaseYear int,
    PerformerID int,
    FileData varbinary(max),
    NumOfPlays int,
    SongLength char(10),
    CONSTRAINT PK_Temp_PerformerSong PRIMARY KEY (SongID)
);
INSERT INTO Temp_PerformerSong (SongID, SongName, SonyLyrics, GenreID, ReleaseYear)
SELECT SongID, SongName, SongLyrics, GenreID, ReleaseYear
FROM Proj_Songs;
ALTER TABLE Temp_PerformerSong
ADD CONSTRAINT FK_Temp_PerformerSong_Songs
FOREIGN KEY (SongID)
REFERENCES Proj_Songs (SongID);
UPDATE tps
SET tps.PerformerID = pps.PerformerID,
    tps.FileData = pps.FileData,
    tps.NumOfPlays = pps.NumOfPlays,
    tps.SongLength = pps.SongLength
FROM Temp_PerformerSong tps
JOIN Proj_PerformerSong pps ON tps.SongID = pps.SongID;
select * from Temp_PerformerSong
select * from Proj_Songs
select * from Proj_PerformerSong
DROP TABLE Proj_PerformerSong;

DROP TABLE Proj_Songs;
ALTER TABLE Proj_PerformerSong
DROP CONSTRAINT DF_Proj_Perf_SongL_58DC1D15
ALTER TABLE Proj_Songs
DROP CONSTRAINT DF_Proj_Perf_SongL_58DC1D15

ALTER TABLE Proj_Songs
DROP CONSTRAINT [FK_Proj_Songs_Genre];
