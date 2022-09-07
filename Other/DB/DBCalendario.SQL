use
DBPolifemo;


/*tabella giorno*/
create table Days
(
    giorno date primary key
)engine innodb;

/*tabella tipologia*/
create table Types
(
    id_tipologia int unsigned primary key auto_increment,
    `name`ENUM ("Festivo", "Esame", "Esame di profitto", "Lauree Magistrali", "Lezione", "Sabato", "Lauree 1 liv", "Altre attività", "Vacanza", "Prova in itinere") NOT NULL
)engine innodb;

/*tabella appartiene*/
create table appartiene
(
    giorno       date,
    foreign key (giorno) references Days (giorno),
    id_tipologia int unsigned,
    foreign key (id_tipologia) references Types (id_tipologia)
)engine innodb;

alter table appartiene
    add constraint appartiene_pk
        primary key (giorno, id_tipologia);


/*tabella lezione*/
create table Lessons
(
    id_lesson    int unsigned primary key auto_increment NOT NULL,
    `name`        varchar(100),
    aula         varchar(100),
    orarioInizio time,
    orarioFine   time,
    professore   varchar(200)
)engine innodb;

/*tabella presente*/
create table presente
(
    giorno    date,
    foreign key (giorno) references Days (giorno),
    id_lesson int unsigned,
    foreign key (id_lesson) references Lessons (id_lesson)
)engine innodb;

/*tabella esame*/
create table Exams
(
    id_exam  int unsigned primary key auto_increment NOT NULL,
    insegnamento    varchar(200),
    sede     varchar(100),
    semestre int,
    docente  varchar(500),
    orario     time,
    giorno   date,
    foreign key (giorno) references Days (giorno),
    lista varchar(1000)
)engine innodb;
