drop database if exists DBCalendario;
create database DBCalendario;
use DBCalendario;


/*tabella giorno*/
create table day(
giorno date primary_key
)engine innodb;

/*tabella tipologia*/
create table tipologia(
id_tipologia int unsigned primary key auto_increment NOT NULL,	
name_ ENUM ("Festivo", "Esame", "Esame di profitto", "Lauree Magistrali", "Lezione", "Sabato", "Laureee 1 liv")
)engine innodb;

/*tabella appartiene*/
create table appartiene(
giorno int unsigned,
foreign key(giorno) references day(giorno),
id_tipologia int unsigned,
foreign key(id_tipologia) references tipologia(id_tipologia)
)engine innodb;

ALTER TABLE appartiene ADD PRIMARY KEY (giorno, id_tipologia);

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
giorno int unsigned,
foreign key(giorno) references day(giorno),
id_lesson int unsigned,
foreign key(id_lesson) references lesson(id_lesson)
)engine innodb;

/*tabella esame*/
create table exam(
id_exam int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50),
sede varchar(100),
semestre int,
docente varchar(500),
data datetime,
giorno int unsigned,
foreign key(giorno) references day(giorno)
)engine innodb;



