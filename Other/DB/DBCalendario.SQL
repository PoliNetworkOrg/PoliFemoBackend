drop database if exists DBCalendario;
create database DBCalendario;
use DBCalendario;


/*tabella calendario scolastico*/
create table calendar(
id_calendar int unsigned primary key auto_increment NOT NULL
)engine innodb;


/*tabella giorno*/
create table day(
id_day int unsigned primary key auto_increment  NOT NULL
)engine innodb;

/*tabella tipologia*/
create table tipologia(
id_tipologia int unsigned primary key auto_increment NOT NULL,	
name_ ENUM ("Festivo", "Esame", "Prova itinere", "Lauree Magistrali", "Lezione", "Sabato", "Laureee 1 liv")
)engine innodb;

/*tabella appartiene*/
create table appartiene(
id_day int unsigned,
foreign key(id_day) references day(id_day),
id_tipologia int unsigned,
foreign key(id_tipologia) references tipologia(id_tipologia)
)engine innodb;

/*tabella lezione*/
create table lesson(
id_lesson int unsigned primary key auto_increment NOT NULL,	
name_ varchar(100),
aula varchar(100),
orarioInizio time,
orarioFine time,
professore varchar(200)
)engine innodb;

/*tabella presente*/
create table presente(
id_day int unsigned,
foreign key(id_day) references day(id_day),
id_lesson int unsigned,
foreign key(id_lesson) references lesson(id_lesson)
)engine innodb;

/*tabella esame*/
create table exam(
id_exam int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50),
sede varchar(100),
semetre int,
docente varchar(500),
data datetime,
id_day int unsigned,
foreign key(id_day) references day(id_day)
)engine innodb;



