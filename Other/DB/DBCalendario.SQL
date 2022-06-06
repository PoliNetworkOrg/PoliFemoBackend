
use DBPolifemo;


/*tabella giorno*/
create table Days(
giorno date primary key
)engine innodb;

/*tabella tipologia*/
create table Types(
id_tipologia int unsigned primary key auto_increment,	
name_ ENUM ("Festivo", "Esame", "Esame di profitto", "Lauree Magistrali", "Lezione", "Sabato", "Laureee 1 liv")
)engine innodb;

/*tabella appartiene*/
create table appartiene(
giorno date,
foreign key(giorno) references Days(giorno),
id_tipologia int unsigned,
foreign key(id_tipologia) references Types(id_tipologia)
)engine innodb;

ALTER TABLE appartiene ADD PRIMARY KEY (giorno, id_tipologia);

/*tabella lezione*/
create table Lessons(
id_lesson int unsigned primary key auto_increment NOT NULL,	
name_ varchar(100),
aula varchar(100),
orarioInizio time,
orarioFine time,
professore varchar(200)
)engine innodb;

/*tabella presente*/
create table presente(
giorno date,
foreign key(giorno) references Days(giorno),
id_lesson int unsigned,
foreign key(id_lesson) references Lessons(id_lesson)
)engine innodb;

/*tabella esame*/
create table Exams(
id_exam int unsigned primary key auto_increment NOT NULL,	
name_ varchar(50),
sede varchar(100),
semestre int,
docente varchar(500),
data datetime,
giorno date,
foreign key(giorno) references Days(giorno)
)engine innodb;
