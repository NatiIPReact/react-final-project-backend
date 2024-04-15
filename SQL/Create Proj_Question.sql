create table Proj_Quiz (
	QuizID int identity (1,1) primary key,
	UserID int references Proj_Users(UserID)
)
create table Proj_Question (
	QuestionID int identity (1,1) primary key,
	content nvarchar(200) not null,
	answer1 nvarchar(200) not null,
	answer2 nvarchar(200) not null,
	answer3 nvarchar(200) not null,
	answer4 nvarchar(200) not null,
	correctAnswer int not null check (correctAnswer between 0 and 3),
	QuizID int references Proj_Quiz(QuizID)
)
drop table if exists Proj_Question
drop table if exists Proj_Quiz